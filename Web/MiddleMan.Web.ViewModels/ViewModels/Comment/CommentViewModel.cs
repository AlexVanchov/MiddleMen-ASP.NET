namespace MiddleMan.Web.ViewModels.ViewModels.Comment
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CommentViewModel
    {
        public string Description { get; set; }

        public string CreatedOn { get; set; }

        public string CreatorName { get; set; }

        public int? RatingGiven { get; set; }
    }
}
