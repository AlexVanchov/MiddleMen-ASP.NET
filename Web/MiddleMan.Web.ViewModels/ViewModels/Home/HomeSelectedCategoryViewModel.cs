using MiddleMan.Data.Models;
using MiddleMan.Web.ViewModels.ViewModels.Offer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.ViewModels
{
    public class HomeSelectedCategoryViewModel : HomeViewModel
    {

        public HomeSelectedCategoryViewModel()
        {
            this.Offers = new HashSet<OfferViewModel>();
        }

        public string CategoryName { get; set; }

        public ICollection<OfferViewModel> Offers { get; set; }
    }
}
