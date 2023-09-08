using Bloggie.Web.Models;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bloggie.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostInterface blogPostInterface;
        private readonly ITagInterface tagInterface;

        public HomeController(ILogger<HomeController> logger,IBlogPostInterface blogPostInterface,ITagInterface tagInterface)
        {
            this._logger = logger;
            this.blogPostInterface = blogPostInterface;
            this.tagInterface = tagInterface;
        }

        public async Task <IActionResult> Index()
        {
            //Getting All Blogs
            var blogpost = await blogPostInterface.GetAllAsync();

            //Get All Tags
            var tags = await tagInterface.GetAllAsync();

            var model = new HomeViewModel
            {
                blogPosts = blogpost,
                tags = tags,
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}