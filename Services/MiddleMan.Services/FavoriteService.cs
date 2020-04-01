namespace MiddleMan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Interfaces;

    public class FavoriteService : IFavoriteService
    {
        private readonly ApplicationDbContext context;
        private readonly IOfferService offerService;

        public FavoriteService(ApplicationDbContext context, IOfferService offerService)
        {
            this.context = context;
            this.offerService = offerService;
        }

        public async Task<bool> AddToFavoritesAsync(string offerId, string userId)
        {
            var offer = await this.offerService.GetOfferByIdAsync(offerId);

            if (offer != null)
            {
                var userFavorite = new UserFavorite()
                {
                    UserId = userId,
                    OfferId = offerId,
                };

                await this.context.UserFavorites.AddAsync(userFavorite);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveFromFavoritesAsync(string offerId, string userId)
        {
            var userFavorite = await this.context.UserFavorites.FirstOrDefaultAsync(x => x.OfferId == offerId && x.UserId == userId);

            if (userFavorite != null)
            {
                this.context.UserFavorites.Remove(userFavorite);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
