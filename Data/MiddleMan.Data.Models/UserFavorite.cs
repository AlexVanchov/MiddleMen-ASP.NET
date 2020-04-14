namespace MiddleMan.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class UserFavorite
    {
        public UserFavorite()
        {
            this.FavoritedOn = DateTime.UtcNow;
        }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [ForeignKey("Offer")]
        public string OfferId { get; set; }

        public DateTime FavoritedOn { get; set; }
    }
}
