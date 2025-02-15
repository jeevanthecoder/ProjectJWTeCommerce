using ProjectJWTeCommerce.Models.UserAPIs;

namespace ProjectJWTeCommerce.Utility
{
    public class LoginUtility
    {
        public string Token { get; set; }
        public UserDetails user { get; set; }

        public LoginUtility()
        {
            Token = null;
            this.user = null;
        }

    }
}
