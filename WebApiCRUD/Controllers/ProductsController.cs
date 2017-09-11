using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApiCRUD.Models;

namespace WebApiCRUD.Controllers
{
    [Produces("application/json")]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var results = await _productRepository.GetAllAsync();

            return Ok(results);
        }

        // GET: api/products/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            await _productRepository.AddAsync(product);

            return Ok();
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            await _productRepository.UpdateAsync(id, product);

            return Ok();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _productRepository.DeleteByIdAsync(id);

            return Ok();
        }
    }
}
