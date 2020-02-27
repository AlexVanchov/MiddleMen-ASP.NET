namespace MiddleMan.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OfferUserRate
    {
        public string OfferId { get; set; }

        public virtual Offer Offer { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int? Rate { get; set; }
    }
}
