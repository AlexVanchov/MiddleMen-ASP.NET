using MiddleMan.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MiddleMan.Data.Models
{
    public class Offer : BaseDeletableModel<int>
    {
        public Offer()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
            this.ModifiedOn = DateTime.UtcNow;
            this.IsApproved = false;
            this.IsDeclined = false;
            this.IsFeatured = false;
            this.IsRemovedByUser = false;
            this.Comments = new HashSet<Comment>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, 5000)]
        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        public string PicUrl { get; set; }

        [Required]
        [ForeignKey("Category")]
        public string CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        public string CreatorId { get; set; }

        [Required]
        public bool IsFeatured { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        public bool IsDeclined { get; set; }

        [Required]
        public bool IsRemovedByUser { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
