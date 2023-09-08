using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostInterface blogPostInterface;
        private readonly IBlogPostLikeInterface blogPostLikeInterface;

        public BlogsController(IBlogPostInterface blogPostInterface, IBlogPostLikeInterface blogPostLikeInterface)
        {
            this.blogPostInterface = blogPostInterface;
            this.blogPostLikeInterface = blogPostLikeInterface;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogpost = await blogPostInterface.GetByUrlHandleAsync(urlHandle);

            var blogpostlikeviewmodel = new BlogDetailsViewModels();

            if (blogpost != null)
            {
                var totallikes = await blogPostLikeInterface.GetTotalLikes(blogpost.Id);

                blogpostlikeviewmodel = new BlogDetailsViewModels
                {
                    Id = blogpost.Id,
                    Content = blogpost.Content,
                    PageTitle = blogpost.PageTitle,
                    Author = blogpost.Author,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl,
                    Headding = blogpost.Headding,
                    PublishedDate = blogpost.PublishedDate,
                    ShortDescription = blogpost.ShortDescription,
                    UrlHandle = blogpost.UrlHandle,
                    Visiable = blogpost.Visiable,
                    Tags = blogpost.Tags,
                    TotalLikes = totallikes,
                };
            }

            return View(blogpostlikeviewmodel);
        }
    }
}
