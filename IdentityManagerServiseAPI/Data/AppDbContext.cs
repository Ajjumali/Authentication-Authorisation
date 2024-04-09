using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityManagerServiseAPI.Data
{
    public class AppDbContext(DbContextOptions option) : IdentityDbContext<ApplicationUser>(option)
    {
    }
}
