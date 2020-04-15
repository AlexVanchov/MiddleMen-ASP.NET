namespace MiddleMan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Services.Mapping;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.ViewModels;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;

    public class CategoryService : ICategoryService
    {

        private readonly ApplicationDbContext context;
        private readonly IUserService userService;

        public CategoryService(
            ApplicationDbContext context,
            IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task CreateCategoryAsync(CreateCategoryModel inputModel)
        {
            var position = await this.context.Categories.CountAsync();
            Category category = new Category()
            {
                Name = inputModel.Name,
                Position = position + 1,
            };

            await this.context.Categories.AddAsync(category);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryViewModelsAsync()
        {
            var categories = await this.context
                .Categories
                .OrderBy(x => x.Position)
                .ToListAsync();

            var allCategories = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                var categoryOffersCount = await this.GetOffersCountInCategoryByIdAsync(category.Id);
                allCategories.Add(new CategoryViewModel() { Name = category.Name, Id = category.Id, OffersCount = categoryOffersCount, Position = category.Position });
            }

            return allCategories;
        }

        public async Task<List<OfferViewModel>> GetAllOffersFromCategoryViewModelsAsync(string id, string userId)
        {
            var offersOutput = new List<OfferViewModel>();
            var offers = await this.context.Offers
                .Where(x => x.CategoryId == id && x.IsApproved == true && x.IsDeclined == false && x.IsRemovedByUser == false)
                .ToListAsync();

            foreach (var offer in offers)
            {
                var offerRating = await this.GetOfferRatingAsync(offer.Id);
                var isFavoritedByUser = await this.userService.IsOfferFavoritedByUserAsync(offer.Id, userId);
                offersOutput.Add(new OfferViewModel()
                {
                    Id = offer.Id,
                    Name = offer.Name,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    Price = offer.Price,
                    PicUrl = offer.PicUrl,
                    ClickUrl = $"/Offer/Details?id={offer.Id}",
                    ReadMore = offer.Description.Length >= 65 ? true : false,
                    IsFavoritedByUser = isFavoritedByUser,
                    StartsStringRating = this.StartsStringRating(offerRating),
                });
            }

            return offersOutput;
        }

        public async Task<string> GetCategoryIdByNameAsync(string name)
        {
            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Name == name);
            if (category == null)
            {
                return null;
            }

            return category.Id;
        }

        public async Task<string> GetCategoryNameByIdAsync(string id)
        {
            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            return category.Name;
        }

        public async Task<string> GetIdByNameAsync(string categoryTitle)
        {
            var category = await this.context
                .Categories
                .FirstOrDefaultAsync(x => x.Name == categoryTitle);

            if (category == null)
            {
                throw new ArgumentNullException("category is null");
            }

            return category.Id;
        }

        public async Task<int> GetOffersCountInCategoryByIdAsync(string categoryId)
        {
            var categoryOffersCount = await this.context.Categories
                    .Where(x => x.Id == categoryId)
                    .OrderBy(x => x.Position)
                    .Select(x => x.Offers.Where(x => x.IsApproved == true && x.IsDeclined == false && x.IsRemovedByUser == false).ToList().Count)
                    .FirstOrDefaultAsync();

            return categoryOffersCount;
        }

        private async Task<double> GetOfferRatingAsync(string id)
        {
            var offers = await this.context.OfferUserRates.Where(x => x.OfferId == id).ToListAsync();
            if (offers.Count == 0)
            {
                return 0;
            }

            return Math.Round(offers.Average(x => (double)x.Rate));
        }

        private string StartsStringRating(double stars)
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
    }
}
