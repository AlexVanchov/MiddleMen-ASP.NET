namespace MiddleMan.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using MiddleMan.Data.Common.Repositories;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Data;
    using MiddleMan.Web.ViewModels.Sell;

    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels.ViewModels;
    using System.Collections.Generic;
    using MiddleMan.Web.ViewModels.InputModels;
    using System.Security.Claims;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;
    using MiddleMan.Services;
    using MiddleMan.Common;
    using Microsoft.AspNetCore.Http;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.ViewModels.Comment;
    using System.Linq;
    using MiddleMan.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Identity.UI.Services;

    public class UserController : BaseController
    {
        private readonly IOfferService offerService;

        private readonly ICategoryService categoryService;

        private readonly ICloudinaryService cloudinaryService;

        private readonly ICommentService commentService;

        private readonly IEmailSender emailSender;

        public UserController(
            IOfferService offerService,
            ICategoryService categoryService,
            ICloudinaryService cloudinaryService,
            ICommentService commentService,
            IEmailSender emailSender)
        {
            this.offerService = offerService;  // todo rename sell to offer or revert
            this.categoryService = categoryService;
            this.cloudinaryService = cloudinaryService;
            this.commentService = commentService;
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> MyOffers()
        {
            // TODO
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var offers = await this.offerService.GetAllUserOffersAsync(userId);

            var aproveModel = new ApproveOffersViewModel();

            foreach (var offer in offers)
            {
                var categoryName = await this.categoryService.GetCategoryNameByIdAsync(offer.CategoryId);
                aproveModel.Offers.Add(new OfferViewModelDetails()
                {
                    Name = offer.Name,
                    CreatedOn = offer.CreatedOn,
                    CreatorId = offer.CreatorId,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    PicUrl = offer.PicUrl,
                    Price = offer.Price,
                    Id = offer.Id,
                    CategoryName = categoryName,
                    IsFeatured = offer.IsFeatured,
                    IsRemovedByUser = offer.IsRemovedByUser,
                });
            }

            return this.View(aproveModel);
        }

        public async Task<IActionResult> Active()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var offers = await this.offerService.GetAllActiveUserOffersAsync(userId);

            var aproveModel = new ApproveOffersViewModel();

            foreach (var offer in offers)
            {
                var categoryName = await this.categoryService.GetCategoryNameByIdAsync(offer.CategoryId);
                aproveModel.Offers.Add(new OfferViewModelDetails()
                {
                    Name = offer.Name,
                    CreatedOn = offer.CreatedOn,
                    CreatorId = offer.CreatorId,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    PicUrl = offer.PicUrl,
                    Price = offer.Price,
                    Id = offer.Id,
                    CategoryName = categoryName,
                    IsFeatured = offer.IsFeatured,
                    IsRemovedByUser = offer.IsRemovedByUser,
                });
            }

            return this.View(aproveModel);
        }

        public async Task<IActionResult> Deactivated()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var offers = await this.offerService.GetAllDeactivatedUserOffersAsync(userId);

            var aproveModel = new ApproveOffersViewModel();

            foreach (var offer in offers)
            {
                var categoryName = await this.categoryService.GetCategoryNameByIdAsync(offer.CategoryId);
                aproveModel.Offers.Add(new OfferViewModelDetails()
                {
                    Name = offer.Name,
                    CreatedOn = offer.CreatedOn,
                    CreatorId = offer.CreatorId,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    PicUrl = offer.PicUrl,
                    Price = offer.Price,
                    Id = offer.Id,
                    CategoryName = categoryName,
                    IsFeatured = offer.IsFeatured,
                    IsRemovedByUser = offer.IsRemovedByUser,
                });
            }

            return this.View(aproveModel);
        }

        public async Task<IActionResult> Activate(string id)
        {
            await this.offerService.ActivateOfferAsUserAsync(id);

            return this.Redirect("/User/Deactivated");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.offerService.DeleteOfferAsUserAsync(id);

            return this.Redirect("/User/Active");
        }
    }
}
