namespace MiddleMan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Data;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels.ViewModels.Messages;

    public class MessagesService : IMessagesService
    {
        private readonly ApplicationDbContext context;
        private readonly IUserService userService;

        public MessagesService(
            ApplicationDbContext context,
            IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task<Message> CreateMessageAsync(string senderId, string recipientId, string offerId, string content)
        {
            if (!await this.context.Offers.AnyAsync(x => x.Id == offerId))
            {
                throw new ArgumentException("OfferNotFound");
            }

            var message = new Message
            {
                SenderId = senderId,
                RecipientId = recipientId,
                OfferId = offerId,
                Content = content,
            };

            await this.context.Messages.AddAsync(message);
            await this.context.SaveChangesAsync();

            return message;
        }

        public async Task<List<MessageViewModel>> GetInboxMessagesAsync()
        {
            var currentUserId = this.userService.GetCurrentUserId();

            var offers = await this.context
                .Offers
                .Where(x => x.Messages.Any(y => y.RecipientId == currentUserId))
                .ToListAsync();

            var inboxMessages = new List<MessageViewModel>();

            foreach (var offer in offers)
            {
                var lastMsg = this.context.Offers
                .Where(x => x.Id == offer.Id).Select(x => x.Messages.FirstOrDefault()).FirstOrDefault();
                var sender = await this.userService.GetUsernameByIdAsync(lastMsg.SenderId);

                inboxMessages.Add(new MessageViewModel()
                {
                    IsRead = lastMsg.IsRead,
                    SenderId = lastMsg.SenderId,
                    RecipientId = lastMsg.RecipientId,
                    OfferId = lastMsg.OfferId,
                    SentOn = lastMsg.CreatedOn,
                    OfferTitle = offer.Name,
                    Sender = sender,
                    LastMessage = lastMsg.Content, // todo cuting listing
                });
            }

            return inboxMessages;
        }

        public async Task<int> GetUnreadMessagesCountAsync(string userId)
        {
            var unreadedMessages = await this.context.Messages.Where(x => x.RecipientId == userId).ToListAsync();

            return unreadedMessages.Count();
        }
    }
}
