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

    public class FavoriteServiceTests
    {
        private IFavoriteService favoriteService;

        public FavoriteServiceTests()
        {
        }

        [Fact]
        public async Task AddToFavoritesAsync_WithValidData_ShouldAddOfferToFavorites()
        {
            var expectedOffersCount = 1;
            var guid = Guid.NewGuid().ToString();

            var moqUsersService = new Mock<IUserService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CurrentUserId");

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();

            moqCloudinaryService.Setup(x => x.UploadPhotoAsync(moqIFormFile.Object, "FileName", "Folder"))
                .ReturnsAsync("http://test.com");

            var createOfferInputModel = new CreateOfferModel
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var context = InitializeContext.CreateContextForInMemory();
            var offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);
            this.favoriteService = new FavoriteService(context, offerService);

            var approvedOffer = await offerService.CreateOfferAsync(createOfferInputModel);
            await offerService.ApproveOfferAsync(approvedOffer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var result = await this.favoriteService.AddToFavoritesAsync(approvedOffer.Id, guid);
            var userFav = await context.UserFavorites.FirstOrDefaultAsync(x => x.OfferId == approvedOffer.Id && x.UserId == guid);
            Assert.NotNull(userFav);
            Assert.True(result);
        }

        [Fact]
        public async Task AddToFavoritesAsync_WithInValidDataWithNotFoundOffer_ShouldNotAddOfferToFavorites()
        {
            var expectedOffersCount = 1;
            var guid = Guid.NewGuid().ToString();

            var moqUsersService = new Mock<IUserService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CurrentUserId");

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();

            moqCloudinaryService.Setup(x => x.UploadPhotoAsync(moqIFormFile.Object, "FileName", "Folder"))
                .ReturnsAsync("http://test.com");

            var createOfferInputModel = new CreateOfferModel
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var context = InitializeContext.CreateContextForInMemory();
            var offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);
            this.favoriteService = new FavoriteService(context, offerService);

            var approvedOffer = await offerService.CreateOfferAsync(createOfferInputModel);
            await offerService.ApproveOfferAsync(approvedOffer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var result = await this.favoriteService.AddToFavoritesAsync("InvalidID", guid);
            var userFav = await context.UserFavorites.FirstOrDefaultAsync(x => x.OfferId == "InvalidID" && x.UserId == guid);
            Assert.Null(userFav);
            Assert.False(result);
        }

        [Fact]
        public async Task RemoveFromFavoritesAsync_WithValidData_ShouldAddOfferToFavorites()
        {
            var expectedOffersCount = 1;
            var guid = Guid.NewGuid().ToString();

            var moqUsersService = new Mock<IUserService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CurrentUserId");

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();

            moqCloudinaryService.Setup(x => x.UploadPhotoAsync(moqIFormFile.Object, "FileName", "Folder"))
                .ReturnsAsync("http://test.com");

            var createOfferInputModel = new CreateOfferModel
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var context = InitializeContext.CreateContextForInMemory();
            var offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);
            this.favoriteService = new FavoriteService(context, offerService);

            var approvedOffer = await offerService.CreateOfferAsync(createOfferInputModel);
            await offerService.ApproveOfferAsync(approvedOffer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            await this.favoriteService.AddToFavoritesAsync(approvedOffer.Id, guid);
            var result = await this.favoriteService.RemoveFromFavoritesAsync(approvedOffer.Id, guid);
            var userFav = await context.UserFavorites.FirstOrDefaultAsync(x => x.OfferId == approvedOffer.Id && x.UserId == guid);
            Assert.Null(userFav);
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveFromFavoritesAsync_WithInValidDataWithNotFoundOffer_ShouldNotAddOfferToFavorites()
        {
            var expectedOffersCount = 1;
            var guid = Guid.NewGuid().ToString();

            var moqUsersService = new Mock<IUserService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CurrentUserId");

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqIFormFile = new Mock<IFormFile>();

            moqCloudinaryService.Setup(x => x.UploadPhotoAsync(moqIFormFile.Object, "FileName", "Folder"))
                .ReturnsAsync("http://test.com");

            var createOfferInputModel = new CreateOfferModel
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var context = InitializeContext.CreateContextForInMemory();
            var offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);
            this.favoriteService = new FavoriteService(context, offerService);

            var approvedOffer = await offerService.CreateOfferAsync(createOfferInputModel);
            await offerService.ApproveOfferAsync(approvedOffer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            await this.favoriteService.AddToFavoritesAsync(approvedOffer.Id, guid);
            var result = await this.favoriteService.AddToFavoritesAsync("InvalidID", guid);
            var userFav = await context.UserFavorites.FirstOrDefaultAsync(x => x.OfferId == approvedOffer.Id && x.UserId == guid);
            Assert.NotNull(userFav);
            Assert.False(result);
        }
    }
}
