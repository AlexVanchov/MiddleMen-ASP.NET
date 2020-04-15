namespace MiddleMan.Web.ViewModels.InputModels
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class CreateOfferModel
    {
        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 2000)]
        public double Price { get; set; }

        [Required]
        [MinLength(20), MaxLength(1000)]
        public string Description { get; set; }

        public string PicUrl { get; set; }

        [Required]
        public string CategotyName { get; set; }

        public string CreatorId { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

        [Required]
        public string BuyContent { get; set; }
    }
}
