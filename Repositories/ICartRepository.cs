using ProjectJWTeCommerce.Models.CartAPIs;

namespace ProjectJWTeCommerce.Repositories
{
    public interface ICartRepository
    {
        Cart AddToCart(int userId, int productId, int addressId);
        int RemoveFromCart(int userId, int productId);
    }
}
