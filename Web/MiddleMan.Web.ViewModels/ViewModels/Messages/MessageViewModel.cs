﻿namespace MiddleMan.Web.ViewModels.ViewModels.Messages
{
    using System;

    public class MessageViewModel
    {
        public string MessageForId { get; set; }

        public string LastMessage { get; set; }

        public string Content { get; set; }

        public string RecipientId { get; set; }

        public string Sender { get; set; }

        public string SenderId { get; set; }

        public string OfferId { get; set; }

        public string OfferTitle { get; set; }

        public bool IsRead { get; set; }

        public string SentOn { get; set; }
    }
}
