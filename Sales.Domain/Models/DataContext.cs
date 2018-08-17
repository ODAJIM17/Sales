
namespace Sales.Domain.Models
{
    using Sales.Common.Models;
    using System.Data.Entity;
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }

        public DbSet<Product> Products { get; set; }

        
    }
}