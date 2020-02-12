using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.ViewModels.Offer
{
    public class DetailsViewModel : HomeViewModel
    {
        public DetailsViewModel(ICollection<CategoryViewModel> categories)
            : base(categories)
        {
            this.Categories = categories;
        }

        public ICollection<CategoryViewModel> Categories { get; set; }

        public string CategoryName { get; set; }

        public string CategoryId { get; set; }

        public OfferViewModelDetails Offer { get; set; }
    }
}
