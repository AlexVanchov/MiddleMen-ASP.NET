namespace MiddleMan.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data;
    using MiddleMan.Services.Interfaces;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<string> GetUsernameByIdAsync(string creatorId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == creatorId);
            return user.UserName;
        }
    }
}
