namespace MiddleMan.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels;
    using MiddleMan.Web.ViewModels.ViewModels;

    public class HomeController : BaseController
    {
        private readonly ICategoryService categoryService;

        private readonly IOfferService offerService;

        public HomeController(ICategoryService categoryService, IOfferService offerService)
        {
            this.categoryService = categoryService;
            this.offerService = offerService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();

            var homeModel = new HomeViewModel(categories);
            return this.View(homeModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public async Task<IActionResult> Category(string id)
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();
            var offers = await this.categoryService.GetAllOffersFromCategoryViewModelsAsync(id);

            var homeModel = new HomeSelectedCategoryViewModel(categories)
            {
                Offers = offers,
            };

            return this.View(homeModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
