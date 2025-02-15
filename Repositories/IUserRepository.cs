using ProjectJWTeCommerce.Models.DTOs;
using ProjectJWTeCommerce.Models.SellerAPIs;
using ProjectJWTeCommerce.Models.UserAPIs;
using ProjectJWTeCommerce.Utility;

namespace ProjectJWTeCommerce.Repositories
{
    public interface IUserRepository
    {
        LoginUtility LoginService (LoginDTO loginDTO);
        UserDetails RegisterService(UserDetails user);
        IEnumerable<UserDetails> UpdateService(int id,UpdateUserDTO updateUserDTO);
        Seller ConversionService(int id);
        UserDetails GetUserById(int id);
        List<Address> AddAddress(int userId,Address address);
        Address UpdateAddress(int Aid,Address address);
        List<Address> GetAddresses(int userId);
    }
}
