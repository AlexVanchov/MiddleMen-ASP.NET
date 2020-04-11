using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiddleMan.Data;
using MiddleMan.Services.Interfaces;

namespace MiddleMan.Web.Controllers
{
    public class ApiController : Controller
    {
        private readonly IOfferService offerService;
        private readonly IFavoriteService favoriteService;
        private readonly IUserService userService;
        private readonly IMessagesService messagesService;
        private readonly ApplicationDbContext context;

        public ApiController(
            IOfferService offerService,
            IFavoriteService favoriteService,
            IUserService userService,
            IMessagesService messagesService,
            ApplicationDbContext context)
        {
            this.offerService = offerService;
            this.favoriteService = favoriteService;
            this.userService = userService;
            this.messagesService = messagesService;
            this.context = context;
        }

        [Authorize]
        public async Task<IActionResult> Add(string offerId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isSuccess = await this.favoriteService.AddToFavoritesAsync(offerId, userId);

            return this.Json(isSuccess);
        }

        [Authorize]
        public async Task<IActionResult> Remove(string offerId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isSuccess = await this.favoriteService.RemoveFromFavoritesAsync(offerId, userId);

            return this.Json(isSuccess);
        }

        [Authorize]
        public async Task<IActionResult> GetFavoritesCount()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favorites = await this.offerService.GetAllFavoriteUserOffersKeysAsync(userId);

            return this.Json(favorites.Count());
        }

        [Authorize]
        public async Task<int> GetAdminOffersForApprove()
        {
            return await this.userService.GetAdminOffersForApprove();
        }

        [Authorize]
        public async Task<IActionResult> GetUnreadMessagesCount()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var unreadMessages = await this.messagesService.GetUnreadMessagesCountAsync(userId);

            return this.Json(unreadMessages);
        }
    }
}