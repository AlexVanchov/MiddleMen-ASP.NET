using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiddleMan.Common;
using MiddleMan.Data;
using MiddleMan.Data.Models;
using MiddleMan.Services.Interfaces;

namespace MiddleMan.Web.Controllers
{
    public class ApiController : Controller
    {
        private readonly IOfferService offerService;
        private readonly IFavoriteService favoriteService;
        private readonly IUserService userService;
        private readonly IMessagesService messagesService;
        private readonly ICategoryService categoryService;
        private readonly ApplicationDbContext context;

        public ApiController(
            IOfferService offerService,
            IFavoriteService favoriteService,
            IUserService userService,
            IMessagesService messagesService,
            ICategoryService categoryService,
            ApplicationDbContext context)
        {
            this.offerService = offerService;
            this.favoriteService = favoriteService;
            this.userService = userService;
            this.messagesService = messagesService;
            this.categoryService = categoryService;
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
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return 0;
            }

            return await this.userService.GetAdminOffersForApprove();
        }

        [Authorize]
        public async Task<IActionResult> GetUnreadMessagesCount()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var unreadMessages = await this.messagesService.GetUnreadMessagesCountAsync(userId);

            return this.Json(unreadMessages);
        }

        [Authorize]
        public async Task<IActionResult> EditCategory(string categoryId, string newName)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Json(GlobalConstants.NoApiAccess);
            }

            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            category.Name = newName;
            await this.context.SaveChangesAsync();

            var categoryOfferCount = await this.categoryService.GetOffersCountInCategoryByIdAsync(categoryId);
            return this.Json(categoryOfferCount);
        }

        [Authorize]
        public async Task<IActionResult> DeleteCategory(string categoryId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Json(GlobalConstants.NoApiAccess);
            }

            var removeFavs = new List<UserFavorite>();

            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
            var categoryOffers = await this.context.Offers.Where(x => x.CategoryId == categoryId).ToListAsync();
            foreach (var offer in categoryOffers)
            {
                var favorites = this.context.UserFavorites.Where(x => x.OfferId == offer.Id);
                foreach (var fav in favorites)
                {
                    removeFavs.Add(fav);
                }

                offer.IsDeleted = true;
            }

            category.IsDeleted = true;
            this.context.UserFavorites.RemoveRange(removeFavs);
            await this.context.SaveChangesAsync();

            var categoryOfferCount = await this.categoryService.GetOffersCountInCategoryByIdAsync(categoryId);
            return this.Json(categoryOfferCount);
        }

        [Authorize]
        public async Task<IActionResult> ReorderCategories(string order)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.Json(GlobalConstants.NoApiAccess);
            }

            var orderArray = order.Split(",").ToList();
            var categories = await this.context.Categories.OrderBy(x => x.Position).ToListAsync();
            var categoriesNew = new List<Category>();

            for (int i = 1; i <= categories.Count; i++)
            {
                var categoryToGetNewPosition = categories.FirstOrDefault(x => x.Position == int.Parse(orderArray[i - 1]));
                categoriesNew.Add(categoryToGetNewPosition);
            }

            var counter = 1;
            foreach (var category in categoriesNew)
            {
                category.Position = counter;
                counter++;
            }

            await this.context.SaveChangesAsync();
            return this.Json(true);
        }
    }
}