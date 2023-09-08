using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class BlogieeDbContext : DbContext
    {
        public BlogieeDbContext(DbContextOptions<BlogieeDbContext> options) : base(options)
        {

        }

        public DbSet<BlogPost> MyBlogPosts { get; set; }
        public DbSet<Tag> MyTags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
        public DbSet<BlogPostComment> BlogPostComment { get; set; }
    }
}
