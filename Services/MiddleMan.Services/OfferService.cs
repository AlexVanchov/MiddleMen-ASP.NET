namespace MiddleMan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Common;
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
        private readonly ICloudinaryService cloudinaryService;

        public OfferService(
            ApplicationDbContext context,
            ICategoryService categoryService,
            ICloudinaryService cloudinaryService)
        {
            this.context = context;
            this.categoryService = categoryService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<Offer> CreateOfferAsync(CreateOfferModel inputModel)
        {
            var offer = new Offer()
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                Price = inputModel.Price,
                PicUrl = inputModel.PicUrl,
                CategoryId = inputModel.CategotyName,
                CreatorId = inputModel.CreatorId,
                BuyContent = inputModel.BuyContent,
            };

            await this.context.Offers.AddAsync(offer);
            await this.context.SaveChangesAsync();

            return offer;
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
                .FirstOrDefaultAsync(x => x.Id == id && x.IsApproved == true && x.IsDeclined == false && x.IsRemovedByUser == false);
            return offer;
        }

        public async Task<Offer> GetOfferAnywayAsync(string id)
        {
            var offer = await this.context.Offers
                .FirstOrDefaultAsync(x => x.Id == id);
            return offer;
        }

        public async Task ApproveOfferAsync(string id)
        {
            if (await this.IsOfferExisting(id))
            {
                var offer = await this.context.Offers
                    .FirstOrDefaultAsync(x => x.Id == id);
                offer.IsApproved = true;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task RemoveOfferAsync(string id)
        {
            if (await this.IsOfferExisting(id))
            {
                var offer = await this.context.Offers
                .FirstOrDefaultAsync(x => x.Id == id);
                offer.IsDeclined = true;
                // offer.DeletedOn = DateTime.UtcNow;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<List<Offer>> GetAllDeletedOffersAsync()
        {
            var offers = await this.context.Offers
                .Where(x => x.IsDeclined == true && x.IsRemovedByUser == false)
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

        public async Task<List<Offer>> GetFeaturedOffersAsync()
        {
            var offers = await this.context.Offers
                .Where(x => x.IsApproved == true && x.IsDeclined == false && x.IsFeatured == true && x.IsRemovedByUser == false)
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

            if (offerUser != null)
            {
                return offerUser.Rate;
            }

            return 0;
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
            return offer != null ? offer.CreatorId == userId ? true : false : false;
        }

        public async Task<List<Offer>> GetOffersBySearchAsync(string searchWord)
        {
            searchWord = searchWord.ToLower();
            var offers = await this.context.Offers
                .Where(x => x.Name.ToLower().Contains(searchWord) || x.Description.ToLower().Contains(searchWord) || x.CategoryId.ToLower().Contains(searchWord))
                .ToListAsync();

            return offers;
        }

        public async Task<Offer> EditOfferAsync(EditOfferModel offerInput, string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);

            if (offer != null)
            {
                offer.Name = offerInput.Name;
                offer.Price = offerInput.Price;
                offer.Description = offerInput.Description;
                offer.ModifiedOn = DateTime.UtcNow;
                offer.CategoryId = offerInput.CategoryId;
                offer.BuyContent = offerInput.BuyContent;

                if (offerInput.Photo != null)
                {
                    var photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                        offerInput.Photo,
                        $"{id}-{DateTime.Now.ToString()}",
                        GlobalConstants.CloudFolderForProfilePictures);
                    offer.PicUrl = photoUrl;
                }

                await this.context.SaveChangesAsync();

                return offer;
            }

            return null;
        }

        public async Task<Offer> ActivateOfferAsync(string id)
        {
            var offer = await this.context.Offers
                   .FirstOrDefaultAsync(x => x.Id == id);

            if (offer != null)
            {
                offer.IsDeclined = false;

                // offer.DeletedOn = DateTime.UtcNow;
                await this.context.SaveChangesAsync();

                return offer;
            }

            return null;
        }

        public async Task<List<Offer>> GetAllUserOffersAsync(string userId)
        {
            return await this.context.Offers
                .Where(x => x.CreatorId == userId && x.IsApproved == true && x.IsDeclined == false)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetAllActiveUserOffersAsync(string userId)
        {
            return await this.context.Offers
                .Where(x => x.CreatorId == userId && x.IsRemovedByUser == false && x.IsApproved == true && x.IsDeclined == false)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetAllDeactivatedUserOffersAsync(string userId)
        {
            return await this.context.Offers
                .Where(x => x.CreatorId == userId && x.IsRemovedByUser == true && x.IsApproved == true && x.IsDeclined == false)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetAllCategoryOffersAsync(string id)
        {
            return await this.context.Offers
                .Where(x => x.CategoryId == id && x.IsApproved == true && x.IsDeclined == false && x.IsRemovedByUser == false)
                .ToListAsync();
        }

        public async Task<Offer> ActivateOfferAsUserAsync(string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);

            if (offer != null)
            {
                offer.IsRemovedByUser = false;
                await this.context.SaveChangesAsync();

                return offer;
            }

            return null;
        }

        public async Task<Offer> DeleteOfferAsUserAsync(string id)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == id);

            if (offer != null)
            {
                offer.IsRemovedByUser = true;
                await this.context.SaveChangesAsync();

                return offer;
            }

            return null;
        }

        public async Task<List<UserFavorite>> GetAllFavoriteUserOffersKeysAsync(string userId)
        {
            var favorites = await this.context.UserFavorites
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.FavoritedOn)
                .ToListAsync();

            var validFavorites = new List<UserFavorite>();

            foreach (var fav in favorites)
            {
                var offer = await this.GetOfferByIdAsync(fav.OfferId);

                if (offer != null)
                {
                    validFavorites.Add(fav);
                }
            }

            return validFavorites;
        }

        public async Task<string> GetOfferNameById(string offerId)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == offerId);
            if (offer != null)
            {
                return offer.Name;
            }
            else return null;
        }

        public async Task<bool> IsOfferExisting(string offerId)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == offerId);
            if (offer == null)
            {
                return false;
            }

            return true;
        }
    }
}
