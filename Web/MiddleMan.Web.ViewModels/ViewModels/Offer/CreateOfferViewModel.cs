﻿using Microsoft.AspNetCore.Http;
using MiddleMan.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.ViewModels
{
    public class CreateOfferViewModel
    {
        public CreateOfferViewModel(ICollection<CategoryViewModel> categories)
        {
            this.Categories = categories;
        }

        public ICollection<CategoryViewModel> Categories { get; set; }

    }
}
