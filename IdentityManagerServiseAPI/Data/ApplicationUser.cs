using Microsoft.AspNetCore.Identity;

namespace IdentityManagerServiseAPI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
