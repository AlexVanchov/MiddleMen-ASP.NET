﻿namespace MiddleMan.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MiddleMan.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
            this.Id = Guid.NewGuid().ToString();
            this.ModifiedOn = DateTime.UtcNow;
        }

        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}
