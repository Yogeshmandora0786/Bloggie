using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class TagRepository : ITagInterface
    {
        private readonly BlogieeDbContext blogieeDbContext;

        public TagRepository(BlogieeDbContext blogieeDbContext)
        {
            this.blogieeDbContext = blogieeDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await blogieeDbContext.MyTags.AddAsync(tag);
            await blogieeDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var exiting = await blogieeDbContext.MyTags.FindAsync(id);

            if(exiting != null)
            {
                blogieeDbContext.MyTags.Remove(exiting);
                await blogieeDbContext.SaveChangesAsync();
                return exiting;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await blogieeDbContext.MyTags.ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
            return blogieeDbContext.MyTags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await blogieeDbContext.MyTags.FindAsync(tag.Id);
            
            if(existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await blogieeDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }
    }
}
