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
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;

    public class CommentServiceTests
    {
        private IUserService userService;
        private IOfferService offerService;
        private ICommentService commentService;

        public CommentServiceTests()
        {
        }

        [Fact]
        public async Task AddReviewToOffer_WithValidData_ShouldAddReview()
        {
            var expected = 1;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
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
            this.commentService = new CommentService(context, this.offerService);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            await this.commentService.AddReviewToOffer(new CreateReviewModel()
            {
                CreatorId = guid,
                Id = offer.Id,
                Rating = "5",
                Review = "Test content",
            });

            var offerComments = await this.commentService.GetOfferCommentsAsync(offer.Id);
            Assert.Equal(expected, offerComments.Count);
            Assert.Equal("Test content", offerComments[0].Description);
            Assert.Equal(guid, offerComments[0].CreatorId);
        }

        [Fact]
        public async Task AddReviewToOffer_WithValidDataButNotDescriptionAndNoOtherRevies_ShouldNotAddReview()
        {
            var expected = 0;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
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
            this.commentService = new CommentService(context, this.offerService);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            await this.commentService.AddReviewToOffer(new CreateReviewModel()
            {
                CreatorId = guid,
                Id = offer.Id,
                Rating = "5",
            });

            var offerComments = await this.commentService.GetOfferCommentsAsync(offer.Id);
            Assert.Equal(expected, offerComments.Count);
        }

        [Fact]
        public async Task AddReviewToOffer_WithInValidData_ShouldReturnEmptyList()
        {
            var expected = new List<Comment>();
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
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
            this.commentService = new CommentService(context, this.offerService);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            await this.commentService.AddReviewToOffer(new CreateReviewModel()
            {
                CreatorId = guid,
                Id = offer.Id,
                Rating = "5",
                Review = "Test content",
            });

            var offerComments = await this.commentService.GetOfferCommentsAsync("InvalidID");
            Assert.Equal(expected, offerComments);
        }

        [Fact]
        public async Task AddReviewToOffer_WithValidDataButAlreadyRate_ShouldChangeRate()
        {
            var expected = 1;
            var guid = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);

            var user = new ApplicationUser()
            {
                Id = guid,
                UserName = "TestUser",
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
            this.commentService = new CommentService(context, this.offerService);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Assert
            await this.commentService.AddReviewToOffer(new CreateReviewModel()
            {
                CreatorId = guid,
                Id = offer.Id,
                Rating = "5",
                Review = "Test content",
            });

            await this.commentService.AddReviewToOffer(new CreateReviewModel()
            {
                CreatorId = guid,
                Id = offer.Id,
                Rating = "1",
            });

            var userRate = await context.OfferUserRates.FirstOrDefaultAsync(x => x.OfferId == offer.Id && x.UserId == guid);
            var offerComments = await this.commentService.GetOfferCommentsAsync(offer.Id);
            Assert.Equal("Test content", offerComments[0].Description);
            Assert.Equal(1, userRate.Rate);
        }
    }
}
