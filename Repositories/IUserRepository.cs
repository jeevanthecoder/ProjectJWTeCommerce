using ProjectJWTeCommerce.Models.DTOs;
using ProjectJWTeCommerce.Models.SellerAPIs;
using ProjectJWTeCommerce.Models.UserAPIs;
using ProjectJWTeCommerce.Utility;

namespace ProjectJWTeCommerce.Repositories
{
    public interface IUserRepository
    {
        Task<LoginUtility> LoginService (LoginDTO loginDTO);
        Task<UserDetails> RegisterService(UserDetails user);
        Task<IEnumerable<UserDetails>> UpdateService(int id,UpdateUserDTO updateUserDTO);
        Task<Seller> ConversionService(int id);
        Task<UserDetails> GetUserById(int id);
        Task<IEnumerable<Address>> AddAddress(int userId,Address address);
        Task<Address> UpdateAddress(int Aid,Address address);
        Task<IEnumerable<Address>> GetAddresses(int userId);
    }
}
