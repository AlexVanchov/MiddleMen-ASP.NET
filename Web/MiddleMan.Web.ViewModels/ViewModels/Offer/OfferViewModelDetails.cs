namespace MiddleMan.Web.ViewModels.ViewModels.Offer
{
    using System;

    public class OfferViewModelDetails
    {
        public OfferViewModelDetails()
        {
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string CreatorId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CategoryName { get; set; }

        public bool IsFeatured { get; set; }

        // todo comments
    }
}