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
        public IActionResult RegisterUser(UserDetails user) 
        {
            if (user == null)
            {
                return BadRequest();
            }

            var result = _userRepository.RegisterService(user);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpPost]
        [Route("login-user")]
        public IActionResult LoginUser(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest();
            }

            var result = _userRepository.LoginService(loginDTO);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpPut]
        [Route("update-user/{userId}")]
        public IActionResult UpdateUser(int userId,UpdateUserDTO updateUserDTO)
        {
            if(updateUserDTO == null || userId == 0)
            {
                return BadRequest();
            }

            var result = _userRepository.UpdateService(userId, updateUserDTO);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpGet]
        [Route("convert-To-Seller/{userId}")]
        public IActionResult ConvertToSeller(int userId)
        {
            if(userId == 0)
            {
                return BadRequest();
            }

            var result = _userRepository.ConversionService(userId);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpPost]
        [Route("add-address/{userId}")]
        public IActionResult AddAddress(int userId, Address address)
        {
            if(userId==0 || address==null)
                { return BadRequest(); }

            var result = _userRepository.AddAddress(userId, address);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpPut]
        [Route("update-address/{AId}")]
        public IActionResult UpdateResult(int AId, Address address)
        {
            if(address==null || AId==0)
                { return BadRequest(); }

            var result = _userRepository.UpdateAddress(AId, address);
            if(result==null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpGet]
        [Route("get-user/{UId}")]
        public IActionResult GetUser(int UId)
        {
            if(UId==0)
                { return BadRequest(); }

            var result = _userRepository.GetUserById(UId);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpGet]
        [Route("get-addresses/{UId}")]
        public IActionResult GetAddresses(int UId)
        {
            if (UId == 0)
            {
                return BadRequest();
            }

            var result = _userRepository.GetAddresses(UId);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }
    }
}
