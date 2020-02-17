namespace MiddleMan.Web.ViewModels.Administration.Dashboard
{
    using MiddleMan.Web.ViewModels.ViewModels.Offer;

    public class DetailsOfferViewModel
    {
        public string CategoryName { get; set; }

        public string CategoryId { get; set; }

        public OfferViewModelDetails Offer { get; set; }
    }
}
