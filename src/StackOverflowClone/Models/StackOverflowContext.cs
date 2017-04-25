using Microsoft.EntityFrameworkCore;
using StackOverflowClone.Models;

namespace StackOverflowClone
{
    internal class StackOverflowContext : DbContext
    {
        public virtual DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=StackOverflow;integrated security=True");
        }

        public StackOverflowContext(DbContextOptions<StackOverflowContext> options)
            : base(options)
        {
        }

        public StackOverflowContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}