 using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagInterface tagInterface;

        public AdminTagsController(ITagInterface tagInterface)
        {
            this.tagInterface = tagInterface;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest To Tag Domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagInterface.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            // use dbContext to read the Tags
            var tags = await tagInterface.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            //1st Method
            //var tag = blogieeDbContext.MyTags.Find(Id);

            //2nd Method
            //var tag = await blogieeDbContext.MyTags.FirstOrDefaultAsync(x => x.Id == Id);

            var tag = await tagInterface.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };
                return View(editTagRequest);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest edittagrequest)
        {
            var tag = new Tag
            {
                Id = edittagrequest.Id,
                Name = edittagrequest.Name,
                DisplayName = edittagrequest.DisplayName,
            };

            var updatetag = await tagInterface.UpdateAsync(tag);

            if (updatetag != null)
            {
                //Show Success Notification
            }
            else
            {
                //Show Error Notification

            }

            return RedirectToAction("Edit", new { id = edittagrequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest edittagrequest)
        {
            var deleted = await tagInterface.DeleteAsync(edittagrequest.Id);

            if (deleted != null)
            {
                //Show Success Notification
                return RedirectToAction("List", new { id = edittagrequest.Id });

            }

            else
            {
                //Show Error Notification

            }
            return RedirectToAction("Edit", new { id = edittagrequest.Id });
        }
    }
}
