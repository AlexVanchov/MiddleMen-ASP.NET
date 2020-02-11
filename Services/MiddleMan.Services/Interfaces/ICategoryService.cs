namespace MiddleMan.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.ViewModels;

    public interface ICategoryService
    {
        Task CreateCategoryAsync(CreateCategoryModel inputModel);

        Task<List<CategoryViewModel>> GetAllCategoryViewModelsAsync();

        Task<string> GetIdByNameAsync(string categoryTitle);
    }
}
