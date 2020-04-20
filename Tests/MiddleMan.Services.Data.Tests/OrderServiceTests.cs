namespace MiddleMan.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Data;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Services.Messaging;
    using MiddleMan.Web.ViewModels.InputModels;
    using MiddleMan.Web.ViewModels.InputModels.Offer;
    using MiddleMan.Web.ViewModels.InputModels.Order;
    using Moq;
    using Xunit;

    public class OrderServiceTests
    {
        private IOfferService offerService;
        private IOrderService orderService;

        public OrderServiceTests()
        {
        }

        [Fact]
        public async Task BuyOfferForUser_WithValidData_ShouldBuyOffer()
        {
            var expected = 1;
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqEmailSender = new Mock<IEmailSender>();

            var context = InitializeContext.CreateContextForInMemory();

            var user1 = new ApplicationUser()
            {
                Id = guid1,
                UserName = "Seller",
            };

            var user2 = new ApplicationUser()
            {
                Id = guid2,
                UserName = "Buyer",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid1,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);
            this.orderService = new OrderService(context, this.offerService, moqEmailSender.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user1);
            context.Users.Add(user2);
            await context.SaveChangesAsync();

            // Assert
            await this.orderService.BuyOfferForUser(offer.Id, guid2, new BuyInput()
            {
                Email = "testmail@mail.com",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                OfferId = offer.Id,
            });

            var offerBought = await context.Orders.FirstOrDefaultAsync(x => x.OfferId == offer.Id && x.UserId == guid2);

            Assert.NotNull(offerBought);
        }

        [Fact]
        public async Task BuyOfferForUser_WithInValidData_ShouldNotBuyOffer()
        {
            var expected = 1;
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqEmailSender = new Mock<IEmailSender>();

            var context = InitializeContext.CreateContextForInMemory();

            var user1 = new ApplicationUser()
            {
                Id = guid1,
                UserName = "Seller",
            };

            var user2 = new ApplicationUser()
            {
                Id = guid2,
                UserName = "Buyer",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid1,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);
            this.orderService = new OrderService(context, this.offerService, moqEmailSender.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user1);
            context.Users.Add(user2);
            await context.SaveChangesAsync();

            // Assert
            await this.orderService.BuyOfferForUser("InvalidID", guid2, new BuyInput()
            {
                Email = "testmail@mail.com",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                OfferId = offer.Id,
            });

            var offerBought = await context.Orders.FirstOrDefaultAsync(x => x.OfferId == offer.Id && x.UserId == guid2);

            Assert.Null(offerBought);
        }

        [Fact]
        public async Task GetOrdersHistoryForUser_WithValidData_ShouldReturnOrderHistory()
        {
            var expected = 1;
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqEmailSender = new Mock<IEmailSender>();

            var context = InitializeContext.CreateContextForInMemory();

            var user1 = new ApplicationUser()
            {
                Id = guid1,
                UserName = "Seller",
            };

            var user2 = new ApplicationUser()
            {
                Id = guid2,
                UserName = "Buyer",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid1,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);
            this.orderService = new OrderService(context, this.offerService, moqEmailSender.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(user1);
            context.Users.Add(user2);
            await context.SaveChangesAsync();

            // Assert
            await this.orderService.BuyOfferForUser(offer.Id, guid2, new BuyInput()
            {
                Email = "testmail@mail.com",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                OfferId = offer.Id,
            });

            var offersBought = await this.orderService.GetOrdersHistoryForUser(guid2);
            var offersSold = await this.orderService.GetOrdersHistoryForUser(guid1);

            Assert.NotNull(offersBought);
            Assert.NotNull(offersSold);
        }
    }
}
