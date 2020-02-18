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

    public class OfferController : BaseController
    {
        private readonly IOfferService offerService;

        private readonly ICategoryService categoryService;

        private readonly ICloudinaryService cloudinaryService;

        public OfferController(IOfferService offerService, ICategoryService categoryService, ICloudinaryService cloudinaryService)
        {
            this.offerService = offerService;  // todo rename sell to offer or revert
            this.categoryService = categoryService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();

            CreateOfferSharedModel viewModel = new CreateOfferSharedModel();
            viewModel.CreateOfferViewModel = new CreateOfferViewModel(categories);
            return this.View(viewModel);
        }

        public async Task<IActionResult> CreateOffer(CreateOfferSharedModel inputModel) // index post requsest for create
        {
            // IFormFile postedPic = this.Request.Form["Photo"];

            var a = inputModel.CreateOfferModel;
            var categoryId = await this.categoryService.GetIdByNameAsync(a.CategotyName);
            a.CategotyName = categoryId;
            a.CreatorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                a.Photo,
                $"{userId}-{a.Name}",
                GlobalConstants.CloudFolderForRecipePhotos);
            a.PicUrl = photoUrl;

            await this.offerService.CreateOfferAsync(a);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Details(string id)
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();
            var offer = await this.offerService.GetOfferByIdAsync(id);
            var category = await this.categoryService.GetCategoryNameByIdAsync(offer.CategoryId);

            var offerView = new OfferViewModelDetails()
            {
                CreatorId = offer.CreatorId,
                Description = offer.Description,
                Name = offer.Name,
                PicUrl = offer.PicUrl,
                Price = offer.Price,
                CreatedOn = offer.CreatedOn,
                Id = offer.Id,
            };

            var detailsModel = new DetailsViewModel(categories)
            {
                CategoryName = category,
                Categories = categories,
                Offer = offerView,
                CategoryId = offer.CategoryId,
            };

            return this.View(detailsModel);
        }

        public async Task<IActionResult> AddReview(CreateReviewModel inputModel) // index post requsest for create
        {
            // TODO
            return this.Redirect("/");
        }
    }
}
