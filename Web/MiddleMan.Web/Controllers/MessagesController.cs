using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MiddleMan.Services.Interfaces;
using MiddleMan.Web.Hubs;

namespace MiddleMan.Web.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessagesService messagesService;
        private readonly IOfferService offerService;
        private readonly IHubContext<MessageHub> hubContext;

        public MessagesController(
            IMessagesService messagesService,
            IOfferService offerService,
            IHubContext<MessageHub> hubContext)
        {
            this.messagesService = messagesService;
            this.offerService = offerService;
            this.hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var inboxMessagesViewModels = await this.messagesService.GetInboxMessagesAsync();

            return this.View(inboxMessagesViewModels);
        }
    }
}