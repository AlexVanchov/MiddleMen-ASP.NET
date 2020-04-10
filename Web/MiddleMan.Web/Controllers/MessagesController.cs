using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MiddleMan.Services.Interfaces;
using MiddleMan.Web.Hubs;
using MiddleMan.Web.ViewModels.InputModels.Message;
using MiddleMan.Web.ViewModels.ViewModels.Messages;

namespace MiddleMan.Web.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessagesService messagesService;
        private readonly IOfferService offerService;
        private readonly IUserService userService;
        private readonly IHubContext<MessageHub> hubContext;

        public MessagesController(
            IMessagesService messagesService,
            IOfferService offerService,
            IUserService userService,
            IHubContext<MessageHub> hubContext)
        {
            this.messagesService = messagesService;
            this.offerService = offerService;
            this.userService = userService;
            this.hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var inboxMessagesViewModels = await this.messagesService.GetInboxMessagesAsync();

            return this.View(inboxMessagesViewModels);
        }

        [Authorize]
        public async Task<IActionResult> Details(MessagesDetailsViewModel viewModel)
        {
            var messagesViewModel = new List<MessageViewModel>();

            var messagesSideA = await this.messagesService.GetMessagesForOfferAsync(viewModel.OfferId, viewModel.SenderId, viewModel.RecipientId);
            var messagesSideB = await this.messagesService.GetMessagesForOfferAsync(viewModel.OfferId, viewModel.RecipientId, viewModel.SenderId);

            var sideAId = viewModel.SenderId;
            var sideBId = viewModel.RecipientId;

            viewModel.SideA = new UserMessagesViewModel()
            {
                ProfilePicUrl = await this.userService.GetUserProfilePictureUrlAsync(sideAId),
                Username = await this.userService.GetUsernameByIdAsync(sideAId),
            };

            viewModel.SideB = new UserMessagesViewModel()
            {
                ProfilePicUrl = await this.userService.GetUserProfilePictureUrlAsync(sideBId),
                Username = await this.userService.GetUsernameByIdAsync(sideBId),
            };

            foreach (var message in messagesSideA)
            {
                messagesViewModel.Add(new MessageViewModel()
                {
                    Content = message.Content,
                    IsRead = message.IsRead,
                    RecipientId = message.RecipientId,
                    SenderId = message.SenderId,
                    SentOn = message.CreatedOn.ToString("MM/dd hh:mm tt"),
                    OfferId = message.OfferId,
                    Sender = await this.userService.GetUsernameByIdAsync(message.SenderId),
                });
            }

            foreach (var message in messagesSideB)
            {
                messagesViewModel.Add(new MessageViewModel()
                {
                    Content = message.Content,
                    IsRead = message.IsRead,
                    RecipientId = message.RecipientId,
                    SenderId = message.SenderId,
                    SentOn = message.CreatedOn.ToString("MM/dd hh:mm tt"),
                    OfferId = message.OfferId,
                    Sender = await this.userService.GetUsernameByIdAsync(message.SenderId),
                });
            }

            viewModel.Messages = messagesViewModel;
            viewModel.OfferTitle = await this.offerService.GetOfferNameById(viewModel.OfferId);
            viewModel.InputModel = new SendMessageInputModel()
            {
                SenderId = viewModel.SenderId,
                OfferId = viewModel.OfferId,
                RecipientId = viewModel.RecipientId,
            };

            return this.View(viewModel);
        }
    }
}