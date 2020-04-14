using Microsoft.AspNetCore.Mvc;
using MiddleMan.Data.Models;
using MiddleMan.Services;
using MiddleMan.Services.Interfaces;
using MiddleMan.Web.ViewModels.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiddleMan.Web.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOfferService offerService;

        private readonly ICategoryService categoryService;

        private readonly ICloudinaryService cloudinaryService;

        private readonly ICommentService commentService;

        private readonly IUserService userService;
        private readonly IOrderService orderService;

        public OrderController(
            IOfferService offerService,
            ICategoryService categoryService,
            ICloudinaryService cloudinaryService,
            ICommentService commentService,
            IUserService userService,
            IOrderService orderService)
        {
            this.offerService = offerService;
            this.categoryService = categoryService;
            this.cloudinaryService = cloudinaryService;
            this.commentService = commentService;
            this.userService = userService;
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Buy(string id)
        {
            var offer = await this.offerService.GetOfferByIdAsync(id);
            if (offer == null)
            {
                return this.Redirect("/");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await this.userService.GetUserByIdAsync(userId);

            var viewModel = new OrderViewModel()
            {
                Offer = new OfferInCheckoutViewModel()
                {
                    Id = offer.Id,
                    ImageUrl = offer.PicUrl,
                    Name = offer.Name,
                    Price = offer.Price,
                    CategoryName = await this.categoryService.GetCategoryNameByIdAsync(offer.CategoryId),
                },
                DeliveryInfo = new DeliveryInfoViewModel()
                {
                    Email = user.Email,
                    FirstName = user.FirstName == null ? string.Empty : user.FirstName,
                    LastName = user.LastName == null ? string.Empty : user.LastName,
                },
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(OrderViewModel viewData)
        {
            var offer = await this.offerService.GetOfferByIdAsync(viewData.BuyInput.OfferId);
            if (offer == null)
            {
                return this.Redirect("/");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await this.userService.GetUserByIdAsync(userId);

            await this.orderService.BuyOfferForUser(offer.Id, userId, viewData.BuyInput);

            return this.Redirect("/Order/History");
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await this.userService.GetUserByIdAsync(userId);

            var ordersViewModel = new List<OfferInCheckoutViewModel>();

            var orders = await this.orderService.GetOrdersHistoryForUser(userId);

            orders
                .OrderByDescending(x => x.CreatedOn)
                .ToList()
                .ForEach(delegate (Order order)
            {
                ordersViewModel.Add(new OfferInCheckoutViewModel()
                {
                    Id = order.OfferId,
                    Name = order.OfferName,
                    ImageUrl = order.OfferPicUrl,
                    Price = order.OfferPrice,
                    SoldBy = order.SellerId,
                    BoughtBy = order.UserId,
                });
            });

            var viewModel = new OrdersHistoryViewModel()
            {
                Orders = ordersViewModel,
                CurrentUserId = userId,
            };

            return this.View(viewModel);
        }
    }
}
