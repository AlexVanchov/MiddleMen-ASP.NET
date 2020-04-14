using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.ViewModels.Orders
{
    public class OrdersHistoryViewModel
    {
        public OrdersHistoryViewModel()
        {
            this.Orders = new HashSet<OfferInCheckoutViewModel>();
        }

        public ICollection<OfferInCheckoutViewModel> Orders { get; set; }

        public string CurrentUserId { get; set; }
    }
}
