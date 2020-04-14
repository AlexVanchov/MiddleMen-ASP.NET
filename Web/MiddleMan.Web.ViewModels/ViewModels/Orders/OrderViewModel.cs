namespace MiddleMan.Web.ViewModels.ViewModels.Orders
{
    using MiddleMan.Web.ViewModels.InputModels.Order;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OrderViewModel
    {
        public OfferInCheckoutViewModel Offer { get; set; }

        public DeliveryInfoViewModel DeliveryInfo { get; set; }

        public BuyInput BuyInput { get; set; }
    }
}
