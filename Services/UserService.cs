using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProjectJWTeCommerce.Models.DTOs;
using ProjectJWTeCommerce.Models.SellerAPIs;
using ProjectJWTeCommerce.Models.UserAPIs;
using ProjectJWTeCommerce.Repositories;
using ProjectJWTeCommerce.Utility;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Diagnostics;

namespace ProjectJWTeCommerce.Services
{
    public class UserService : IUserRepository
    {

        private readonly MyDBContext _dbContext;
        private readonly IConfiguration configuration;

        public UserService(MyDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            this.configuration = configuration;
        }

        public async Task<IEnumerable<Address>> AddAddress(int id, Address address)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var query = @"INSERT INTO Address (userId,FullName,Phone,City,Country, Pincode,Landmark, HouseDetails, StreetDetails) 
                VALUES(@userId,@name, @phone, @city, @country, @pincode, @landmark, @house, @street);
                SELECT * FROM Address WHERE userId = @userId ;";
                try
                {
                    var addressValues = await connection.QueryAsync<Address>(query, new
                    {
                        userId = id,
                        name = address.FullName,
                        phone = address.Phone,
                        city = address.City,
                        country = address.Country,
                        pincode = address.Pincode,
                        landmark = address.Landmark,
                        house = address.HouseDetails,
                        street = address.StreetDetails

                    });
                    return addressValues;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<Seller> ConversionService(int userid)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var getuser = @"SELECT * FROM UserDetails WHERE UId = @id;";
                UserDetails user = await connection.QueryFirstAsync<UserDetails>(getuser, new { id = userid });
                var query = @"INSERT INTO sellers (SellerName,SellerPhone,SellerEmail,NoOfProducts,userId) VALUES (@name, @phone, @email, @num, @userId);
                UPDATE UserDetails SET sellerId = SCOPE_IDENTITY() WHERE UId = @userId ;
                SELECT * FROM sellers WHERE SId = SCOPE_IDENTITY();";

                try
                {
                    var seller = connection.QuerySingleAsync<Seller>(query, new
                    {
                        name = user.UserName,
                        phone = user.UserPhone,
                        email = user.UserEmail,
                        num = 0,
                        userId = user.UId

                    });
                    return await seller;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task DeleteAddress(int addressId)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var query = "DELETE FROM Address WHERE AId=@addressId;";
                try
                {
                    await connection.QueryAsync<Address>(query, new { addressId = addressId });
                }catch(Exception e)
                {

                }
            }
        }

        public async Task<Address> GetAddressById(int id)
        {
            using(var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Address WHERE AId=@aid;";
                try
                {
                    var addressValue = await connection.QuerySingleAsync<Address>(query, new { aid = id });
                    return addressValue;

                }catch(Exception e)
                {
                    return null;
                }
            }
        }

        public async Task<IEnumerable<Address>> GetAddresses(int userId)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Address WHERE userId=@userId;";
                try
                {
                    var addresses = await connection.QueryAsync<Address>(query, new
                    {
                        userId = userId
                    });
                    return addresses;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<Seller> GetSellerById(int id)
        {
            using(var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var query = "SELECT * FROM sellers WHERE SId = @sId";
                try
                {
                    var sellerValue = await connection.QuerySingleAsync<Seller>(query, new
                    {
                        sId = id
                    });
                    return sellerValue;
                }catch(Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<UserDetails> GetUserById(int id)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var query = "SELECT * FROM UserDetails WHERE UId = @id;";
                try
                {
                    var userValues = connection.QuerySingleAsync<UserDetails>(query, new { id = id });

                    return await userValues;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<LoginUtility> LoginService(LoginDTO loginDTO)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var query = "SELECT * FROM UserDetails WHERE UserEmail = @email;";
                try
                {
                    var user = await connection.QueryFirstOrDefaultAsync<UserDetails>(query, new { email = loginDTO.email });

                    var decodedPassword = EncodeDecode.Decrypt(user.UserPassword).ToString();
                    //Response.Write("User : "+user);

                    if (user != null && decodedPassword == loginDTO.password)
                    {
                        var claims = new[]
                        {
        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("UserName", user.UserName.ToString()),
        new Claim("UserEmail",user.UserEmail.ToString()),

    };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                        var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            configuration["Jwt:Issuer"],
                            configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(60),
                            signingCredentials: signin
                            );

                        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                        LoginUtility values = new LoginUtility();
                        values.Token = tokenValue;
                        values.user = user;
                        return values;

                    }
                    else
                    {
                        return new LoginUtility();
                    }
                }
                catch (Exception e) {
                    return null;
                }
            }
        }



        public async Task<UserDetails> RegisterService(UserDetails user)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var query = @"
        INSERT INTO [UserDetails] (UserName, UserPassword, UserEmail, UserPhone, FirstName, LastName, sellerId)
        VALUES (@name, @password, @email, @phone, @firstName, @lastName, @sellerId);
        SELECT * FROM UserDetails WHERE UId = SCOPE_IDENTITY();";  // Ensure it returns the new User ID

                try
                {
                    var userDetails = await connection.QuerySingleAsync<UserDetails>(query, new
                    {
                        name = user.UserName,
                        password = EncodeDecode.Encrypt(user.UserPassword),
                        email = user.UserEmail,
                        phone = user.UserPhone,
                        firstName = user.FirstName,
                        lastName = user.LastName,

                        sellerId = 0
                    });
                    if (userDetails == null)
                    {
                        throw new Exception("Insert failed, no ID returned.");
                    }

                    return userDetails;
                }
                catch (Exception ex)
                {
                    //throw ex;
                    return null;
                }
                connection.Close();
            }
        }

        public async Task<Address> UpdateAddress(int AId, Address address)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var query = @"UPDATE Address SET FullName=@name,Phone=@phone,City=@city,Country=@country, 
                    Pincode=@pincode,Landmark=@landmark, HouseDetails = @house, StreetDetails = @street 
                    WHERE AId=@aid;
                    SELECT * FROM Address WHERE AId = @aid;";
                try
                {
                    var addressValue = await connection.QuerySingleAsync<Address>(query, new
                    {
                        name = address.FullName,
                        phone = address.Phone,
                        city = address.City,
                        country = address.Country,
                        pincode = address.Pincode,
                        landmark = address.Landmark,
                        house = address.HouseDetails,
                        street = address.StreetDetails,
                        aid = AId
                    });

                    return addressValue;
                }
                catch (Exception ex)
                {
                    return null;
                }
                connection.Close();
            }
        }

        public async Task<IEnumerable<UserDetails>> UpdateService(int id, UpdateUserDTO updateUserDTO)
        {
            using (var connection = new SqlConnection(Database.ConnectionString))
            {
                connection.Open();
                var query = @"
        UPDATE [UserDetails] SET UserName=@name, UserEmail=@email, UserPhone=@phone, FirstName=@firstName, LastName=@lastName WHERE UId=@id;
        SELECT * FROM UserDetails WHERE UId = @id;";  // Ensure it returns the new User ID

                try
                {
                    var userDetails = await connection.QueryAsync<UserDetails>(query, new
                    {
                        id = id,
                        name = updateUserDTO.UserName,
                        email = updateUserDTO.UserEmail,
                        phone = updateUserDTO.UserPhone,
                        firstName = updateUserDTO.FirstName,
                        lastName = updateUserDTO.LastName
                    });
                    if (userDetails == null)
                    {
                        throw new Exception("Insert failed, no ID returned.");
                    }

                    return userDetails;

                }
                catch (Exception ex)
                {
                    return null;
                }
                connection.Close();
            }
        }
    }
}
