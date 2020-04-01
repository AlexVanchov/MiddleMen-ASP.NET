namespace MiddleMan.Web.ViewModels.ViewModels.Offer
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OfferViewModel
    {
        public OfferViewModel()
        {
            this.ReadMore = false;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string ClickUrl { get; set; }

        public bool ReadMore { get; set; }

        public string CategoryName { get; set; }

        public bool IsFeatured { get; set; }

        public string StartsStringRating { get; set; }

        public double Rating { get; set; }
    }
}
