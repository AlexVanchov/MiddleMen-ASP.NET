namespace MiddleMan.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using MiddleMan.Data.Models;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.ViewModels;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;

    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(CreateCategoryModel inputModel);

        Task<List<CategoryViewModel>> GetAllCategoryViewModelsAsync();

        Task<List<OfferViewModel>> GetAllOffersFromCategoryViewModelsAsync(string categoryId, string userId);

        Task<string> GetIdByNameAsync(string categoryTitle);

        Task<string> GetCategoryNameByIdAsync(string id);

        Task<int> GetOffersCountInCategoryByIdAsync(string categoryId);

        Task<double> GetOfferRatingAsync(string id);

        string StartsStringRating(double stars);
    }
}
