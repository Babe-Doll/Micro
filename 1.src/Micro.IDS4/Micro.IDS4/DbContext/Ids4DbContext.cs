using Micro.IDS4.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Micro.IDS4.DbContext
{
    public class Ids4DbContext : IdentityDbContext<SysUser,SysRole,string>
    {
        public Ids4DbContext(DbContextOptions<Ids4DbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

