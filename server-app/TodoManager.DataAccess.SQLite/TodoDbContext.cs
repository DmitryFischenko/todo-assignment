using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoManager.DataAccess.SQLite.Entities;

namespace TodoManager.DataAccess.SQLite
{
    internal sealed class TodoDbContext: DbContext 
    {
        private readonly IConfiguration _configuration;

        public TodoDbContext(IConfiguration configuration)
        {
            _configuration = configuration;

            Database.EnsureCreated();
        }
        
        public DbSet<TodoItem> TodoItems { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_configuration["SQLite:Connection"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().ToTable("TodoItems");
            modelBuilder.Entity<TodoItem>().HasIndex(i => i.Title).IsUnique();
        }
    }
}