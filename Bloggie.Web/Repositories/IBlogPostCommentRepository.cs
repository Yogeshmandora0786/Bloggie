using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync (BlogPostComment comment); 
        Task<IEnumerable<BlogPostComment>>  GetCommentsByBlogIdAsync (Guid blogPostId); 
    }
}
