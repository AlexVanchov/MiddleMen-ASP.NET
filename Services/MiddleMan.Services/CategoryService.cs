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
            var categories = this.context
                .Categories
                .ToList();

            var allCategories = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                allCategories.Add(new CategoryViewModel() { Name = category.Name });
            }

            return allCategories;
        }
    }
}
