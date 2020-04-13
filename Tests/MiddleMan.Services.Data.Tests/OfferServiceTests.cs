namespace MiddleMan.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using MiddleMan.Services.Data;
    using MiddleMan.Services.Interfaces;
    using Moq;
    using Xunit;
    using Microsoft.AspNetCore.Http;
    using MiddleMan.Web.ViewModels.InputModels;
    using MiddleMan.Data.Models;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class OfferServiceTests
    {
        private IOfferService offerService;

        public OfferServiceTests()
        {
        }

        [Fact]
        public async Task CreateOfferAsync_WithValidData_ShouldCreateOffer()
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

            var context = InitializeContext.CreateContextForInMemory();
            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            await this.offerService.CreateOfferAsync(createOfferInputModel);

            // Assert
            var offers = await context.Offers.ToListAsync();
            Assert.Equal(expectedOffersCount, offers.Count);
        }

        [Fact]
        public async Task GetOfferByIdAsync_WithInValidData_ShouldReturnOffer()
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

            var context = InitializeContext.CreateContextForInMemory();
            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var expectedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(expectedOffer.Id);
            expectedOffer.IsApproved = true;

            // Assert
            var offerFromMethod = await this.offerService.GetOfferByIdAsync(expectedOffer.Id);
            Assert.True(expectedOffer.Equals(offerFromMethod));
        }
    }
}
