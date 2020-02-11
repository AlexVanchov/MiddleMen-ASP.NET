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
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
            this.Id = Guid.NewGuid().ToString();
            this.ModifiedOn = DateTime.UtcNow;
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, 5000)]
        public double Price { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        [ForeignKey("Category")]
        public string CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
