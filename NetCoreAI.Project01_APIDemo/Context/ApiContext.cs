using Microsoft.EntityFrameworkCore;
using NetCoreAI.Project01_APIDemo.Entities;

namespace NetCoreAI.Project01_APIDemo.Context
{
	public class ApiContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=DESKTOP-3OD251U\\SQLEXPRESS;initial catalog=ApiAIDb;integrated security=true;trust server certificate=true");
		}

		public DbSet<Customer> Customers { get; set; }
	}
}
