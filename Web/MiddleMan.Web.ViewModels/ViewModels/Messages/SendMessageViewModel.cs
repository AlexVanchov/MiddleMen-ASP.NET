namespace MiddleMan.Web.ViewModels.ViewModels.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SendMessageViewModel
    {
        public string RecipientId { get; set; }

        public string SenderId { get; set; }

        public string SellerPhone { get; set; }

        public string OfferId { get; set; }

        public string OfferName { get; set; }

        public double OfferPrice { get; set; }
    }
}
