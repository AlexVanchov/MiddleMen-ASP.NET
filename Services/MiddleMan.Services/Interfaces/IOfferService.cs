namespace MiddleMan.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using MiddleMan.Data.Models;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.InputModels;

    public interface IOfferService
    {
        Task CreateOfferAsync(CreateOfferModel inputModel);

        Task<Offer> GetOfferByIdAsync(string id);

        Task<List<Offer>> GetAllNotAprovedOffersAsync();

        Task<List<Offer>> GetAllAprovedOffersAsync();

        Task ApproveOfferAsync(string id);

        Task RemoveOffer(string id);

        Task<List<Offer>> GetAllDeletedOffersAsync();

        Task<List<Offer>> GetLatestOffers(int n);

        Task FeatureItem(string id);

        Task<List<Offer>> GetFeaturedOffers();

        Task RemoveFeatureOnItem(string id);

        Task<bool> IsOfferRatedAsync(string id, string userId);

        Task<int?> GetRateForOffer(string id, string userId);

        Task<List<Offer>> GetAllUserOffersAsync(string userId);

        Task<double> GetOfferRatingAsync(string id);

        string StartsStringRating(double offerRating);
    }
}
