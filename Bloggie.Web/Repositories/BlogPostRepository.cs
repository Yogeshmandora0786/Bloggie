using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostInterface
    {
        private readonly BlogieeDbContext blogieeDbContext;

        public BlogPostRepository(BlogieeDbContext blogieeDbContext)
        {
            this.blogieeDbContext = blogieeDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await blogieeDbContext.AddAsync(blogPost);
            await blogieeDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingblog = await blogieeDbContext.MyBlogPosts.FindAsync(id);

            if(existingblog != null)
            {
                blogieeDbContext.MyBlogPosts.Remove(existingblog);

                await blogieeDbContext.SaveChangesAsync();

                return existingblog;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await blogieeDbContext.MyBlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await blogieeDbContext.MyBlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle) 
        {
            return await blogieeDbContext.MyBlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await blogieeDbContext.MyBlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if(existingBlog !=null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Headding = blogPost.Headding;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.Content;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visiable = blogPost.Visiable;
                existingBlog.PublishedDate = blogPost.PublishedDate;

                existingBlog.Tags = blogPost.Tags;

                await blogieeDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }
    }
}
