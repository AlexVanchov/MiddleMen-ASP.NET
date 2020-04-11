namespace MiddleMan.Services.Interfaces
{
    using MiddleMan.Data.Models;
    using MiddleMan.Web.ViewModels.InputModels.Message;
    using MiddleMan.Web.ViewModels.ViewModels.Messages;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        Task<Message> CreateMessageAsync(string senderId, string recipientId, string offerId, string content);

        Task<int> GetUnreadMessagesCountAsync(string userId);

        Task<List<MessageViewModel>> GetInboxMessagesAsync();

        Task<List<Message>> GetMessagesForOfferAsync(string offerId, string senderId, string recipientId);

        Task<SendMessageBindingModel> GetMessageBindingModelByOfferIdAsync(string offerId);

        Task<SendMessageViewModel> GetMessageViewModelByOfferIdAsync(string offerId);

        Task MarkAsSeenForUserAsync(string userId, string offerId);
    }
}
