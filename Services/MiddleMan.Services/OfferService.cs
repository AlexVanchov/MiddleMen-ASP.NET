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
    using MiddleMan.Web.ViewModels.InputModels.Offer;

    public class OfferService : IOfferService
    {

        private readonly ApplicationDbContext context;
        private readonly ICategoryService categoryService;

        public OfferService(
            ApplicationDbContext context,
            ICategoryService categoryService)
        {
            this.context = context;
            this.categoryService = categoryService;
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
                .Where(x => x.IsApproved == false && x.IsDeclined == false && x.IsRemovedByUser == false)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
            return offers;
        }

        public async Task<List<Offer>> GetAllAprovedOffersAsync()
        {
            var offers = await this.context.Offers
                .Where(x => x.IsApproved == true && x.IsDeclined == false && x.IsRemovedByUser == false)
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

        public async Task RemoveOfferAsync(string id)
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
                .Where(x => x.IsApproved == true && x.IsDeclined == false && x.IsRemovedByUser == false)
                .OrderByDescending(x => x.CreatedOn)
                .Take(n)
                .ToListAsync();
            return offers;
        }

        public async Task FeatureItemAsync(string id)
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

        public async Task RemoveFeatureOnItemAsync(string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);
            offer.IsFeatured = false;
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> IsOfferRatedAsync(string id, string userId)
        {
            return await this.context.OfferUserRates
                .AnyAsync(x => x.UserId == userId && x.OfferId == id) ? true : false;
        }

        public async Task<int?> GetRateForOffer(string id, string userId)
        {
            var offerUser = await this.context.OfferUserRates
                .FirstOrDefaultAsync(x => x.UserId == userId && x.OfferId == id);

            return offerUser.Rate;
        }

        public async Task<double> GetOfferRatingAsync(string id)
        {
            var offers = await this.context.OfferUserRates.Where(x => x.OfferId == id).ToListAsync();
            if (offers.Count == 0)
            {
                return 0;
            }

            return Math.Round(offers.Average(x => (double)x.Rate));
        }

        public string StartsStringRating(double stars)
        {
            string empty = "☆";
            string full = "★";

            var sb = new StringBuilder();

            int startsCount = 0;
            for (int i = 0; i < stars; i++)
            {
                sb.Append(full);
                startsCount++;
            }

            while (startsCount < 5)
            {
                sb.Append(empty);
                startsCount++;
            }

            return sb.ToString().TrimEnd();
        }

        public async Task<bool> IsUserCreatorOfOfferAsync(string userId, string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);
            return offer.CreatorId == userId ? true : false;
        }

        public async Task<List<Offer>> GetOffersBySearchAsync(string searchWord)
        {
            return await this.context.Offers
                .Where(x => x.Name.Contains(searchWord) || x.Description.Contains(searchWord))
                .ToListAsync();
        }

        public async Task EditOfferAsync(EditOfferModel offerInput, string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);

            offer.Name = offerInput.Name;
            offer.Price = offerInput.Price;
            offer.Description = offerInput.Description;
            offer.ModifiedOn = DateTime.UtcNow;
            offer.CategoryId = offerInput.CategoryId;

            await this.context.SaveChangesAsync();
        }

        public async Task ActivateOfferAsync(string id)
        {
            var offer = await this.context.Offers
                   .FirstOrDefaultAsync(x => x.Id == id);
            offer.IsDeclined = false;

            // offer.DeletedOn = DateTime.UtcNow;
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Offer>> GetAllUserOffersAsync(string userId)
        {
            return await this.context.Offers
                .Where(x => x.CreatorId == userId)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetAllActiveUserOffersAsync(string userId)
        {
            return await this.context.Offers
                .Where(x => x.CreatorId == userId && x.IsRemovedByUser == false)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetAllDeactivatedUserOffersAsync(string userId)
        {
            return await this.context.Offers
                .Where(x => x.CreatorId == userId && x.IsRemovedByUser == true)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetAllCategoryOffersAsync(string id)
        {
            return await this.context.Offers
                .Where(x => x.CategoryId == id && x.IsApproved == true && x.IsDeclined == false && x.IsRemovedByUser == false)
                .ToListAsync();
        }

        public async Task ActivateOfferAsUserAsync(string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);
            offer.IsRemovedByUser = false;
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteOfferAsUserAsync(string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);
            offer.IsRemovedByUser = true;
            await this.context.SaveChangesAsync();
        }
    }
}
