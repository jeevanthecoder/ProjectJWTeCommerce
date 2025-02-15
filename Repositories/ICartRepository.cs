using ProjectJWTeCommerce.Models.CartAPIs;
using ProjectJWTeCommerce.Models.ProductAPIs;

namespace ProjectJWTeCommerce.Repositories
{
    public interface ICartRepository
    {
        Cart AddToCart(int userId, int productId, int addressId);
        int RemoveFromCart(int userId, int productId);
        Cart GetCart(int userId);
        List<ItemQuantity> GetItems(int userId);
    }
}
