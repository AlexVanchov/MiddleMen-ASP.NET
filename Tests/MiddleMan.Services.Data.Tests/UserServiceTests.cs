namespace MiddleMan.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using MiddleMan.Services.Data;
    using MiddleMan.Services.Interfaces;
    using Moq;
    using Xunit;
    using Microsoft.AspNetCore.Http;
    using MiddleMan.Web.ViewModels.InputModels;
    using MiddleMan.Data.Models;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Web.ViewModels.InputModels.Offer;
    using Microsoft.AspNetCore.Identity;
    using System.Security.Claims;
    using System.Security.Principal;

    public class UserServiceTests
    {
        private IUserService userService;
        private IOfferService offerService;

        public UserServiceTests()
        {
        }

        [Fact]
        public async Task GetUsernameByIdAsync_WithValidData_ShouldReturnUsername()
        {
            var expected = "TestUser";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var username = await this.userService.GetUsernameByIdAsync(guid);
            Assert.Equal(expected, username);
        }

        [Fact]
        public async Task GetUsernameByIdAsync_WithInValidData_ShouldReturnNull()
        {
            var expected = "TestUser";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var username = await this.userService.GetUsernameByIdAsync("InvalidID");
            Assert.Null(username);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithValidData_ShouldReturnUser()
        {
            var expected = "TestUser";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserByIdAsync(guid);
            Assert.Equal(user, userGet);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithInValidData_ShouldReturnNull()
        {
            var expected = "TestUser";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserProfilePictureUrlAsync("InvalidID");
            Assert.Null(userGet);
        }

        [Fact]
        public async Task GetUserProfilePictureUrlAsync_WithValidData_ShouldReturnUrl()
        {
            var expected = "TestUrl.com";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserProfilePictureUrlAsync(guid);
            Assert.Equal(expected, userGet);
        }

        [Fact]
        public async Task GetUserProfilePictureUrlAsync_WithInValidData_ShouldReturnNull()
        {
            var expected = "TestUser";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserProfilePictureUrlAsync("InvalidID");
            Assert.Null(userGet);
        }

        [Fact]
        public async Task UpdateProfilePictureUrl_WithValidData_ShouldUpdateProfilePicUrl()
        {
            var expected = "TestUrlEdit.com";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.UpdateProfilePictureUrl(guid, "TestUrlEdit.com");
            Assert.Equal(expected, userGet);
        }

        [Fact]
        public async Task UpdateProfilePictureUrl_WithInValidData_ShouldNoUpdateProfilePicUrlAndReturnNull()
        {
            var expected = "TestUrlEdit.com";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.UpdateProfilePictureUrl("InvalidID", "TestUrlEdit.com");
            Assert.Null(userGet);
        }

        [Fact]
        public async Task GetUserRatingAsync_WithValidData_ShouldReturnUserRating()
        {
            var expected = 4;
            var guid = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();
            var guidOffer = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user1 = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
            };

            var user2 = new ApplicationUser()
            {
                Id = guid2,
                UserName = "TestUser2",
                ProfilePhotoUrl = "TestUrl2.com",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);

            var rating1 = new OfferUserRate()
            {
                OfferId = offer.Id,
                Rate = 5,
                UserId = guid,
            };

            var rating2 = new OfferUserRate()
            {
                OfferId = offer.Id,
                Rate = 3,
                UserId = guid2,
            };

            context.Users.Add(user1);
            context.Users.Add(user2);
            context.OfferUserRates.Add(rating1);
            context.OfferUserRates.Add(rating2);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserRatingAsync(guid);
            Assert.Equal(expected, userGet);
        }

        [Fact]
        public async Task GetUserRatingAsync_WithValidDataButNoOffersOrRatings_ShouldReturnUserRatingZero()
        {
            var expected = 0;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user1 = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
            };

            context.Users.Add(user1);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserRatingAsync(guid);
            Assert.Equal(expected, userGet);
        }

        [Fact]
        public async Task IsOfferFavoritedByUserAsync_WithValidData_ShouldReturnTrue()
        {
            var expected = 4;
            var guid = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();
            var guidOffer = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user1 = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);

            var fav = new UserFavorite()
            {
                OfferId = offer.Id,
                UserId = guid,
            };

            context.Users.Add(user1);
            context.UserFavorites.Add(fav);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.IsOfferFavoritedByUserAsync(offer.Id, guid);
            Assert.True(userGet);
        }

        [Fact]
        public async Task IsOfferFavoritedByUserAsync_WithValidData_ShouldReturnFalse()
        {
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user1 = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);

            context.Users.Add(user1);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.IsOfferFavoritedByUserAsync(offer.Id, guid);
            Assert.False(userGet);
        }

        [Fact]
        public async Task UpdateUserFirstAndLastNameAsync_WithValidData_ShouldUpdateFirstandLastNames()
        {
            var expected = "UpdatedFName UpdatedLName";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
                FirstName = "test",
                LastName = "test",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.UpdateUserFirstAndLastNameAsync(guid, "UpdatedFName", "UpdatedLName");
            Assert.Equal(expected, userGet.FirstName + " " + userGet.LastName);
        }

        [Fact]
        public async Task UpdateUserFirstAndLastNameAsync_WithInValidData_ShouldReturnNull()
        {
            var expected = "UpdatedFName UpdatedLName";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
                FirstName = "test",
                LastName = "test",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.UpdateUserFirstAndLastNameAsync("InvalidID", "UpdatedFName", "UpdatedLName");
            Assert.Null(userGet);
        }

        [Fact]
        public async Task GetUserFirstNameAsync_WithValidData_ShouldReturnFirstName()
        {
            var expected = "Test";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
                FirstName = "Test",
                LastName = "Test2",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserFirstNameAsync(guid);
            Assert.Equal(expected, userGet);
        }

        [Fact]
        public async Task GetUserFirstNameAsync_WithInValidData_ShouldReturnNull()
        {
            var expected = "Test";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
                FirstName = "Test",
                LastName = "Test2",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserFirstNameAsync("InvalidID");
            Assert.Null(userGet);
        }

        [Fact]
        public async Task GetUserLastNameAsync_WithValidData_ShouldReturnLastName()
        {
            var expected = "Test2";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
                FirstName = "Test",
                LastName = "Test2",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserLastNameAsync(guid);
            Assert.Equal(expected, userGet);
        }

        [Fact]
        public async Task GetUserLastNameAsync_WithInValidData_ShouldReturnNull()
        {
            var expected = "Test";
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
                FirstName = "Test",
                LastName = "Test2",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserLastNameAsync("InvalidID");
            Assert.Null(userGet);
        }

        [Fact]
        public async Task GetUserRolesAsync_WithValidData_ShouldReturnUserRoles()
        {
            var expected = new List<string>() { "User", "Administrator" };
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
                FirstName = "Test",
                LastName = "Test2",
            };

            var roleUser = new ApplicationRole()
            {
                Name = "User",
            };

            var roleAdmin = new ApplicationRole()
            {
                Name = "Administrator",
            };

            context.Roles.Add(roleUser);
            context.Roles.Add(roleAdmin);
            context.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = roleUser.Id,
                UserId = guid,
            });
            context.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = roleAdmin.Id,
                UserId = guid,
            });

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserRolesAsync(guid);
            Assert.Equal(expected, userGet);
        }

        [Fact]
        public async Task GetUserRolesAsync_WithValidDataButNoRoles_ShouldReturnUserRolesEmptyList()
        {
            var expected = new List<string>();
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
                FirstName = "Test",
                LastName = "Test2",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserRolesAsync(guid);
            Assert.Equal(expected, userGet);
        }

        [Fact]
        public async Task GetUserRolesAsync_WithInValidData_ShouldReturnNull()
        {
            var expected = new List<string>() { "User", "Administrator" };
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
                FirstName = "Test",
                LastName = "Test2",
            };

            var roleUser = new ApplicationRole()
            {
                Name = "User",
            };

            var roleAdmin = new ApplicationRole()
            {
                Name = "Administrator",
            };

            context.Roles.Add(roleUser);
            context.Roles.Add(roleAdmin);
            context.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = roleUser.Id,
                UserId = guid,
            });
            context.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = roleAdmin.Id,
                UserId = guid,
            });

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetUserRolesAsync("InvalidID");
            Assert.Null(userGet);
        }

        [Fact]
        public async Task GetAdminOffersForApprove_WithValidData_ShouldReturnAdminOffers()
        {
            var expected = 1;
            var guid = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();
            var guidOffer = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user1 = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var createOfferInputModelApproved = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            var offerapproved = await this.offerService.CreateOfferAsync(createOfferInputModelApproved);
            await this.offerService.ApproveOfferAsync(offerapproved.Id);

            context.Users.Add(user1);
            await context.SaveChangesAsync();

            // Assert
            var userGet = await this.userService.GetAdminOffersForApproveCount();
            Assert.Equal(expected, userGet);
        }

        [Fact]
        public async Task GetCurrentUserId_WithValidData_ShouldReturnUserId()
        {
            var guid = Guid.NewGuid().ToString();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, guid),
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(claimsPrincipal);

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();
            moqHttpContextAccessor.Setup(x => x.HttpContext.User.IsInRole(It.IsAny<string>())).Returns(true);
            moqHttpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
                ProfilePhotoUrl = "TestUrl.com",
                FirstName = "Test",
                LastName = "Test2",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var userGet = this.userService.GetCurrentUserId();
            Assert.Equal(guid, userGet);
        }
    }
}
