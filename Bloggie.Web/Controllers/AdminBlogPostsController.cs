using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagInterface tagInterface;
        private readonly IBlogPostInterface blogPostInterface;

        public AdminBlogPostsController(ITagInterface tagInterface, IBlogPostInterface blogPostInterface)
        {
            this.tagInterface = tagInterface;
            this.blogPostInterface = blogPostInterface;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await tagInterface.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            //getget tag from Repository
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //Map The View Model To Domail Model 

            var blogPostDomain = new BlogPost
            {
                Headding = addBlogPostRequest.Headding,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visiable = addBlogPostRequest.Visiable,
            };

            //Map Tags From Selected Tags 
            var SelectedTags = new List<Tag>();
            foreach (var SelectedTagId in addBlogPostRequest.SelectedTags)
            {
                var SelectedTagIdAsGuid = Guid.Parse(SelectedTagId);
                var existingTag = await tagInterface.GetAsync(SelectedTagIdAsGuid);

                if (existingTag != null)
                {
                    SelectedTags.Add(existingTag);
                }
            }
            //Mapping Tags back to Domain Model
            blogPostDomain.Tags = SelectedTags;
            await blogPostInterface.AddAsync(blogPostDomain);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            //Call To Repository

            var blogspost = await blogPostInterface.GetAllAsync();

            return View(blogspost);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //Reterive the Result from the repository
            var blogpost = await blogPostInterface.GetAsync(id);
            var tagsDomainModel = await tagInterface.GetAllAsync();

            if (blogpost != null)
            {
                //map the domain model into the view model 
                var model = new EditBlogPostRequest
                {
                    Id = blogpost.Id,
                    Headding = blogpost.Headding,
                    PageTitle = blogpost.PageTitle,
                    Content = blogpost.Content,
                    Author = blogpost.Author,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl,
                    UrlHandle = blogpost.UrlHandle,
                    ShortDescription = blogpost.ShortDescription,
                    PublishedDate = blogpost.PublishedDate,
                    Visiable = blogpost.Visiable,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogpost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);
            }
            //Pass Data To View 
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // Map To View Model Back to Domain Model 

            var blogpostdomainmodel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Headding = editBlogPostRequest.Headding,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visiable = editBlogPostRequest.Visiable,
            };

            //Map Tags Into Domain Model 

            var selectedTags = new List<Tag>();

            foreach (var SelectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(SelectedTag, out var tag))
                {
                    var foundtag = await tagInterface.GetAsync(tag);

                    if (foundtag != null)
                    {
                        selectedTags.Add(foundtag);
                    }
                }

            }

            blogpostdomainmodel.Tags = selectedTags;

            // Submit Information to repository to update
            var UpdatedBlog = await blogPostInterface.UpdateAsync(blogpostdomainmodel);

            if (UpdatedBlog != null)
            {
                //Show Success nofification
                return RedirectToAction("List");
            }

            //Show Error nofification
            return RedirectToAction("Edit");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //Talk to Repository to Delete This blog Post and Tags  

            var deletedblogpost = await blogPostInterface.DeleteAsync(editBlogPostRequest.Id);

            if (deletedblogpost != null)
            {
                //show success message

                return RedirectToAction("List");
            }

            //show Error message

            return RedirectToAction("Edit",new { id = editBlogPostRequest.Id});

            //Display The responce
        }
    }
}
