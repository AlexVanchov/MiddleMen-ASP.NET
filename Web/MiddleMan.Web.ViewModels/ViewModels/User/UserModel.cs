namespace MiddleMan.Web.ViewModels.ViewModels.User
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class UserModel
    {
        public UserModel()
        {
            this.Roles = new HashSet<string>();
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }

        public string NormalizedEmail { get; set; }

        public string Email { get; set; }

        public string NormalizedUserName { get; set; }

        public string UserName { get; set; }

        public string ProfilePictureUrl { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
