using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task1.DTO;
using task1.DTO;
using task1.Models;

namespace task1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly CustomerorderContext _context;

        public OrderController(CustomerorderContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .Select(o => new OrderDTO
                    {
                        OrderId = o.OrderId,
                        CustomerId = o.CustomerId,
                        CustomerName = o.Customer.FullName,
                        OrderDate = o.OrderDate ?? DateTime.MinValue,
                        OrderItems = o.OrderDetails.Select(od => new OrderItemDTO
                        {
                            ProductId = od.ProductId,
                            ProductName = od.Product.ProductName,
                            Quantity = od.Quantity ?? 0,
                            UnitPrice = od.Price ?? 0
                        }).ToList()
                    }).ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.OrderId == id)
                .Select(o => new OrderDTO
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.FullName,
                    OrderDate = o.OrderDate ?? DateTime.MinValue,
                    OrderItems = o.OrderDetails.Select(od => new OrderItemDTO
                    {
                        ProductId = od.ProductId,
                        ProductName = od.Product.ProductName,
                        Quantity = od.Quantity ?? 0,
                        UnitPrice = od.Price ?? 0
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (order == null)
                return NotFound("Order not found");

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDTO dto)
        {
            try
            {
                var order = new Order
                {
                    CustomerId = dto.CustomerId,
                    OrderDate = DateTime.Now,
                    OrderDetails = dto.OrderItems.Select(i => new OrderDetail
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.UnitPrice
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return Ok("Order created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound("Order not found");

            try
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return Ok("Order deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                return StatusCode(500, "An error occurred while deleting the order.");
            }
        }
    }
}
