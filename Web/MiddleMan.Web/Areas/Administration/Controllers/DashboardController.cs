namespace MiddleMan.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Services;
    using MiddleMan.Services.Data;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels.Administration.Dashboard;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
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
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.categoryService.CreateCategoryAsync(inputModel);

            //TempData["CreatedAd"] = SuccessfullyCreatedAdMessage;


            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> ForApprove()
        {
            var offers = await this.offerService.GetAllNotAprovedOffersAsync();

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
                });
            }

            return this.View(aproveModel);
        }

        public async Task<IActionResult> Approve(string id)
        {
            await this.offerService.ApproveOfferAsync(id);

            return this.Redirect("/Administration/Dashboard/ForApprove");
        }

        public async Task<IActionResult> Remove(string id)
        {
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
                IsFeatured = offer.IsFeatured,
            };

            var detailsModel = new DetailsOfferViewModel()
            {
                CategoryName = category,
                Offer = offerView,
                CategoryId = offer.CategoryId,
            };

            return this.View("Preview", detailsModel);
        }

        public async Task<IActionResult> RemoveConfirmed(string id)
        {
            await this.offerService.RemoveOfferAsync(id);

            return this.Redirect("/Administration/Dashboard/Approved");
        }

        public async Task<IActionResult> Deleted()
        {
            var offers = await this.offerService.GetAllDeletedOffersAsync();

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
                });
            }

            return this.View(aproveModel);
        }

        public async Task<IActionResult> RemovedOffer(string id)
        {
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
                IsFeatured = offer.IsFeatured,
            };

            var detailsModel = new DetailsOfferViewModel()
            {
                CategoryName = category,
                Offer = offerView,
                CategoryId = offer.CategoryId,
            };

            return this.View(detailsModel);
        }

        public async Task<IActionResult> Boost(string id)
        {
            await this.offerService.FeatureItemAsync(id);

            return this.Redirect("/Administration/Dashboard/Approved");
        }

        public async Task<IActionResult> RemoveBoost(string id)
        {
            await this.offerService.RemoveFeatureOnItemAsync(id);

            return this.Redirect("/Administration/Dashboard/Approved");
        }

        public async Task<IActionResult> ActivateOffer(string id)
        {
            await this.offerService.ActivateOfferAsync(id);

            return this.Redirect("/Administration/Dashboard/Deleted");
        }
    }
}
