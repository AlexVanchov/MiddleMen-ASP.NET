namespace MiddleMan.Web.ViewModels.InputModels.Message
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class SendMessageInputModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Content { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string RecipientId { get; set; }

        [Required]
        public string OfferId { get; set; }
    }
}
