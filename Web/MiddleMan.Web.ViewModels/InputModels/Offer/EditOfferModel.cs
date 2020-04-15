using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MiddleMan.Web.ViewModels.InputModels.Offer
{
    public class EditOfferModel
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

        [Required]
        public string CategoryName { get; set; }

        public string CategoryId { get; set; }

        public IFormFile Photo { get; set; }

        [Required]
        public string BuyContent { get; set; }
    }
}
