namespace MiddleMan.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using MiddleMan.Data.Common.Models;

    public class Comment : BaseDeletableModel<string>
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
            this.ModifiedOn = DateTime.UtcNow;
        }

        [Required]
        public string Description { get; set; }

        [Required]
        [ForeignKey("Offer")]
        public string OfferId { get; set; }

        public virtual Offer Offer { get; set; }

        [Required]
        public string CreatorId { get; set; }

        [Range(0, 6)]
        public int? RatingGiven { get; set; }
    }
}
