namespace MiddleMan.Web.ViewModels.Administration.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UsersViewModel
    {
        public UsersViewModel()
        {
            this.Users = new HashSet<UserViewModel>();
        }

        public ICollection<UserViewModel> Users { get; set; }
    }
}
