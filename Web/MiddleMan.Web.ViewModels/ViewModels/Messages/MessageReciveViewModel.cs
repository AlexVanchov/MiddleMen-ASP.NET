using System;
using System.Collections.Generic;
using System.Text;

namespace MiddleMan.Web.ViewModels.ViewModels.Messages
{
    public class MessageReciveViewModel
    {
        public UserMessagesViewModel SideA { get; set; }

        public UserMessagesViewModel SideB { get; set; }

        public string RecipientId { get; set; }

        public string SenderId { get; set; }

        public string OfferId { get; set; }

        public string MessageContent { get; set; }

        public string CurrentUserId { get; set; }

        public string SentOn { get; set; }
    }
}
