namespace MiddleMan.Web.ViewModels.InputModels
{
    using Microsoft.AspNetCore.Http;

    public class CreateOfferModel
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string CategotyName { get; set; }

        public string CreatorId { get; set; }

        public IFormFile Photo { get; set; }

        public string BuyContent { get; set; }
    }
}
