using ProjectJWTeCommerce.Models.ProductAPIs;
using ProjectJWTeCommerce.Models.SellerAPIs;

namespace ProjectJWTeCommerce.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> AddProduct(int sellerId,Product product);
        Task<Product> UpdateProduct(int sellerId,int productId,Product product);
        Task DeleteProduct(int sellerId,int productId);
        Task<IEnumerable<Product>> GetProductsOfSeller(int sellerId);
        Task<Product> GetProduct(int productId);
        Task<IEnumerable<Product>> GetAllProducts(int pageNumber, int pageSize);
    }
}
