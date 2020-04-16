namespace MiddleMan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Interfaces;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor contextAccessor;

        public UserService(
            ApplicationDbContext context,
            IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.contextAccessor = contextAccessor;
        }

        public async Task<int> GetAdminOffersForApproveCount()
        {
            var offers = await this.context.Offers.Where(x => x.IsApproved == false &&
            x.IsDeclined == false &&
            x.IsRemovedByUser == false && x.IsDeleted == false)
                .ToListAsync();

            return offers.Count;
        }

        public string GetCurrentUserId()
        {
            var currentUserId = this.contextAccessor
                .HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value;

            return currentUserId;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<string> GetUserFirstNameAsync(string id)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            return user.FirstName;
        }

        public async Task<string> GetUserLastNameAsync(string id)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            return user.LastName;
        }

        public async Task<string> GetUsernameByIdAsync(string creatorId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == creatorId);

            if (user == null)
            {
                return null;
            }

            return user.UserName;
        }

        public async Task<string> GetUserProfilePictureUrlAsync(string id)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

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

        public async Task<List<string>> GetUserRolesAsync(string id)
        {
            if (await this.GetUserByIdAsync(id) == null)
            {
                return null;
            }

            var userRoles = await this.context.UserRoles.Where(x => x.UserId == id).ToListAsync();

            var roles = new List<string>();

            foreach (var userRole in userRoles)
            {
                roles.Add(await this.GetUserRoleByIdAsync(userRole.RoleId));
            }

            return roles;
        }

        public async Task<bool> IsOfferFavoritedByUserAsync(string id, string userId)
        {
            return await this.context.UserFavorites.AnyAsync(x => x.OfferId == id && x.UserId == userId);
        }

        public async Task<string> UpdateProfilePictureUrl(string userId, string photoUrl)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user != null)
            {
                user.ProfilePhotoUrl = photoUrl;

                await this.context.SaveChangesAsync();

                return photoUrl;
            }

            return null;
        }

        public async Task<ApplicationUser> UpdateUserFirstAndLastNameAsync(string id, string firstName, string lastName)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            user.FirstName = firstName;
            user.LastName = lastName;

            await this.context.SaveChangesAsync();

            return user;
        }

        private async Task<string> GetUserRoleByIdAsync(string roleId)
        {
            var role = await this.context.Roles.FirstOrDefaultAsync(x => x.Id == roleId);

            return role.Name;
        }
    }
}
