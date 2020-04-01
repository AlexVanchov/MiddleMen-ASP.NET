namespace MiddleMan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public CategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateCategoryAsync(CreateCategoryModel inputModel)
        {
            Category category = new Category()
            {
                Name = inputModel.Name,
            };

            await this.context.Categories.AddAsync(category);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryViewModelsAsync()
        {
            var categories = await this.context
                .Categories
                .ToListAsync();

            var allCategories = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                allCategories.Add(new CategoryViewModel() { Name = category.Name, Id = category.Id });
            }

            return allCategories;
        }

        public async Task<List<OfferViewModel>> GetAllOffersFromCategoryViewModelsAsync(string id)
        {
            var offersOutput = new List<OfferViewModel>();
            var offers = await this.context.Offers
                .Where(x => x.CategoryId == id && x.IsApproved == true && x.IsDeclined == false && x.IsRemovedByUser == false)
                .ToListAsync();

            foreach (var offer in offers)
            {
                offersOutput.Add(new OfferViewModel()
                {
                    Id = offer.Id,
                    Name = offer.Name,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    Price = offer.Price,
                    PicUrl = offer.PicUrl,
                    ClickUrl = $"/Offer/Details?id={offer.Id}",
                    ReadMore = offer.Description.Length >= 65 ? true : false,
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
    }
}
