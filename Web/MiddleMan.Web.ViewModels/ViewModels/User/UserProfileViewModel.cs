namespace MiddleMan.Web.ViewModels.ViewModels.User
{
    using MiddleMan.Web.ViewModels.ViewModels.Offer;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserProfileViewModel
    {
        public UserProfileViewModel()
        {
            this.UserOffers = new HashSet<OfferViewModel>();
        }

        public UserModel User { get; set; }

        public ICollection<OfferViewModel> UserOffers { get; set; }
    }
}
