namespace MiddleMan.Web.ViewModels.ViewModels.User
{
    using System;

    public class UserModel
    {
        public string Id { get; set; }

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
    }
}
