using Microsoft.AspNetCore.Identity;

namespace MoviesApi.Models
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
    }
}
