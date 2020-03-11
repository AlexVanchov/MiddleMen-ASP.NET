namespace MiddleMan.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    using MiddleMan.Web.ViewModels.ViewModels.Offer;

    public class ApproveOffersViewModel
    {
        public ApproveOffersViewModel()
        {
            this.Offers = new HashSet<OfferViewModelDetails>();
        }

        public ICollection<OfferViewModelDetails> Offers { get; set; }
    }
}
