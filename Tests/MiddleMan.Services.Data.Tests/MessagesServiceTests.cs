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

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Data;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels.Administration.Dashboard.InputModels;
    using MiddleMan.Web.ViewModels.InputModels;
    using MiddleMan.Web.ViewModels.InputModels.Offer;
    using MiddleMan.Web.ViewModels.ViewModels;
    using MiddleMan.Web.ViewModels.ViewModels.Offer;
    using Moq;
    using Xunit;

    public class MessagesServiceTests
    {
        private ICategoryService categoryService;
        private IUserService userService;
        private IOfferService offerService;
        private ICommentService commentService;
        private IMessagesService messagesService;

        public MessagesServiceTests()
        {
        }

        [Fact]
        public async Task CreateMessageAsync_WithValidData_ShouldCreateNewMessage()
        {
            var expected = 1;
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqOfferService = new Mock<IOfferService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.messagesService = new MessagesService(context, this.userService, moqOfferService.Object);

            var sender = new ApplicationUser()
            {
                Id = guid1,
                UserName = "TestUser",
            };

            var reciver = new ApplicationUser()
            {
                Id = guid2,
                UserName = "TestUser",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid2,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(sender);
            context.Users.Add(reciver);
            await context.SaveChangesAsync();

            // Assert

            var messageCreated = await this.messagesService.CreateMessageAsync(guid1, guid2, offer.Id, "TestMessage");
            var messages = await context.Messages.FirstOrDefaultAsync(x => x.Id == messageCreated.Id);
            Assert.NotNull(messages);
        }

        [Fact]
        public async Task GetUnreadMessagesCountAsync_WithValidData_ShouldReturnUnreadCount()
        {
            var expected = 1;
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqOfferService = new Mock<IOfferService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.messagesService = new MessagesService(context, this.userService, moqOfferService.Object);

            var sender = new ApplicationUser()
            {
                Id = guid1,
                UserName = "TestUser",
            };

            var reciver = new ApplicationUser()
            {
                Id = guid2,
                UserName = "TestUser",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid2,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(sender);
            context.Users.Add(reciver);
            await context.SaveChangesAsync();

            // Assert

            var messageCreated = await this.messagesService.CreateMessageAsync(guid1, guid2, offer.Id, "TestMessage");
            var unreadMessages = await this.messagesService.GetUnreadMessagesCountAsync(guid2);
            Assert.Equal(expected, unreadMessages);
        }

        [Fact]
        public async Task GetInboxMessagesAsync_WithValidData_ShouldReturnInboxMessages()
        {
            var expected = 1;
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, guid2),
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

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqOfferService = new Mock<IOfferService>();
            var moqUsersService = new Mock<IUserService>();
            moqUsersService.Setup(x => x.GetCurrentUserId())
                .Returns("CurrentUserId");

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.messagesService = new MessagesService(context, this.userService, moqOfferService.Object);

            var sender = new ApplicationUser()
            {
                Id = guid1,
                UserName = "TestUser",
            };

            var reciver = new ApplicationUser()
            {
                Id = guid2,
                UserName = "TestUser2",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid2,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(sender);
            context.Users.Add(reciver);
            await context.SaveChangesAsync();

            // Assert

            await this.messagesService.CreateMessageAsync(guid1, guid2, offer.Id, "TestMessage");
            await this.messagesService.CreateMessageAsync(guid1, guid2, offer.Id, "TestMessage2");
            var InboxMessages = await this.messagesService.GetInboxMessagesAsync();
            Assert.Equal(expected, InboxMessages.Count);
        }

        [Fact]
        public async Task GetMessagesForOfferAsync_WithValidData_ShouldReturnMessages()
        {
            var expected = 2;
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqOfferService = new Mock<IOfferService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.messagesService = new MessagesService(context, this.userService, moqOfferService.Object);

            var sender = new ApplicationUser()
            {
                Id = guid1,
                UserName = "TestUser",
            };

            var reciver = new ApplicationUser()
            {
                Id = guid2,
                UserName = "TestUser",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid2,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(sender);
            context.Users.Add(reciver);
            await context.SaveChangesAsync();

            // Assert

            await this.messagesService.CreateMessageAsync(guid1, guid2, offer.Id, "TestMessage");
            await this.messagesService.CreateMessageAsync(guid1, guid2, offer.Id, "TestMessage2");
            var unreadMessages = await this.messagesService.GetMessagesForOfferAsync(offer.Id, guid1, guid2);
            Assert.Equal(expected, unreadMessages.Count);
        }

        [Fact]
        public async Task MarkAsSeenForUserAsync_WithValidData_ShouldMarkReciverMessagesAsSeen()
        {
            var expected = 0;
            var guid1 = Guid.NewGuid().ToString();
            var guid2 = Guid.NewGuid().ToString();

            var moqHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var moqCategoriesService = new Mock<ICategoryService>();
            var moqCloudinaryService = new Mock<ICloudinaryService>();
            var moqOfferService = new Mock<IOfferService>();

            var context = InitializeContext.CreateContextForInMemory();
            this.userService = new UserService(context, moqHttpContextAccessor.Object);
            this.messagesService = new MessagesService(context, this.userService, moqOfferService.Object);

            var sender = new ApplicationUser()
            {
                Id = guid1,
                UserName = "TestUser",
            };

            var reciver = new ApplicationUser()
            {
                Id = guid2,
                UserName = "TestUser",
            };

            var createOfferInputModel = new CreateOfferModel()
            {
                Name = "Wow Account",
                CategotyName = "Wow",
                CreatorId = guid2,
                Description = "Some Test Description",
                Price = 10.00,
                PicUrl = "link",
            };

            this.offerService = new OfferService(context, moqCategoriesService.Object, moqCloudinaryService.Object);

            var offer = await this.offerService.CreateOfferAsync(createOfferInputModel);
            await this.offerService.ApproveOfferAsync(offer.Id);

            context.Users.Add(sender);
            context.Users.Add(reciver);
            await context.SaveChangesAsync();

            // Assert

            await this.messagesService.CreateMessageAsync(guid1, guid2, offer.Id, "TestMessage");
            await this.messagesService.CreateMessageAsync(guid1, guid2, offer.Id, "TestMessage2");

            await this.messagesService.MarkAsSeenForUserAsync(guid2, offer.Id);
            var count = await context.Messages.Where(x => x.IsRead == false).ToListAsync();
            Assert.Equal(expected, count.Count);
        }
    }
}
