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
        public async Task GetOfferByIdAsync_WithValidData_ShouldReturnOffer()
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

        [Fact]
        public async Task GetOfferByIdAsync_WithInValidData_ShouldReturnNull()
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
            var offerFromMethod = await this.offerService.GetOfferByIdAsync(Guid.NewGuid().ToString());
            Assert.Null(offerFromMethod);
        }

        [Fact]
        public async Task GetOfferAnywayAsync_WithValidData_ShouldReturnOffer()
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
            // the offer is not approved

            // Assert
            var offerFromMethod = await this.offerService.GetOfferAnywayAsync(expectedOffer.Id);
            Assert.True(expectedOffer.Equals(offerFromMethod));
        }

        [Fact]
        public async Task GetAllNotAprovedOffersAsync_WithValidData_ShouldReturnNotAprovedOffers()
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
            // the offer is not approved
            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            // Assert
            var offers = await this.offerService.GetAllNotAprovedOffersAsync();
            Assert.Equal(expectedOffersCount, offers.Count);
        }

        [Fact]
        public async Task GetAllAprovedOffersAsync_WithValidData_ShouldReturnAprovedOffers()
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
            // the offer is not approved
            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            // Assert
            var offers = await this.offerService.GetAllAprovedOffersAsync();
            Assert.Equal(expectedOffersCount, offers.Count);
        }

        [Fact]
        public async Task ApproveOfferAsync_WithValidData_ShouldAprovedOffer()
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            // Assert
            var offers = await context.Offers.Where(x => x.IsApproved == true).ToListAsync();
            Assert.Equal(expectedOffersCount, offers.Count());
        }

        [Fact]
        public async Task ApproveOfferAsync_WithInValidData_ShouldNotAprovedOffer()
        {
            var expectedOffersCount = 0;
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(Guid.NewGuid().ToString());

            // Assert
            var offers = await context.Offers.Where(x => x.IsApproved == true).ToListAsync();
            Assert.Equal(expectedOffersCount, offers.Count());
        }

        [Fact]
        public async Task RemoveOfferAsync_WithValidData_ShouldDeclineOffer()
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.RemoveOfferAsync(approvedOffer.Id);

            // Assert
            var offers = await context.Offers.Where(x => x.IsDeclined == true).ToListAsync();
            Assert.Equal(expectedOffersCount, offers.Count());
        }

        [Fact]
        public async Task RemoveOfferAsync_WithInValidData_ShouldNotDeclineOffer()
        {
            var expectedOffersCount = 0;
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.RemoveOfferAsync(Guid.NewGuid().ToString());

            // Assert
            var offers = await context.Offers.Where(x => x.IsDeclined == true).ToListAsync();
            Assert.Equal(expectedOffersCount, offers.Count());
        }

        [Fact]
        public async Task GetAllDeletedOffersAsync_WithValidData_ShouldDeclineOffer()
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.RemoveOfferAsync(approvedOffer.Id);

            // Assert
            var offers = await this.offerService.GetAllDeletedOffersAsync();
            Assert.Equal(expectedOffersCount, offers.Count());
        }

        [Fact]
        public async Task GetLatestOffers_WithValidData_ShouldReturnNOffers()
        {

            var expectedOffersCount = 9;
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

            for (int i = 0; i < 20; i++)
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
            }

            // Assert
            var offers = await this.offerService.GetLatestOffers(expectedOffersCount);
            Assert.Equal(expectedOffersCount, offers.Count());
        }

        [Fact]
        public async Task GetLatestOffers_WithValidDataButLessOffers_ShouldReturnAllExistingOffers()
        {

            var wantedOffersCount = 9;
            var expectedOffersCount = 5;
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

            for (int i = 0; i < 5; i++)
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
            }

            // Assert
            var offers = await this.offerService.GetLatestOffers(wantedOffersCount);
            Assert.Equal(expectedOffersCount, offers.Count());
        }

        [Fact]
        public async Task FeatureItemAsync_WithValidData_ShouldFeatureOffer()
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.FeatureItemAsync(approvedOffer.Id);

            // Assert
            var offers = await context.Offers.Where(x => x.IsFeatured == true).ToListAsync();
            Assert.Equal(expectedOffersCount, offers.Count());
        }

        [Fact]
        public async Task RemoveFeatureOnItemAsync_WithValidData_ShouldRemoveFeatureOnOffer()
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
            for (int i = 0; i < 9; i++)
            {
                await this.offerService.CreateOfferAsync(createOfferInputModel);
            }

            var featuredOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(featuredOffer.Id);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);
            await this.offerService.FeatureItemAsync(featuredOffer.Id);
            await this.offerService.FeatureItemAsync(approvedOffer.Id);
            await this.offerService.RemoveFeatureOnItemAsync(approvedOffer.Id);

            // Assert
            var offers = await this.offerService.GetFeaturedOffersAsync();
            Assert.Equal(expectedOffersCount, offers.Count());
        }

        [Fact]
        public async Task GetFeaturedOffers_WithValidData_ShouldReturnFeaturedOffers()
        {
            var expectedOffersCount = 3;
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
            for (int i = 0; i < 7; i++)
            {
                var randomOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(randomOffer.Id);
            }

            for (int i = 0; i < 3; i++)
            {
                var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(approvedOffer.Id);
                await this.offerService.FeatureItemAsync(approvedOffer.Id);
            }

            // Assert
            var offers = await this.offerService.GetFeaturedOffersAsync();
            Assert.Equal(expectedOffersCount, offers.Count());
        }

        [Fact]
        public async Task IsOfferRatedAsync_WithValidData_ShouldReturnIsOfferRatedTrue()
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userRate = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 4,
                UserId = guid,
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            context.OfferUserRates.Add(userRate);
            await context.SaveChangesAsync();

            // Assert
            var isRated = await this.offerService.IsOfferRatedAsync(approvedOffer.Id, guid);

            Assert.True(isRated);
        }

        [Fact]
        public async Task IsOfferRatedAsync_WithValidData_ShouldReturnIsOfferRatedFalse()
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userRate = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 4,
                UserId = guid,
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            //context.OfferUserRates.Add(userRate);
            await context.SaveChangesAsync();

            // Assert
            var isRated = await this.offerService.IsOfferRatedAsync(approvedOffer.Id, guid);

            Assert.False(isRated);
        }

        [Fact]
        public async Task IsOfferRatedAsync_WithInValidData_ShouldReturnIsOfferRatedFalse()
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userRate = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 4,
                UserId = guid,
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            //context.OfferUserRates.Add(userRate);
            await context.SaveChangesAsync();

            // Assert
            var isRated1 = await this.offerService.IsOfferRatedAsync("ivalidID", guid);
            var isRated2 = await this.offerService.IsOfferRatedAsync(approvedOffer.Id, "InvalidID");

            Assert.False(isRated1);
            Assert.False(isRated2);
        }

        [Fact]
        public async Task GetRateForOffer_WithValidData_ShouldReturnOfferRate()
        {
            var expectedRate = 4;
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userRate = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 4,
                UserId = guid,
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            context.OfferUserRates.Add(userRate);
            await context.SaveChangesAsync();

            // Assert
            var rate = await this.offerService.GetRateForOffer(approvedOffer.Id, guid);

            Assert.Equal(expectedRate, rate);
        }

        [Fact]
        public async Task GetRateForOffer_WithValidDataButNoData_ShouldReturnOfferRateZero()
        {
            var expectedRate = 0;
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userRate = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 4,
                UserId = guid,
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            //context.OfferUserRates.Add(userRate);
            await context.SaveChangesAsync();

            // Assert
            var rate = await this.offerService.GetRateForOffer(approvedOffer.Id, guid);

            Assert.Equal(expectedRate, rate);
        }

        [Fact]
        public async Task GetAllUserOffersAsync_WithValidData_ShouldReturnUserOffers()
        {
            var expected = 2;
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

            for (int i = 0; i < 2; i++) // offers for user
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
            }

            for (int i = 0; i < 2; i++) // offers for random user
            {
                createOfferInputModel.CreatorId = "RandomID";
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
            }

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var offers = await this.offerService.GetAllUserOffersAsync(guid);

            Assert.Equal(expected, offers.Count);
        }

        [Fact]
        public async Task GetOfferRatingAsync_WithValidData_ShouldReturnOfferAverageRating()
        {
            var expected = 3;
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userRate1 = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 4,
                UserId = "Guid1",
            };

            var userRate2 = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 2,
                UserId = "Guid2",
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            context.OfferUserRates.Add(userRate1);
            context.OfferUserRates.Add(userRate2);
            await context.SaveChangesAsync();

            // Assert
            var offerRate = await this.offerService.GetOfferRatingAsync(approvedOffer.Id);

            Assert.Equal(expected, offerRate);
        }

        [Fact]
        public async Task StartsStringRating_WithValidData_ShouldReturnOfferAverageRatingWithStars()
        {
            var expected = "★★★☆☆";
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userRate1 = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 4,
                UserId = "Guid1",
            };

            var userRate2 = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 2,
                UserId = "Guid2",
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            context.OfferUserRates.Add(userRate1);
            context.OfferUserRates.Add(userRate2);
            await context.SaveChangesAsync();

            // Assert
            var offerRate = await this.offerService.GetOfferRatingAsync(approvedOffer.Id);
            var offerRateInsStars = this.offerService.StartsStringRating(offerRate);

            Assert.Equal(expected, offerRateInsStars);
        }

        [Fact]
        public async Task IsUserCreatorOfOfferAsync_WithValidData_ShouldReturnTrue()
        {
            var expectedRate = 4;
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userRate = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 4,
                UserId = guid,
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            context.OfferUserRates.Add(userRate);
            await context.SaveChangesAsync();

            // Assert
            var isOwner = await this.offerService.IsUserCreatorOfOfferAsync(guid, approvedOffer.Id);

            Assert.True(isOwner);
        }

        [Fact]
        public async Task IsUserCreatorOfOfferAsync_WithValidAndInValidData_ShouldReturnFalse()
        {
            var expectedRate = 4;
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userRate = new OfferUserRate()
            {
                OfferId = approvedOffer.Id,
                Rate = 4,
                UserId = guid,
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            context.OfferUserRates.Add(userRate);
            await context.SaveChangesAsync();

            // Assert
            var isOwner1 = await this.offerService.IsUserCreatorOfOfferAsync(guid, "Invalid");
            var isOwner2 = await this.offerService.IsUserCreatorOfOfferAsync("Invalid", "Invalid");

            Assert.False(isOwner1);
            Assert.False(isOwner2);
        }

        [Fact]
        public async Task GetAllFavoriteUserOffersKeysAsync_WithValidData_ShouldReturnUserFavorites()
        {
            var expected = 1;
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            for (int i = 0; i < 2; i++)
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
            }

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userFavorite = new UserFavorite()
            {
                OfferId = approvedOffer.Id,
                UserId = guid,
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            context.UserFavorites.Add(userFavorite);
            await context.SaveChangesAsync();

            // Assert
            var offers = await this.offerService.GetAllFavoriteUserOffersKeysAsync(guid);

            Assert.Equal(expected, offers.Count);
        }

        [Fact]
        public async Task GetAllFavoriteUserOffersKeysAsync_WithInValidData_ShouldReturnZeroUserFavorites()
        {
            var expected = 0;
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            for (int i = 0; i < 2; i++)
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
            }

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            var comment = new Comment()
            {
                Description = "test comment",
                OfferId = approvedOffer.Id,
                CreatorId = guid,
            };

            var userFavorite = new UserFavorite()
            {
                OfferId = approvedOffer.Id,
                UserId = guid,
            };

            context.Comments.Add(comment);
            context.Users.Add(user);
            context.UserFavorites.Add(userFavorite);
            await context.SaveChangesAsync();

            // Assert
            var offers = await this.offerService.GetAllFavoriteUserOffersKeysAsync("InvalidID");

            Assert.Equal(expected, offers.Count);
        }

        [Fact]
        public async Task GetOfferNameById_WithValidData_ShouldReturnOfferName()
        {
            var expectedOffersCount = "Wow Account";
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.RemoveOfferAsync(approvedOffer.Id);

            // Assert
            var offerName = await this.offerService.GetOfferNameById(approvedOffer.Id);
            Assert.Equal(expectedOffersCount, offerName);
        }

        [Fact]
        public async Task GetOfferNameById_WithInValidData_ShouldReturnNull()
        {
            var expectedOffersCount = "Wow Account";
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.RemoveOfferAsync(approvedOffer.Id);

            // Assert
            var offerName = await this.offerService.GetOfferNameById("InvalidID");
            Assert.Null(offerName);
        }

        [Theory]
        [InlineData("wow")]
        [InlineData("test")]
        [InlineData("warcraft")]
        public async Task GetOffersBySearchAsync_WithValidData_ShouldReturnOfferName(string searchWord)
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
                CategotyName = "World Of Warcraft",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var createOfferInputModelNoMatching = new CreateOfferModel
            {
                Name = "Lol Account",
                CategotyName = "Lol",
                CreatorId = guid,
                Description = "Some no matching Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var context = InitializeContext.CreateContextForInMemory();
            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var approvedOfferInvalid = await this.offerService.CreateOfferAsync(createOfferInputModelNoMatching);
            await this.offerService.ApproveOfferAsync(approvedOfferInvalid.Id);

            // Assert
            var results = await this.offerService.GetOffersBySearchAsync(searchWord);
            Assert.Equal(expectedOffersCount, results.Count);
        }

        [Fact]
        public async Task EditOfferAsync_WithValidData_ShouldEditOffer()
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
                CategotyName = "World Of Warcraft",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var createOfferInputModelNoMatching = new CreateOfferModel
            {
                Name = "Lol Account",
                CategotyName = "Lol",
                CreatorId = guid,
                Description = "Some no matching Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var context = InitializeContext.CreateContextForInMemory();
            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var expectedOfferEdited = await this.offerService.CreateOfferAsync(createOfferInputModelNoMatching);
            await this.offerService.ApproveOfferAsync(expectedOfferEdited.Id);

            expectedOfferEdited.CategoryId = "CategoryIdForLol";

            var edit = new EditOfferModel()
            {
                Name = "Lol Account",
                CategoryName = "Lol",
                Description = "Some no matching Description",
                Price = 10,
                CategoryId = "CategoryIdForLol",
            };
            // Assert
            var results = await this.offerService.EditOfferAsync(edit, approvedOffer.Id);

            Assert.Equal(expectedOfferEdited.Name, results.Name);
            Assert.Equal(expectedOfferEdited.CategoryId, results.CategoryId);
            Assert.Equal(expectedOfferEdited.Description, results.Description);
            Assert.Equal(expectedOfferEdited.Price, results.Price);
        }

        [Fact]
        public async Task EditOfferAsync_WithValidDataButInvalidOfferId_ShouldNotEditOfferAndReturnNull()
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
                CategotyName = "World Of Warcraft",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var createOfferInputModelNoMatching = new CreateOfferModel
            {
                Name = "Lol Account",
                CategotyName = "Lol",
                CreatorId = guid,
                Description = "Some no matching Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var context = InitializeContext.CreateContextForInMemory();
            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            var expectedOfferEdited = await this.offerService.CreateOfferAsync(createOfferInputModelNoMatching);
            await this.offerService.ApproveOfferAsync(expectedOfferEdited.Id);

            expectedOfferEdited.CategoryId = "CategoryIdForLol";

            var edit = new EditOfferModel()
            {
                Name = "Lol Account",
                CategoryName = "Lol",
                Description = "Some no matching Description",
                Price = 10,
                CategoryId = "CategoryIdForLol",
            };
            // Assert
            var results = await this.offerService.EditOfferAsync(edit, "InvalidID");

            Assert.Null(results);
        }

        [Fact]
        public async Task ActivateOfferAsync_WithValidData_ShouldNotBeDeclinedOffer()
        {
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            // await this.offerService.ApproveOfferAsync(approvedOffer.Id);
            await this.offerService.RemoveOfferAsync(approvedOffer.Id);

            // Assert
            var offer = await this.offerService.ActivateOfferAsync(approvedOffer.Id);
            Assert.False(offer.IsDeclined);
        }

        [Fact]
        public async Task ActivateOfferAsync_WithInValidData_ShouldReturnNull()
        {
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            // await this.offerService.ApproveOfferAsync(approvedOffer.Id);
            await this.offerService.RemoveOfferAsync(approvedOffer.Id);

            // Assert
            var offer = await this.offerService.ActivateOfferAsync("InvalidID");
            Assert.Null(offer);
        }

        [Fact]
        public async Task GetAllActiveUserOffersAsync_WithValidData_ShouldReturnUserOffers()
        {
            var expected = 4;
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

            for (int i = 0; i < 4; i++) // offers for user active
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
            }

            for (int i = 0; i < 2; i++) // offers for user deactivated
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
                await this.offerService.DeleteOfferAsUserAsync(offer.Id);
            }

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var offers = await this.offerService.GetAllActiveUserOffersAsync(guid);

            Assert.Equal(expected, offers.Count);
        }

        [Fact]
        public async Task GetAllDeactivatedUserOffersAsync_WithValidData_ShouldReturnUserOffers()
        {
            var expected = 2;
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

            for (int i = 0; i < 4; i++) // offers for user active
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
            }

            for (int i = 0; i < 2; i++) // offers for user deactivated
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
                await this.offerService.DeleteOfferAsUserAsync(offer.Id);
            }

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            var offers = await this.offerService.GetAllDeactivatedUserOffersAsync(guid);

            Assert.Equal(expected, offers.Count);
        }

        [Fact]
        public async Task GetAllCategoryOffersAsync_WithValidData_ShouldReturnCategoryOffers()
        {
            var expected = 4;
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
                CategotyName = "wowcategoryId",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var createOfferInputModelOtherCategory = new CreateOfferModel
            {
                Name = "Wow Account",
                CategotyName = "WowInvalid",
                CreatorId = guid,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            var context = InitializeContext.CreateContextForInMemory();
            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            for (int i = 0; i < 4; i++) // offers for user active
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
                await this.offerService.ApproveOfferAsync(offer.Id);
            }

            for (int i = 0; i < 2; i++) // offers for user deactivated
            {
                var offer = await this.offerService.CreateOfferAsync(createOfferInputModelOtherCategory);
                await this.offerService.ApproveOfferAsync(offer.Id);
            }

            var category = new Category()
            {
                Id = "wowcategoryId",
                Name = "Wow",
            };

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
            };

            context.Users.Add(user);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // Assert
            var offers = await this.offerService.GetAllCategoryOffersAsync("wowcategoryId");

            Assert.Equal(expected, offers.Count());
        }

        [Fact]
        public async Task ActivateOfferAsUserAsync_WithValidData_ShouldNotBeDeclinedOffer()
        {
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);
            await this.offerService.RemoveOfferAsync(approvedOffer.Id);

            // Assert
            var offer = await this.offerService.ActivateOfferAsUserAsync(approvedOffer.Id);
            Assert.False(offer.IsRemovedByUser);
        }

        [Fact]
        public async Task ActivateOfferAsUserAsync_WithInValidData_ShouldReturnNull()
        {
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);
            await this.offerService.RemoveOfferAsync(approvedOffer.Id);

            // Assert
            var offer = await this.offerService.ActivateOfferAsUserAsync("InvalidID");
            Assert.Null(offer);
        }

        [Fact]
        public async Task DeleteOfferAsUserAsync_WithValidData_ShouldNotBeDeclinedOffer()
        {
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);

            // Assert
            var offer = await this.offerService.DeleteOfferAsUserAsync(approvedOffer.Id);
            Assert.True(offer.IsRemovedByUser);
        }

        [Fact]
        public async Task DeleteOfferAsUserAsync_WithInValidData_ShouldReturnNull()
        {
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

            var approvedOffer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(approvedOffer.Id);
            await this.offerService.ActivateOfferAsUserAsync(approvedOffer.Id);

            // Assert
            var offer = await this.offerService.DeleteOfferAsUserAsync("InvalidID");
            Assert.Null(offer);
        }

        [Fact]
        public async Task IsOfferExisting_WithValidData_ShouldReturnTrue()
        {
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

            // Assert
            var isExists = await this.offerService.IsOfferExisting(expectedOffer.Id);
            Assert.True(isExists);
        }

        [Fact]
        public async Task IsOfferExisting_WithValidData_ShouldReturnFalse()
        {
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

            // Assert
            var isExists = await this.offerService.IsOfferExisting("InvalidID");
            Assert.False(isExists);
        }
    }
}