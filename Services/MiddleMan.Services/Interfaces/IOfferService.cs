namespace MiddleMan.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using MiddleMan.Data.Models;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.InputModels;
    using MiddleMan.Web.ViewModels.InputModels.Offer;

    public interface IOfferService
    {
        Task<Offer> CreateOfferAsync(CreateOfferModel inputModel);

        Task<Offer> GetOfferByIdAsync(string id);

        Task<Offer> GetOfferAnywayAsync(string id);

        Task<List<Offer>> GetAllNotAprovedOffersAsync();

        Task<List<Offer>> GetAllAprovedOffersAsync();

        Task ApproveOfferAsync(string id);

        Task RemoveOfferAsync(string id);

        Task<List<Offer>> GetAllDeletedOffersAsync();

        Task<List<Offer>> GetLatestOffers(int n);

        Task FeatureItemAsync(string id);

        Task<List<Offer>> GetFeaturedOffersAsync();

        Task RemoveFeatureOnItemAsync(string id);

        Task<bool> IsOfferRatedAsync(string id, string userId);

        Task<int?> GetRateForOffer(string id, string userId);

        Task<List<Offer>> GetAllUserOffersAsync(string userId);

        Task<double> GetOfferRatingAsync(string id);

        string StartsStringRating(double offerRating);

        Task<bool> IsUserCreatorOfOfferAsync(string userId, string id);

        Task<List<UserFavorite>> GetAllFavoriteUserOffersKeysAsync(string userId);

        Task<string> GetOfferNameById(string offerId);

        Task<List<Offer>> GetOffersBySearchAsync(string searchWord);

        Task<Offer> EditOfferAsync(EditOfferModel offerInput, string id);

        Task<Offer> ActivateOfferAsync(string id);

        Task<List<Offer>> GetAllActiveUserOffersAsync(string userId);

        Task<List<Offer>> GetAllDeactivatedUserOffersAsync(string userId);

        Task<List<Offer>> GetAllCategoryOffersAsync(string id);

        Task<Offer> ActivateOfferAsUserAsync(string id);

        Task<Offer> DeleteOfferAsUserAsync(string id);

        Task<bool> IsOfferExisting(string offerId);
    }
}
