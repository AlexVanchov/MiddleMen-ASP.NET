using MiddleMan.Web.ViewModels.ViewModels.Offer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.Administration.Dashboard
{
    public class ApproveOffersViewModel
    {
        public ApproveOffersViewModel()
        {
            this.Offers = new HashSet<OfferViewModelDetails>();
        }

        public ICollection<OfferViewModelDetails> Offers { get; set; }
    }
}
