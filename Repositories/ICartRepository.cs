using ProjectJWTeCommerce.Models.CartAPIs;
using ProjectJWTeCommerce.Models.ProductAPIs;

namespace ProjectJWTeCommerce.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> AddToCart(int userId, int productId, int addressId);
        Task<int> RemoveFromCart(int userId, int productId);
        Task<Cart> GetCart(int userId);
        Task<IEnumerable<ItemQuantity>> GetItems(int userId);
    }
}
