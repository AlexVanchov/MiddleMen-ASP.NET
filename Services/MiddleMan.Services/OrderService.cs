namespace MiddleMan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Services.Messaging;
    using MiddleMan.Web.ViewModels.InputModels.Order;

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;
        private readonly IOfferService offerService;
        private readonly IEmailSender emailSender;

        public OrderService(
            ApplicationDbContext context,
            IOfferService offerService,
            IEmailSender emailSender)
        {
            this.context = context;
            this.offerService = offerService;
            this.emailSender = emailSender;
        }

        public async Task BuyOfferForUser(string id, string userId, BuyInput buyInput)
        {
            var offer = await this.offerService.GetOfferByIdAsync(id);

            if (offer == null)
            {
                return;
            }

            var order = new Order()
            {
                OfferId = id,
                OfferName = offer.Name,
                OfferPrice = offer.Price,
                OfferPicUrl = offer.PicUrl,
                UserId = userId,
                SellerId = offer.CreatorId,
                DeliveryEmail = buyInput.Email,
                UserFirstName = buyInput.FirstName,
                UserLastName = buyInput.LastName,
            };

            this.context.Orders.Add(order);
            this.context.SaveChanges();

            await this.emailSender.SendEmailAsync(
                    order.DeliveryEmail,
                    "Order Details",
                    $"{order.Id.ToString()}");
        }

        public async Task<List<Order>> GetOrdersHistoryForUser(string userId)
        {
            return await this.context.Orders.Where(x => x.UserId == userId || x.SellerId == userId).ToListAsync();
        }
    }
}
