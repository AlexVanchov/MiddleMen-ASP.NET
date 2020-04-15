using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.ViewModels
{
    public class CategoryViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int OffersCount { get; set; }

        public int Position { get; set; }
    }
}
