using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.ViewModels.Offer
{
    public class OfferViewModel
    {
        public OfferViewModel()
        {
            this.ReadMore = false;
        }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string ClickUrl { get; set; }

        public bool ReadMore { get; set; }
    }
}
