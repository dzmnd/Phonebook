using Business.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Business.Context
{
    public class PhonebookContext : DbContext
    {
        public PhonebookContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
