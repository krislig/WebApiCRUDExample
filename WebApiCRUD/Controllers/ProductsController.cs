using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        // GET: api/products
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_productRepository.GetAll());
        }

        // GET: api/products/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            _productRepository.Add(product);

            return CreatedAtRoute("products", new { id = product.Id }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            _productRepository.Update(id, product);

            return Ok();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productRepository.DeleteById(id);

            return Ok();
        }
    }
}
