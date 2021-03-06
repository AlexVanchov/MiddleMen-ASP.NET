﻿namespace MiddleMan.Web.ViewModels.ViewModels.Offer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using MiddleMan.Web.ViewModels.ViewModels.Comment;

    public class OfferViewModelDetails
    {
        public OfferViewModelDetails()
        {
            this.Comments = new HashSet<CommentViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string CreatorId { get; set; }

        public string CreatorName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CategoryName { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsRemovedByUser { get; set; }

        public bool Rated { get; set; }

        public double OfferRating { get; set; }

        public string StartsStringRating { get; set; }

        public bool IsFavoritedByUser { get; set; }

        public string BuyContent { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
