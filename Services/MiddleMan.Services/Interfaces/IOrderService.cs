using MiddleMan.Data.Models;
using MiddleMan.Web.ViewModels.InputModels.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiddleMan.Services.Interfaces
{
    public interface IOrderService
    {
        Task BuyOfferForUser(string id, string userId, BuyInput buyInput);

        Task<List<Order>> GetOrdersHistoryForUser(string userId);
    }
}
