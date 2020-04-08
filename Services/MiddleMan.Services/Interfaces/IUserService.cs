namespace MiddleMan.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MiddleMan.Data.Models;

    public interface IUserService
    {
        Task<string> GetUsernameByIdAsync(string creatorId);

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<string> GetUserProfilePictureUrlAsync(string id);

        Task UpdateProfilePictureUrl(string userId, string photoUrl);

        Task<double> GetUserRatingAsync(string id);

        Task UpdateUserFirstAndLastNameAsync(string id, string firstName, string lastName);

        Task<string> GetUserFirstNameAsync(string id);

        Task<string> GetUserLastNameAsync(string id);

        Task<List<string>> GetUserRolesAsync(string id);

        Task<bool> IsOfferFavoritedByUserAsync(string id, string userId);

        Task<int> GetAdminOffersForApprove();

        string GetCurrentUserId();
    }
}
