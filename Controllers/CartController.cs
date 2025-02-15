using Microsoft.AspNetCore.Mvc;
using ProjectJWTeCommerce.Repositories;

namespace ProjectJWTeCommerce.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet]
        [Route("add-to-cart/{userId}/{productId}/{addressId}")]
        public IActionResult AddToCart(int userId, int productId, int addressId)
        {
            if (userId == 0 || productId == 0 || addressId == 0)
            {
                return BadRequest();
            }

            var result = _cartRepository.AddToCart(userId, productId, addressId);
            if(result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);
        }

        [HttpDelete]
        [Route("remove-from-cart/{userId}/{productId}")]
        public IActionResult RemoveFromCart(int userId, int productId)
        {
            if(userId == 0 || productId == 0)
                return BadRequest();

            var val = _cartRepository.RemoveFromCart(userId, productId);
            if (val == 0)
                return BadRequest("Something went wrong with query");

            return Ok();
        }
    }
}
