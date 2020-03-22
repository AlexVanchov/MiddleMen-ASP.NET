namespace MiddleMan.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using MiddleMan.Common;
    using MiddleMan.Data.Models;
    using MiddleMan.Services;
    using MiddleMan.Services.Interfaces;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService userService;
        private readonly ICloudinaryService cloudinaryService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService,
            ICloudinaryService cloudinaryService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.userService = userService;
            this.cloudinaryService = cloudinaryService;
        }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Update Profile Photo")]
            public IFormFile ProfilePhoto { get; set; }

            [MinLength(3)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [MinLength(3)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this._userManager.GetUserNameAsync(user);
            var phoneNumber = await this._userManager.GetPhoneNumberAsync(user);
            var profilePhotoUrl = await this.userService.GetUserProfilePictureUrlAsync(user.Id);
            var firstName = await this.userService.GetUserFirstNameAsync(user.Id);
            var lastName = await this.userService.GetUserLastNameAsync(user.Id);

            this.Username = userName;
            this.ProfilePhotoUrl = profilePhotoUrl;

            this.Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (Input.ProfilePhoto != null)
            {
                var imgSize = Input.ProfilePhoto.Length;

                if (imgSize >= 1048576)
                {
                    StatusMessage = "Profile picture is too powerful";
                    return RedirectToPage();
                }
                else
                {
                    var photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                    this.Input.ProfilePhoto,
                    $"{user.Id}-{DateTime.Now.ToString()}",
                    GlobalConstants.CloudFolderForProfilePictures);

                    await this.userService.UpdateProfilePictureUrl(user.Id, photoUrl);
                }
            }

            await this.userService.UpdateUserFirstAndLastNameAsync(user.Id, Input.FirstName, Input.LastName);

            await this._signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
