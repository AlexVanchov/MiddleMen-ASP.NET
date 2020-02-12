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

    public class OfferController : BaseController
    {
        private readonly IOfferService offerService;

        private readonly ICategoryService categoryService;

        public OfferController(IOfferService offerService, ICategoryService categoryService)
        {
            this.offerService = offerService;  //todo rename sell to offer or revert
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();

            CreateOfferViewModel viewModel = new CreateOfferViewModel(categories);
            return this.View(viewModel);
        }

        public async Task<IActionResult> CreateOffer(CreateOfferModel inputModel) // index post requsest for create
        {
            var categoryId = await this.categoryService.GetIdByNameAsync(inputModel.CategoryId);
            inputModel.CategoryId = categoryId;
            inputModel.CreatorId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.offerService.CreateOfferAsync(inputModel);

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
    }
}
