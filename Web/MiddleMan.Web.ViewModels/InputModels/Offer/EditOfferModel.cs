using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiddleMan.Web.ViewModels.InputModels.Offer
{
    public class EditOfferModel
    {
        public string Name { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string CategoryId { get; set; }

        public IFormFile Photo { get; set; }
    }
}
