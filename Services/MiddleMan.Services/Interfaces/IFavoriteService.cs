using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiddleMan.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<bool> AddToFavoritesAsync(string offerId, string userId);

        Task<bool> RemoveFromFavoritesAsync(string offerId, string userId);
    }
}
