using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task1.DTO;

using task1.Models;

namespace task1.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly CustomerorderContext cs;
        public ProductController(CustomerorderContext cs)
        {
            this.cs = cs;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await cs.Products
                    .Select(p => new ProductDTO
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        CategoryId = p.CategoryId,
                        Price = p.Price ?? 0,
                        Unit = p.Unit,
                        CreatedDate = p.CreatedDate ?? DateTime.MinValue
                    })
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error occurred", ex.Message);
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            var product = await cs.Products
                .Where(p => p.ProductId == id)
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryId = p.CategoryId,
                    Price = p.Price ?? 0,
                    Unit = p.Unit,
                    CreatedDate = p.CreatedDate ?? DateTime.MinValue
                }).FirstOrDefaultAsync();

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO dto)
        {
            try
            {
                var product = new Product
                {
                    ProductName = dto.ProductName,
                    CategoryId = dto.CategoryId,
                    Price = dto.Price,
                    Unit = dto.Unit,
                    CreatedDate = DateTime.Now
                };

                cs.Products.Add(product);
                await cs.SaveChangesAsync();
                return Ok("Successfully inserted");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error occurred", ex.Message);
                return StatusCode(500, "An error occurred while inserting product.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDTO dto)
        {
            var product = await cs.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found");

            product.ProductName = dto.ProductName;
            product.CategoryId = dto.CategoryId;
            product.Price = dto.Price;
            product.Unit = dto.Unit;

            try
            {
                await cs.SaveChangesAsync();
                return Ok("Successfully updated");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error occurred", ex.Message);
                return StatusCode(500, "An error occurred while updating product.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await cs.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found");

            try
            {
                cs.Products.Remove(product);
                await cs.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error occurred", ex.Message);
                return StatusCode(500, "An error occurred while deleting product.");
            }
        }
    }
}