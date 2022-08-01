using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PoseidonAPI.Data
{
    public class PoseidonDBContext : IdentityDbContext<IdentityUser>
    {
        public PoseidonDBContext(DbContextOptions<PoseidonDBContext> options) : base(options) { }
    }
}
