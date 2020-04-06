namespace MiddleMan.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserFavorite
    {
        public UserFavorite()
        {
            this.FavoritedOn = DateTime.UtcNow;
        }

        public string UserId { get; set; }

        public string OfferId { get; set; }

        public DateTime FavoritedOn { get; set; }
    }
}
