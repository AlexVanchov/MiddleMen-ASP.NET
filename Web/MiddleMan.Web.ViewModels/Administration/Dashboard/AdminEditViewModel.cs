using MiddleMan.Web.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.Administration.Dashboard
{
    public class AdminEditViewModel
    {
        public AdminEditViewModel(ICollection<CategoryViewModel> categories)
        {
            this.Categories = categories;
        }

        public ICollection<CategoryViewModel> Categories { get; set; }
    }
}
