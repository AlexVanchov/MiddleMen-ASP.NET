namespace MiddleMan.Web.ViewModels.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MiddleMan.Data.Models;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;

    public class HomeViewModel
    {
        public ICollection<CategoryViewModel> Categories { get; set; }

        public List<OfferViewModel> FeaturedOffers { get; set; }

        public ICollection<OfferViewModel> LatestOffers { get; set; }
    }
}
