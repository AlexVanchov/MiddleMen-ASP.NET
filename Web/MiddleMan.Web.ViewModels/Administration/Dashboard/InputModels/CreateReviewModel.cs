namespace MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class CreateReviewModel
    {
        [Required]
        [Range(1, 5)]
        public string Rating { get; set; }

        public string Review { get; set; }

        public string CreatorId { get; set; }

        public string Id { get; set; }
    }
}
