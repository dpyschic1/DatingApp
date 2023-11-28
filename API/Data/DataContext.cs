using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        //Constructor for passing in the connection string
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        //Tables
        public DbSet<AppUser> Users { get; set; }
    }
}