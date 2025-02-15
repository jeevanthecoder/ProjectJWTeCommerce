using ProjectJWTeCommerce.Models.ProductAPIs;
using ProjectJWTeCommerce.Models.SellerAPIs;

namespace ProjectJWTeCommerce.Repositories
{
    public interface IProductRepository
    {
        List<Product> AddProduct(int sellerId,Product product);
        Product UpdateProduct(int sellerId,int productId,Product product);
        void DeleteProduct(int sellerId,int productId);
        List<Product> GetProductsOfSeller(int sellerId);
        Product GetProduct(int productId);
        List<Product> GetAllProducts();
    }
}
