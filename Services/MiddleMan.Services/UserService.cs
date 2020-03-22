namespace MiddleMan.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Interfaces;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<string> GetUsernameByIdAsync(string creatorId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == creatorId);

            return user.UserName;
        }

        public async Task<string> GetUserProfilePictureUrlAsync(string id)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user.ProfilePhotoUrl;
        }

        public async Task<double> GetUserRatingAsync(string id)
        {
            var offers = await this.context.OfferUserRates.Where(x => x.Offer.CreatorId == id).ToListAsync();

            if (offers.Count == 0)
            {
                return 0;
            }

            return Math.Round(offers.Average(x => (double)x.Rate));
        }

        public async Task UpdateProfilePictureUrl(string userId, string photoUrl)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            user.ProfilePhotoUrl = photoUrl;

            await this.context.SaveChangesAsync();
        }
    }
}
