namespace MiddleMan.Services
{
    using System;
    using System.Threading.Tasks;
    using MiddleMan.Data;
    using MiddleMan.Data.Models;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;

    public class CategoryService
    {

        private readonly ApplicationDbContext context;

        public async Task CreateCategoryAsync(CreateCategoryModel inputModel)
        {
            Category category = new Category()
            {
                Name = inputModel.Name,
            };

            //await context.ads.addasync(ad);
            //await context.savechangesasync();
        }
    }
}
