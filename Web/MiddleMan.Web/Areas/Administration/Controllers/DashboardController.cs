namespace MiddleMan.Web.Areas.Administration.Controllers
{
    using MiddleMan.Services.Data;
    using MiddleMan.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using System.Threading.Tasks;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }
        public IActionResult CreateCategory()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CreateCategoryModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            //await categoryService.CreateCategoryAsync(inputModel);

            //TempData["CreatedAd"] = SuccessfullyCreatedAdMessage;


            return RedirectToAction("Index");
        }
    }
}
