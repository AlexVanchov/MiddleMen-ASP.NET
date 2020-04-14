namespace MiddleMan.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using MiddleMan.Data.Common.Models;

    public class Order : BaseDeletableModel<int>
    {
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        [ForeignKey("ApplicationUser")]
        public string SellerId { get; set; }

        public virtual ApplicationUser Seller { get; set; }

        [ForeignKey("Offer")]
        public string OfferId { get; set; }

        public virtual Offer Offer { get; set; }

        public string OfferName { get; set; }

        public double OfferPrice { get; set; }

        public string OfferPicUrl { get; set; }

        public string DeliveryEmail { get; set; }
    }
}
