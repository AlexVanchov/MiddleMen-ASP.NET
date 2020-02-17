namespace MiddleMan.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using MiddleMan.Data.Models;
    using MiddleMan.Web.ViewModels.InputModels;

    public interface IOfferService
    {
        Task CreateOfferAsync(CreateOfferModel inputModel);  // TODO model

        Task<Offer> GetOfferByIdAsync(string id);

        Task<List<Offer>> GetAllNotAprovedOffersAsync();

        Task<List<Offer>> GetAllAprovedOffersAsync();

        Task ApproveOfferAsync(string id);

        Task RemoveOffer(string id);
    }
}
