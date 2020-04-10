namespace MiddleMan.Web.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MiddleMan.Web.ViewModels.InputModels.Message;

    public class SendMessageComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(SendMessageInputModel viewModel)
        {
            return this.View(viewModel);
        }
    }
}