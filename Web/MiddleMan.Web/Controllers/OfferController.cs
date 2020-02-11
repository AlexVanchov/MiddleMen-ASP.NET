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

            await this.offerService.CreateOfferAsync(inputModel);

            return this.Redirect("/");
        }
    }
}
