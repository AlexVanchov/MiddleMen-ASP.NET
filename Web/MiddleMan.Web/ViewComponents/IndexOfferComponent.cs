namespace MiddleMan.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;
    using System.Threading.Tasks;

    public class IndexOffer : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(OfferViewModel viewModel)
        {
            return this.View(viewModel);
        }
    }
}