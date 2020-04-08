namespace MiddleMan.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using MiddleMan.Data.Common.Models;

    public class Message : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public bool IsRead { get; set; }

        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        [ForeignKey("AspNetUsers")]
        public string RecipientId { get; set; }

        public virtual ApplicationUser Recipient { get; set; }

        [ForeignKey("Offer")]
        public string OfferId { get; set; }

        public virtual Offer Offer { get; set; }
    }
}
