using Microsoft.AspNetCore.Mvc;
using MiddleMan.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleMan.Web.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IOfferService offerService;

        private readonly ICategoryService categoryService;

        public CategoryController(IOfferService offerService, ICategoryService categoryService)
        {
            this.offerService = offerService;  //todo rename sell to offer or revert
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string categoryName)
        {
            var categories = await this.categoryService.GetAllCategoryViewModelsAsync();

            // CreateOfferViewModel viewModel = new CreateOfferViewModel(categories);
            return this.View();
        }
    }
}
