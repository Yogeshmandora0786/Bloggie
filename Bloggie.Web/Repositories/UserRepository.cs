using Bloggie.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;

        public UserRepository(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }


        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var User = await authDbContext.Users.ToListAsync(); 

            var superAdminUser = await authDbContext.Users.FirstOrDefaultAsync(x => x.Email == "SuperAdmin@bloggie.com");

            if (superAdminUser is not null)
            {
                User.Remove(superAdminUser);
            }

            return User;
        }
    }
}
