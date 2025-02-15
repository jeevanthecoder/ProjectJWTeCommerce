using System.Diagnostics;
using Dapper;
using Microsoft.Data.SqlClient;
using ProjectJWTeCommerce.Models.CartAPIs;
using ProjectJWTeCommerce.Models.ProductAPIs;
using ProjectJWTeCommerce.Models.SellerAPIs;
using ProjectJWTeCommerce.Repositories;

namespace ProjectJWTeCommerce.Services
{
    public class CartService : ICartRepository
    {

        public Cart AddToCart(int userId, int productId, int addressId)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                try
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        var cart_query = @"DECLARE @InsertedCartId INT;

                                    IF NOT EXISTS (SELECT 1 FROM Cart WHERE userId = @userid AND addressId = @addressid)
                                    BEGIN
                                        INSERT INTO Cart (userId, addressId, TotalCost) VALUES (@userid, @addressid, @totalcost);
                                        SET @InsertedCartId = SCOPE_IDENTITY();
                                        UPDATE UserDetails SET cartId = @InsertedCartId WHERE UId = @userid ;
                                    END
                                    ELSE
                                    BEGIN
                                        SELECT @InsertedCartId = CId FROM Cart WHERE userId = @userid AND addressId = @addressid;
                                    END

                                    SELECT @InsertedCartId AS CartId;";
                        var cartId = connection.ExecuteScalar<int>(cart_query, new { totalcost = 0, userid = userId, addressid = addressId }, transaction);

                        if (cartId == 0)
                        {
                            throw new Exception("cart not created!!!");
                        }

                        var item_query = @"MERGE INTO ItemQuantity AS target
                                    USING (SELECT @ProductId AS PId, @IncrementBy AS Quantity, @cartid AS Cid) AS source
                                    ON target.PId = source.PId AND target.CId = source.Cid
                                    WHEN MATCHED THEN
                                        UPDATE SET target.quantity = target.quantity + source.Quantity
                                    WHEN NOT MATCHED THEN
                                        INSERT (PId, CId, quantity) VALUES (source.PId, @cartid, source.Quantity);";

                        connection.Execute(item_query, new { ProductId = productId, IncrementBy = 1, cartid = cartId }, transaction);

                        var incrementValue = @"SELECT * FROM Product WHERE Pid = @productId;";
                        var product = connection.QuerySingle<Product>(incrementValue, new { productId = productId }, transaction);

                        var costIncrement = @"UPDATE Cart SET TotalCost = TotalCost + @incrementValue;";
                        connection.Execute(costIncrement, new { incrementValue = product.PPrice }, transaction);

                        transaction.Commit();
                        var cartQuery = @"SELECT * FROM Cart WHERE CId = @cartId;";
                        var cart = connection.QuerySingle<Cart>(cartQuery, new { cartId = cartId }, transaction);

                        return cart;

                    }
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            return null;
        }

        public int RemoveFromCart(int userId, int productId)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                using(var transaction = connection.BeginTransaction())
                {
                    int cartId = 0, price = 0;
                    try
                    {
                        var query = @"DECLARE @cartid INT;
                                    DECLARE @price INT;
                                    
                                    SELECT @cartid = u.cartId FROM UserDetails u WHERE u.UId = @userId; 
                                    SELECT @price = p.PPrice FROM Product p WHERE p.Pid = @productId;
                                    SELECT @price AS Price, @cartid AS CartId;"
                        ;

                        var result = connection.QueryFirstOrDefault(query, new { productId = productId, userId = userId }, transaction);
                        if(result.CartId != 0 && result.Price != 0)
                        {
                            cartId = result.CartId;
                            price = result.Price;
                        }

                        var removeProductQuery = @"UPDATE ItemQuantity SET Quantity = CASE 
                                                WHEN Quantity-1 < 0 THEN 0 
                                                ELSE Quantity - 1 
                                                END WHERE PId = @productId AND CId = @cartId;
                      
                                                UPDATE Cart SET TotalCost = CASE 
                                                WHEN TotalCost - @price < 0 THEN 0 
                                                ELSE TotalCost - @price 
                                                END WHERE CId = @cartId; ";
                        connection.Execute(removeProductQuery, new { productId = productId, cartId = cartId, price = price}, transaction);
                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error: {ex.Message}");
                        return 0;
                    }
                }
            }
        }
    }
}
