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
    }
}
