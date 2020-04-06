namespace MiddleMan.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels.ViewModels.Favorites;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;

    public class FavoritesController : Controller
    {
        private readonly IFavoriteService favoritesService;
        private readonly IOfferService offerService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;

        public FavoritesController(
            IFavoriteService favoritesService,
            IOfferService offerService,
            ICategoryService categoryService,
            IUserService userService)
        {
            this.favoritesService = favoritesService;
            this.offerService = offerService;
            this.categoryService = categoryService;
            this.userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favoriteOfferUser = await this.offerService.GetAllFavoriteUserOffersKeysAsync(userId);
            var favoritesModel = new FavoriteOffersViewModel();

            foreach (var favoriteOffer in favoriteOfferUser)
            {
                var offer = await this.offerService.GetOfferByIdAsync(favoriteOffer.OfferId);

                var categoryName = await this.categoryService.GetCategoryNameByIdAsync(offer.CategoryId);
                var offerRating = await this.offerService.GetOfferRatingAsync(offer.Id);
                var isFavoritedByUser = await this.userService.IsOfferFavoritedByUserAsync(offer.Id, userId);

                favoritesModel.Offers.Add(new OfferViewModel()
                {
                    Id = offer.Id,
                    Name = offer.Name,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    Price = offer.Price,
                    PicUrl = offer.PicUrl,
                    ClickUrl = $"/Offer/Details?id={offer.Id}",
                    ReadMore = offer.Description.Length >= 65 ? true : false,
                    CategoryName = categoryName,
                    StartsStringRating = this.offerService.StartsStringRating(offerRating),
                    IsFavoritedByUser = isFavoritedByUser,
                });
            }

            return this.View(favoritesModel);
        }

        [Authorize]
        public async Task<IActionResult> Add(string offerId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isSuccess = await this.favoritesService.AddToFavoritesAsync(offerId, userId);

            return this.Json(isSuccess);
        }

        [Authorize]
        public async Task<IActionResult> Remove(string offerId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isSuccess = await this.favoritesService.RemoveFromFavoritesAsync(offerId, userId);

            return this.Json(isSuccess);
        }

        [Authorize]
        public async Task<IActionResult> GetFavoritesCount()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorites = await this.offerService.GetAllFavoriteUserOffersKeysAsync(userId);

            return this.Json(favorites.Count());
        }
    }
}