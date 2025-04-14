using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project01_APIDemo.Context;
using NetCoreAI.Project01_APIDemo.Entities;

namespace NetCoreAI.Project01_APIDemo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		private readonly ApiContext _context;

		public CustomersController(ApiContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult CustomerList()
		{
			var value = _context.Customers.ToList();
			return Ok(value);
		}

		[HttpPost]
		public IActionResult CreateCustomer(Customer customer)
		{
			_context.Customers.Add(customer);
			_context.SaveChanges();
			return Ok("Müşteri ekleme işlemi başarılı");
		}

		[HttpDelete]
		public IActionResult DeleteCustomer(int id)
		{
			var value = _context.Customers.Find(id);
			_context.Customers.Remove(value);
			_context.SaveChanges();
			return Ok("Müşteri silindi");
		}

		[HttpGet("GetCustomer")]
		public IActionResult GetCustomer(int id)
		{
			var value = _context.Customers.Find(id);
			return Ok(value);
		}

		[HttpPut]
		public IActionResult UpdateCustomer(Customer customer)
		{
			_context.Customers.Update(customer);
			_context.SaveChanges();
			return Ok("Müşteri başarıyla güncellendi");
		}
	}
}
