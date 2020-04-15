using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var inboxMessagesViewModels = await this.messagesService.GetInboxMessagesAsync();

            return this.View(inboxMessagesViewModels);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendMessageInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var sendMessageBindingModel =
                    await this.messagesService.GetMessageBindingModelByOfferIdAsync(inputModel.OfferId);
                sendMessageBindingModel.InputModel = inputModel;

                return this.View(sendMessageBindingModel);
            }

            var unreadMessagesCount = await this.messagesService.GetUnreadMessagesCountAsync(inputModel.RecipientId);

            await this.hubContext.Clients.User(inputModel.RecipientId).SendAsync("MessageCount", unreadMessagesCount);

            if (this.ModelState.IsValid)
            {
                var messageViewModel = await this.messagesService.CreateMessageAsync(inputModel.SenderId, inputModel.RecipientId, inputModel.OfferId, inputModel.Content);
                await this.hubContext.Clients.User(inputModel.RecipientId)
                    .SendAsync("SendMessage", messageViewModel);
            }

            return this.RedirectToAction("Details", new { offerId = inputModel.OfferId, senderId = inputModel.RecipientId, recipientId = inputModel.SenderId });
        }

        [Authorize]
        public async Task<IActionResult> Details(MessagesDetailsViewModel viewModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (viewModel.SenderId != userId)
            {
                return this.Redirect("/Messages");
            }

            var messagesViewModel = new List<MessageViewModel>();

            var messages = await this.messagesService.GetMessagesForOfferAsync(viewModel.OfferId, viewModel.SenderId, viewModel.RecipientId);
            messages.OrderBy(date => date.CreatedOn);

            var sideAId = viewModel.SenderId;
            var sideBId = viewModel.RecipientId;

            await this.messagesService.MarkAsSeenForUserAsync(viewModel.SenderId, viewModel.OfferId);

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

            foreach (var message in messages)
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