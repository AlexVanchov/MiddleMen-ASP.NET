namespace MiddleMan.Web.Areas.Administration.Controllers
{
    using MiddleMan.Services.Data;
    using MiddleMan.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using System.Threading.Tasks;
    using MiddleMan.Services;
    using MiddleMan.Services.Interfaces;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly ICategoryService categoryService;

        public DashboardController(ISettingsService settingsService, ICategoryService categoryService)
        {
            this.settingsService = settingsService;
            this.categoryService = categoryService;
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
        public IActionResult EditCategories()
        {
            var categories = this.categoryService.GetAllCategoryViewModels();

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
    }
}
