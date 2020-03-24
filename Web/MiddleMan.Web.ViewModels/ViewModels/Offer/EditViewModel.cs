using Microsoft.AspNetCore.Mvc;
using MiddleMan.Web.ViewModels.InputModels.Offer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.ViewModels.Offer
{
    public class EditViewModel : HomeViewModel
    {
        public ICollection<CategoryViewModel> Categories { get; set; }

        public string CategoryName { get; set; }

        public string CategoryId { get; set; }

        public OfferViewModelDetails Offer { get; set; }

        public EditOfferModel InputModel { get; set; }

        public bool IsOwner { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
    }
}