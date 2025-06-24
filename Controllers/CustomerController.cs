using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task1.Models;

namespace task1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerorderContext cs;
        public CustomerController(CustomerorderContext cs) {
            this.cs = cs;
        }
        //retrive all
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var customers = await cs.Customers.ToListAsync();
                return Ok(customers);
            }
            catch(Exception ex)
            {
                Console.WriteLine("error occured", ex.Message);
                return StatusCode(500, "An error occurred while retrieving customers.");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("id is invalid");
                }
                var customer = await cs.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound("id not found");
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error occured", ex.Message);
                return StatusCode(500, "An error occurred while finding customer by id.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            try
            {
                cs.Customers.Add(customer);
                await cs.SaveChangesAsync();
                return Ok("successfully inserted");
            }
            catch(Exception ex)
            {
                Console.WriteLine("error occured", ex.Message);
                return StatusCode(500, "An error occurred while inserting customers.");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Customer customer)
        {
            var existingCustomer = await cs.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return NotFound("id is not found");
            }
            existingCustomer.FullName = customer.FullName;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Address = customer.Address;

            try
            {
                await cs.SaveChangesAsync();
                return Ok("successfully saved"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("error occured", ex.Message);
                return StatusCode(500, "An error occurred while updating customers.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var customer = await cs.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound("id is not found");
                }
                cs.Customers.Remove(customer);
                await cs.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                Console.WriteLine("error occured", ex.Message);
                return StatusCode(500, "An error occurred while deleting customers.");
            }
        }

    }
}
