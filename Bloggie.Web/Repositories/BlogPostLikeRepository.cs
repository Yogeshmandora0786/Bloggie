using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeInterface
    {
        private readonly BlogieeDbContext blogieeDbContext;

        public BlogPostLikeRepository(BlogieeDbContext blogieeDbContext)
        {
            this.blogieeDbContext = blogieeDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await blogieeDbContext.BlogPostLike.AddAsync(blogPostLike);
            await blogieeDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<int> GetTotalLikes(Guid blogpostId)
        {
           return await blogieeDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogpostId);
        }
    }
}
