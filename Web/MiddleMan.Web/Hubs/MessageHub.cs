using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MiddleMan.Services.Interfaces;
using MiddleMan.Web.ViewModels.InputModels.Message;
using MiddleMan.Web.ViewModels.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiddleMan.Web.Hubs
{

    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessagesService messagesService;
        private readonly IUserService userService;

        public MessageHub(
            IMessagesService messagesService,
            IUserService userService)
        {
            this.messagesService = messagesService;
            this.userService = userService;
        }

        public async Task UserMessagesCount(string userId)
        {
            var messagesCount = await this.messagesService.GetUnreadMessagesCountAsync(userId);

            await this.Clients.User(userId).SendAsync("MessageCount", messagesCount);
        }

        public async Task SendMessage(string offerId, string senderId, string recipientId, string content)
        {
            var userId = this.userService.GetCurrentUserId();
            if (userId != senderId)
            {
                throw new ArgumentException("Invalid Data");
            }

            var message = await this.messagesService.CreateMessageAsync(senderId, recipientId, offerId, content);

            var viewModel = await this.GetMessageViewModel(offerId, senderId, recipientId);

            viewModel.MessageContent = message.Content;
            viewModel.SentOn = message.CreatedOn.ToString("MM/dd hh:mm tt");

            await this.Clients.Users(recipientId, senderId)
                .SendAsync("NewMessage", viewModel);
        }

        private async Task<MessageReciveViewModel> GetMessageViewModel(string offerId, string senderId, string recipientId)
        {
            var userId = this.userService.GetCurrentUserId();
            var viewModel = new MessageReciveViewModel()
            {
                SenderId = senderId,
                RecipientId = recipientId,
                OfferId = offerId,
                CurrentUserId = userId,
            };

            var sideAId = senderId;
            var sideBId = recipientId;

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

            return viewModel;
        }
    }
}
