using Microsoft.AspNetCore.Mvc;
using ProjectJWTeCommerce.Models.ProductAPIs;
using ProjectJWTeCommerce.Repositories;

namespace ProjectJWTeCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        [Route("add-product/{sellerId}")]
        public IActionResult AddProduct(int sellerId, Product product)
        {
            if (sellerId == 0 || product == null)
            {
                return BadRequest();
            }
            var result = _productRepository.AddProduct(sellerId, product);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);

        }

        [HttpPut]
        [Route("update-product/{sellerId}/{productId}")]
        public IActionResult UpdateProduct(int sellerId, int productId, Product product)
        {
            if (sellerId == 0 || productId == 0 || product == null)
                return BadRequest();

            var result = _productRepository.UpdateProduct(sellerId,productId, product);
            if (result == null)
                return BadRequest("Something went wrong with the query");
            return Ok(result);

        }

        [HttpDelete]
        [Route("delete-product/{sellerId}/{productId}")]
        public IActionResult DeleteProduct(int sellerId, int productId)
        {
            if(sellerId == 0 || productId == 0)
                return BadRequest();

            _productRepository.DeleteProduct(sellerId,productId);
            return Ok();
        }
    }
}
