using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<BlogPost> blogPosts { get; set;}
        public IEnumerable<Tag> tags { get; set;}
    }
}
