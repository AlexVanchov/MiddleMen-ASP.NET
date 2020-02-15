﻿namespace MiddleMan.Web.Areas.Administration.Controllers
{
    using MiddleMan.Services.Data;
    using MiddleMan.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using System.Threading.Tasks;
    using MiddleMan.Services;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly ICategoryService categoryService;
        private readonly IOfferService offerService;

        public DashboardController(ISettingsService settingsService, ICategoryService categoryService, IOfferService offerService)
        {
            this.settingsService = settingsService;
            this.categoryService = categoryService;
            this.offerService = offerService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> EditCategories()
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();

            var editModel = new AdminEditViewModel(categories);

            return this.View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CreateCategoryModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            await this.categoryService.CreateCategoryAsync(inputModel);

            //TempData["CreatedAd"] = SuccessfullyCreatedAdMessage;


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ForApprove()
        {
            var offers = await this.offerService.GetAllNotAprovedOffersAsync();

            var aproveModel = new ApproveOffersViewModel();

            foreach (var offer in offers)
            {
                aproveModel.Offers.Add(new OfferViewModelDetails()
                {
                    Name = offer.Name,
                    CreatedOn = offer.CreatedOn,
                    CreatorId = offer.CreatorId,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    PicUrl = offer.PicUrl,
                    Price = offer.Price,
                    Id = offer.Id,
                });
            }

            return this.View(aproveModel);
        }

        public async Task<IActionResult> Approved()
        {
            var offers = await this.offerService.GetAllAprovedOffersAsync();

            var aproveModel = new ApproveOffersViewModel();

            foreach (var offer in offers)
            {
                aproveModel.Offers.Add(new OfferViewModelDetails()
                {
                    Name = offer.Name,
                    CreatedOn = offer.CreatedOn,
                    CreatorId = offer.CreatorId,
                    Description = offer.Description.Length >= 65 ? offer.Description.Substring(0, 65) : offer.Description,
                    PicUrl = offer.PicUrl,
                    Price = offer.Price,
                    Id = offer.Id,
                });
            }

            return this.View(aproveModel);
        }

        public async Task<IActionResult> Approve(string id)
        {
            await this.offerService.ApproveOfferAsync(id);

            return this.Redirect("/Administration/Dashboard/ForApprove");
        }
    }
}
