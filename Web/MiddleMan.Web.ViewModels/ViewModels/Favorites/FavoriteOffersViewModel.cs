namespace MiddleMan.Web.ViewModels.ViewModels.Favorites
{
    using System.Collections.Generic;

    using MiddleMan.Web.ViewModels.ViewModels.Offer;

    public class FavoriteOffersViewModel
    {
        public FavoriteOffersViewModel()
        {
            this.Offers = new HashSet<OfferViewModel>();
        }

        public ICollection<OfferViewModel> Offers { get; set; }
    }
}
