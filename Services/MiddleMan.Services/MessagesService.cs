namespace MiddleMan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MiddleMan.Common;
    using MiddleMan.Data;
    using MiddleMan.Data.Models;
    using MiddleMan.Services.Interfaces;
    using MiddleMan.Web.ViewModels.InputModels.Message;
    using MiddleMan.Web.ViewModels.ViewModels.Messages;

    public class MessagesService : IMessagesService
    {
        private readonly ApplicationDbContext context;
        private readonly IUserService userService;
        private readonly IOfferService offerService;

        public MessagesService(
            ApplicationDbContext context,
            IUserService userService,
            IOfferService offerService)
        {
            this.context = context;
            this.userService = userService;
            this.offerService = offerService;
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
                    SentOn = lastMsg.CreatedOn.ToString("MM/dd hh:mm tt"),
                    OfferTitle = offer.Name,
                    Sender = sender,
                    LastMessage = lastMsg.Content, // todo cuting listing
                });
            }

            return inboxMessages;
        }

        public async Task<List<Message>> GetMessagesForOfferAsync(string offerId, string senderId, string recipientId)
        {
            var offer = await this.context.Offers.FirstOrDefaultAsync(x => x.Id == offerId);
            var offerOwnerId = offer.CreatorId;

            return await this.context.Messages
                .Where(x => x.OfferId == offerId && (x.SenderId == senderId || x.SenderId == offerOwnerId) && (x.RecipientId == offerOwnerId || x.RecipientId == senderId))
                .ToListAsync();
        }

        public async Task<int> GetUnreadMessagesCountAsync(string userId)
        {
            var unreadedMessages = await this.context.Messages.Where(x => x.RecipientId == userId).ToListAsync();

            return unreadedMessages.Count();
        }

        public async Task<SendMessageBindingModel> GetMessageBindingModelByOfferIdAsync(string offerId)
        {
            if (!await this.context.Offers.AnyAsync(x => x.Id == offerId))
            {
                throw new ArgumentException("Invalid ID");
            }

            var sendMessageViewModel = await this.GetMessageViewModelByOfferIdAsync(offerId);
            var sendMessageBindingModel = new SendMessageBindingModel
            {
                ViewModel = sendMessageViewModel,
            };

            return sendMessageBindingModel;
        }

        public async Task<SendMessageViewModel> GetMessageViewModelByOfferIdAsync(string offerId)
        {
            if (!await this.context.Offers.AnyAsync(x => x.Id == offerId))
            {
                throw new ArgumentException("InvalidID");
            }

            var offer = await this.offerService.GetOfferByIdAsync(offerId);

            var sendMessageViewModel = new SendMessageViewModel()
            {
                OfferName = offer.Name,
                SenderId = this.userService.GetCurrentUserId(),
                RecipientId = offer.CreatorId,
                OfferPrice = offer.Price,
                OfferId = offer.Id,
            };
            sendMessageViewModel.SenderId = this.userService.GetCurrentUserId();

            return sendMessageViewModel;
        }
    }
}
