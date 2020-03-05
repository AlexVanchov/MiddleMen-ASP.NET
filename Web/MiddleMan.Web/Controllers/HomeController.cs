namespace MiddleMan.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels;
    using MiddleMan.Web.ViewModels.ViewModels;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;

    public class HomeController : BaseController
    {
        private readonly ICategoryService categoryService;

        private readonly IOfferService offerService;
        private readonly IUserService userService;

        public HomeController(
            ICategoryService categoryService, 
            IOfferService offerService,
            IUserService userService)
        {
            this.categoryService = categoryService;
            this.offerService = offerService;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();
            var latestOffers = await this.offerService.GetLatestOffers(9);
            var latestOffersViewModel = new List<OfferViewModel>();
            var featuredOffers = await this.offerService.GetFeaturedOffers();
            var featuredOffersViewModel = new List<OfferViewModel>();

            foreach (var offer in latestOffers)
            {
                var categoryName = await this.categoryService.GetCategoryNameByIdAsync(offer.CategoryId);
                var offerRating = await this.offerService.GetOfferRatingAsync(offer.Id);

                latestOffersViewModel.Add(new OfferViewModel()
                {
                    Name = offer.Name,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    Price = offer.Price,
                    PicUrl = offer.PicUrl,
                    ClickUrl = $"/Offer/Details?id={offer.Id}",
                    ReadMore = offer.Description.Length >= 65 ? true : false,
                    CategoryName = categoryName,
                    StartsStringRating = this.offerService.StartsStringRating(offerRating),
                });
            }

            foreach (var offer in featuredOffers)
            {
                featuredOffersViewModel.Add(new OfferViewModel()
                {
                    Name = offer.Name,
                    Price = offer.Price,
                    PicUrl = offer.PicUrl,
                    ClickUrl = $"/Offer/Details?id={offer.Id}",
                });
            }

            var homeModel = new HomeViewModel()
            {
                Categories = categories,
                LatestOffers = latestOffersViewModel,
                FeaturedOffers = featuredOffersViewModel,
            };

            return this.View(homeModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public async Task<IActionResult> Category(string name)
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();
            var categoryId = await this.categoryService.GetCategoryIdByNameAsync(name);
            var offers = await this.categoryService.GetAllOffersFromCategoryViewModelsAsync(categoryId);
            // var category = await this.categoryService.GetCategoryNameByIdAsync(id);

            var homeModel = new HomeSelectedCategoryViewModel()
            {
                Categories = categories,
                CategoryName = name,
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

        public IActionResult Search()
        {
            return this.View();
        }
    }
}
