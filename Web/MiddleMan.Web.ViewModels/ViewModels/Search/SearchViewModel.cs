using MiddleMan.Web.ViewModels.ViewModels.Offer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.ViewModels.Search
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            this.Offers = new HashSet<OfferViewModelDetails>();
        }

        public ICollection<OfferViewModelDetails> Offers { get; set; }

        public string SearchWord { get; set; }
    }
}
