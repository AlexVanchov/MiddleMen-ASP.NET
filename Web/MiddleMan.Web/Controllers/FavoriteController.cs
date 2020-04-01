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

    public class FavoriteController : Controller
    {
        private readonly IFavoriteService favoritesService;

        public FavoriteController(IFavoriteService favoritesService)
        {
            this.favoritesService = favoritesService;
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
    }
}