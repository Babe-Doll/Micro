using Microsoft.EntityFrameworkCore;

namespace Micro.API.DBContext
{
    public class MicroDBContext : DbContext
    {
        public DbSet<User> Users { get; set; } 

        public MicroDBContext(DbContextOptions<MicroDBContext> options):base(options)
        {
           
        } 
 
    }

    public class User
    { 
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
