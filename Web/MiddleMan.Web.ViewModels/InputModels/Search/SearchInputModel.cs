namespace MiddleMan.Web.ViewModels.Search
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class SearchInputModel
    {
        [MinLength(1)]
        [Required]
        public string SearchWord { get; set; }
    }
}
