namespace MiddleMan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.InputModels;

    public class OfferService : IOfferService
    {

        private readonly ApplicationDbContext context;

        public OfferService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateOfferAsync(CreateOfferModel inputModel)
        {
            var offer = new Offer()
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                Price = inputModel.Price,
                PicUrl = inputModel.PicUrl,
                CategoryId = inputModel.CategotyName,
                CreatorId = inputModel.CreatorId,
            };

            await this.context.Offers.AddAsync(offer);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Offer>> GetAllNotAprovedOffersAsync()
        {
            var offers = await this.context.Offers
                .Where(x => x.IsApproved == false && x.IsDeclined == false)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
            return offers;
        }

        public async Task<List<Offer>> GetAllAprovedOffersAsync()
        {
            var offers = await this.context.Offers
                .Where(x => x.IsApproved == true && x.IsDeclined == false)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
            return offers;
        }

        public async Task<Offer> GetOfferByIdAsync(string id)
        {
            var offer = await this.context.Offers
                .FirstOrDefaultAsync(x => x.Id == id);
            return offer;
        }

        public async Task ApproveOfferAsync(string id)
        {
            var offer = await this.context.Offers
                .FirstOrDefaultAsync(x => x.Id == id);
            offer.IsApproved = true;
            await this.context.SaveChangesAsync();
        }

        public async Task RemoveOffer(string id)
        {
            var offer = await this.context.Offers
                .FirstOrDefaultAsync(x => x.Id == id);
            offer.IsDeclined = true;
            // offer.DeletedOn = DateTime.UtcNow;
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Offer>> GetAllDeletedOffersAsync()
        {
            var offers = await this.context.Offers
                .Where(x => x.IsDeclined == true)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
            return offers;
        }

        public async Task<List<Offer>> GetLatestOffers(int n)
        {
            var offers = await this.context.Offers
                .Where(x => x.IsApproved == true && x.IsDeclined == false)
                .OrderByDescending(x => x.CreatedOn)
                .Take(n)
                .ToListAsync();
            return offers;
        }

        public async Task FeatureItem(string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);
            offer.IsFeatured = true;
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Offer>> GetFeaturedOffers()
        {
            var offers = await this.context.Offers
                .Where(x => x.IsApproved == true && x.IsDeclined == false && x.IsFeatured == true)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();

            return offers;
        }

        public async Task RemoveFeatureOnItem(string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);
            offer.IsFeatured = false;
            await this.context.SaveChangesAsync();
        }

        public async Task AddReviewToOffer(CreateReviewModel inputModel)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == inputModel.Id);
            if (offer == null)
                throw new ArgumentNullException("Invalid Data");
            else if (int.Parse(inputModel.Rating) < 1 || int.Parse(inputModel.Rating) > 5)
                throw new ArgumentNullException("Invalid Data");
            else if (inputModel.Review == null)
                throw new ArgumentNullException("Invalid Data");

            var comment = new Comment()
            {
                Description = inputModel.Review,
                OfferId = inputModel.Id,
                RatingGiven = int.Parse(inputModel.Rating),
                CreatorId = inputModel.CreatorId,
            };

            offer.Comments.Add(comment);
            await this.context.SaveChangesAsync();
        }
    }
}
