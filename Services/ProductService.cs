using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using ProjectJWTeCommerce.Models.ProductAPIs;
using ProjectJWTeCommerce.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProjectJWTeCommerce.Services
{
    public class ProductService : IProductRepository
    {
        public async Task<IEnumerable<Product>> AddProduct(int sellerId, Product product)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                try
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        var productQuery = @"INSERT INTO Product (SId, PTitle, PCategory, PImageURL, PDescription, Image, PPrice, Quantity)
                                    OUTPUT INSERTED.PId
                                    VALUES(@sellerid, @title, @category, @imgURL, @description, @image, @price, @quantity) ;
                                    UPDATE sellers SET NoOfProducts = NoOfProducts + 1 WHERE SId = @sellerid ;";
                        var productId = await connection.ExecuteScalarAsync<int>(productQuery, new
                        {
                            sellerid = sellerId,
                            title = product.PTitle,
                            category = product.PCategory,
                            imgURL = "http://sample_img.com",
                            description = product.PDescription,
                            image = "sample_img",
                            price = product.PPrice,
                            quantity = product.Quantity
                        }, transaction);
                        if (productId == null)
                            throw new Exception("productId not found");

                        var productValue =  await connection.QuerySingleAsync<Product>(
                               "SELECT * FROM Product WHERE PId = @PId",
                               new { PId = productId },
                               transaction
                        );

                        if (productValue == null)
                            throw new Exception("Failed to retrieve the inserted product.");


                        foreach (var feature in product.features)
                        {
                            var featureQuery = "INSERT INTO Features (PId, FName) VALUES (@PId, @FeatureName);";
                            await connection.ExecuteAsync(featureQuery, new { PId = productValue.Pid, FeatureName = feature.FName }, transaction);
                        }

                        // Insert multiple ItemQuantities
                        foreach (var item in product.itemQuantities)
                        {
                            var itemQuantityQuery = "INSERT INTO ItemQuantity (PId, Quantity) VALUES (@PId, @Quantity);";
                            await connection.ExecuteAsync(itemQuantityQuery, new { PId = productValue.Pid, Quantity = item.Quantity }, transaction);
                        }
                        transaction.Commit();
                    }
                    var productquery = @"SELECT * FROM Product WHERE SId=@id";
                    var productValues = await connection.QueryAsync<Product>(productquery, new { id = sellerId });
                    return productValues;
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return null;
                }
            }

        }

        public async Task DeleteProduct(int sellerId, int productId)
        {
            int price = 0;
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                try
                {
                    using(var transaction = connection.BeginTransaction())
                    {
                        var query = @"
                                    DECLARE @price INT;
                                    
                                    SELECT @price = p.PPrice FROM Product p WHERE p.Pid = @productId;
                                    SELECT @price AS Price;";

                        var result = await connection.QueryFirstOrDefaultAsync(query, new { productId = productId}, transaction);
                        if (result != null)
                            price = result.Price;

                        var fetchItemQuantities = @"SELECT * FROM ItemQuantity q WHERE q.PId = @productId;";
                        var items = await connection.QueryAsync<ItemQuantity>(fetchItemQuantities, new {productId = productId}, transaction);

                        foreach (var item in items)
                        {
                            var correctingTheCost = @"UPDATE Cart SET TotalCost = TotalCost - (@quantity * @price) WHERE CId = @cid;";
                            await connection.ExecuteAsync(correctingTheCost, new {quantity = item.Quantity, price = price, cid = item.CId}, transaction);
                        }

                        var deleteQuery = @"
                                    DELETE FROM Product WHERE Pid = @productId AND SId = @sellerId;

                                    -- Update Seller's Product Count
                                    UPDATE sellers SET NoOfProducts = NoOfProducts - 1 WHERE SId = @sellerId;";
                        await connection.ExecuteAsync(deleteQuery, new {sellerId = sellerId, productId = productId}, transaction);



                        transaction.Commit();

                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts(int pageNumber, int pageSize)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                try
                {
                    var query = @"
                SELECT * FROM Product p
                LEFT JOIN ItemQuantity iq ON p.Pid = iq.PId
                LEFT JOIN Features f ON p.Pid = f.PId
                ORDER BY p.Pid
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

                    var productDictionary = new Dictionary<int, Product>();

                    var products =connection.Query<Product, ItemQuantity, Features, Product>(
                        query,
                        (product, itemQuantity, feature) =>
                        {
                            if (!productDictionary.TryGetValue(product.Pid, out var prod))
                            {
                                prod = product;
                                prod.itemQuantities = new List<ItemQuantity>();
                                prod.features = new List<Features>();
                                productDictionary.Add(prod.Pid, prod);
                            }
                            if (itemQuantity != null)
                                prod.itemQuantities.Add(itemQuantity);
                            if (feature != null)
                                prod.features.Add(feature);

                            return prod;
                        },
                        new { Offset = (pageNumber - 1) * pageSize, PageSize = pageSize },
                        splitOn: "QId, FId"
                    ).Distinct().ToList();

                    return products;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<Product> GetProduct(int productId)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                try
                {
                    var productQuery = @"SELECT * FROM Product WHERE PId = @productId;";
                    var productValue = await connection.QuerySingleOrDefaultAsync<Product>(productQuery, new { productId = productId });
                    if (productValue == null)
                        throw new Exception("Product not found");
                    return productValue;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }   
        }

        public async Task<IEnumerable<Product>> GetProductsOfSeller(int sellerId)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                try
                {
                    var productQuery = @"SELECT * FROM Product WHERE SId = @sellerId;";
                    var productValues = await connection.QueryAsync<Product>(productQuery, new { sellerId = sellerId });
                    return productValues;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<Product> UpdateProduct(int sellerId, int productId, Product product)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                try
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        var verifySeller = @"SELECT 1 FROM Product WHERE SId = @sellerId AND Pid = @productId;";
                        var returnVal = await connection.QuerySingleAsync<int>(verifySeller,new { sellerId = sellerId, productId = productId },transaction);
                        if (returnVal == null || returnVal == 0)
                            return null;
                        var productQuery = @"UPDATE Product SET PTitle = @title, PCategory = @category, PImageURL = @imgURL,
                                    PDescription = @description, Image = @image, PPrice = @price, Quantity = @quantity
                                    WHERE Pid = @productId;";
                        var productIdVal = await connection.ExecuteScalarAsync<int>(productQuery, new
                        {
                            productId = productId,
                            title = product.PTitle,
                            category = product.PCategory,
                            imgURL = "http://sample_img.com",
                            description = product.PDescription,
                            image = "sample_img",
                            price = product.PPrice,
                            quantity = product.Quantity
                        }, transaction);

                        if (productIdVal == null)
                            throw new Exception("productId not found");

                        foreach (var feature in product.features)
                        {
                            var featureQuery = "UPDATE Features SET FName = @FeatureName WHERE PId = @PId;";
                            await connection.ExecuteAsync(featureQuery, new { PId = productId, FeatureName = feature.FName }, transaction);
                        }

                        // Insert multiple ItemQuantities
                        foreach (var item in product.itemQuantities)
                        {
                            var itemQuantityQuery = "UPDATE ItemQuantity SET Quantity = @Quantity WHERE PId = @PId;";
                            await connection.ExecuteAsync(itemQuantityQuery, new { PId = productId, Quantity = item.Quantity }, transaction);
                        }
                        transaction.Commit();
                        var productValue = await connection.QuerySingleOrDefaultAsync<Product>(
                               "SELECT * FROM Product WHERE PId = @PId",
                               new { PId = productId }
                        );
                        return productValue;
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return null;
                }
            }
        }
    }
}
