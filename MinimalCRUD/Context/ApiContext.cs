using Microsoft.EntityFrameworkCore;
using MinimalCRUD.Models;

namespace MinimalCRUD.Context
{
    public class ApiContext : DbContext
    {
        public DbSet<Book> BookEntity { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity => entity.Property(p => p.ISBN).IsRequired());
        }
    }
}
