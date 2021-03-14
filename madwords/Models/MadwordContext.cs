using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace madwords.Models
{
    public class MadwordContext : IdentityDbContext<AppUser>
    {
        public MadwordContext(DbContextOptions<MadwordContext> options) : base(options) { }
        public DbSet<Madword> Madwords { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        //public DbSet<User> User { get; set; }
        /// <summary>
        /// BlogPosts
        /// </summary>
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogPostComment> BlogPostComments { get; set; }
        public DbSet<MadwordTemplate> MadwordTemplates { get; set; }
    }
}
