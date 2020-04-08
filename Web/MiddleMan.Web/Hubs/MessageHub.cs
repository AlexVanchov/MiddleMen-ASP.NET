using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MiddleMan.Services.Interfaces;
using MiddleMan.Web.ViewModels.InputModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleMan.Web.Hubs
{

    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessagesService messagesService;

        public MessageHub(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public async Task UserMessagesCount(string userId)
        {
            var messagesCount = await this.messagesService.GetUnreadMessagesCountAsync(userId);

            await this.Clients.User(userId).SendAsync("MessageCount", messagesCount);
        }

        public async Task SendMessage(SendMessageInputModel inputModel)
        {
            var messageViewModel = await this.messagesService.CreateMessageAsync(inputModel.SenderId, inputModel.RecipientId, inputModel.OfferId, inputModel.Content);

            await this.Clients.Users(inputModel.RecipientId)
                .SendAsync("SendMessage", messageViewModel);
        }
    }
}
