using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.InputModels
{
    public class CreateOfferModel
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string CategoryId { get; set; }

        public string CreatorId { get; set; }

    }
}
