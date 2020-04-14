namespace MiddleMan.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class OfferUserRate
    {
        [ForeignKey("Offer")]
        public string OfferId { get; set; }

        public virtual Offer Offer { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int? Rate { get; set; }
    }
}
