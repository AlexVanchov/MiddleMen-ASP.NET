namespace MiddleMan.Web.ViewModels.ViewModels.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MiddleMan.Web.ViewModels.InputModels.Message;

    public class MessagesDetailsViewModel
    {
        public string RecipientId { get; set; }

        public string SenderId { get; set; }

        public string OfferId { get; set; }

        public string OfferTitle { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public SendMessageInputModel InputModel { get; set; }
    }
}
