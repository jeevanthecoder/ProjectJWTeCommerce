using Microsoft.AspNetCore.Mvc;
using ProjectJWTeCommerce.Models.DTOs;
using ProjectJWTeCommerce.Models.UserAPIs;
using ProjectJWTeCommerce.Repositories;

namespace ProjectJWTeCommerce.Controllers
{ 
        [ApiController]
        [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpPost]
        [Route("register-user")]
        public async Task<IActionResult> RegisterUser(UserDetails user) 
        {
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userRepository.RegisterService(user);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpPost]
        [Route("login-user")]
        public async Task<IActionResult> LoginUser(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest();
            }

            var result = await _userRepository.LoginService(loginDTO);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpDelete("delete-address/{addressId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            if(addressId == 0)
                return BadRequest();

            await _userRepository.DeleteAddress(addressId);
            return Ok();
        }

        [HttpGet]
        [Route("get-seller/{sellerId}")]
        public async Task<IActionResult> GetSeller(int sellerId)
        {
            if (sellerId == 0)
                return BadRequest();

            var result = await _userRepository.GetSellerById(sellerId);
            if(result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("get-address/{addressId}")]
        public async Task<IActionResult> GetAddress(int addressId)
        {
            if (addressId == 0)
                return BadRequest();

            var result = await _userRepository.GetAddressById(addressId);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPut]
        [Route("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId,UpdateUserDTO updateUserDTO)
        {
            if(updateUserDTO == null || userId == 0)
            {
                return BadRequest();
            }

            var result = await _userRepository.UpdateService(userId, updateUserDTO);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpGet]
        [Route("convert-To-Seller/{userId}")]
        public async Task<IActionResult> ConvertToSeller(int userId)
        {
            if(userId == 0)
            {
                return BadRequest();
            }

            var result = await _userRepository.ConversionService(userId);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpPost]
        [Route("add-address/{userId}")]
        public async Task<IActionResult> AddAddress(int userId, Address address)
        {
            if(userId==0 || address==null)
                { return BadRequest(); }

            var result = await _userRepository.AddAddress(userId, address);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpPut]
        [Route("update-address/{AId}")]
        public async Task<IActionResult> UpdateResult(int AId, Address address)
        {
            if(address==null || AId==0)
                { return BadRequest(); }

            var result = await _userRepository.UpdateAddress(AId, address);
            if(result==null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpGet]
        [Route("get-user/{UId}")]
        public async Task<IActionResult> GetUser(int UId)
        {
            if(UId==0)
                { return BadRequest(); }

            var result = await _userRepository.GetUserById(UId);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpGet]
        [Route("get-addresses/{UId}")]
        public async Task<IActionResult> GetAddresses(int UId)
        {
            if (UId == 0)
            {
                return BadRequest();
            }

            var result = await _userRepository.GetAddresses(UId);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }
    }
}
