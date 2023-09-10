using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostInterface blogPostInterface;
        private readonly IBlogPostLikeInterface blogPostLikeInterface;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;

        public BlogsController(IBlogPostInterface blogPostInterface,
            IBlogPostLikeInterface blogPostLikeInterface,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository)
        {
            this.blogPostInterface = blogPostInterface;
            this.blogPostLikeInterface = blogPostLikeInterface;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogpost = await blogPostInterface.GetByUrlHandleAsync(urlHandle);

            var blogpostlikeviewmodel = new BlogDetailsViewModels();

            if (blogpost != null)
            {
                var totallikes = await blogPostLikeInterface.GetTotalLikes(blogpost.Id);


                //Get Comments For For Blog Post

                var BlogCommentsDomainModel = await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogpost.Id);

                var blogCommentsForView = new List<BlogComment>();


                foreach(var BlogComment in BlogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description  = BlogComment.Description,
                        DateAdded  = BlogComment.DateAdded,
                        UserName  = (await userManager.FindByIdAsync(BlogComment.UserId.ToString())).UserName
                    });
                }



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
                    Comments = blogCommentsForView
                };
            }

            return View(blogpostlikeviewmodel);
        }


        [HttpPost]

        public async Task<IActionResult> Index(BlogDetailsViewModels blogDetailsViewModels)
        {
            if (signInManager.IsSignedIn(User))
            {
                var DomainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModels.Id,
                    Description = blogDetailsViewModels.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };

                await blogPostCommentRepository.AddAsync(DomainModel);

                return RedirectToAction("Index" , "Blogs" , new { urlHandle = blogDetailsViewModels .UrlHandle});
            }

            return Forbid();
        }
    }
}
