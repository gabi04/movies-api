using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using UserApi.Models;

namespace MoviesApi.Data
{
    public class Context: IdentityDbContext<AppUser>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        //public DbSet<User> Users { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}
 
