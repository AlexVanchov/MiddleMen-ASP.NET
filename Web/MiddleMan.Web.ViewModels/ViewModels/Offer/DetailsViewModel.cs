﻿namespace MiddleMan.Web.ViewModels.ViewModels.Offer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.InputModels.Offer;
    using MiddleMan.Web.ViewModels.ViewModels.Comment;

    public class DetailsViewModel : HomeViewModel
    {
        public ICollection<CategoryViewModel> Categories { get; set; }

        public string CategoryName { get; set; }

        public string CategoryId { get; set; }

        public OfferViewModelDetails Offer { get; set; }

        public int? UserRated { get; set; }

        public CreateReviewModel CommentInputModel { get; set; }

        public EditOfferModel EditInputModel { get; set; }

        public bool IsOwner { get; set; }
    }
}
