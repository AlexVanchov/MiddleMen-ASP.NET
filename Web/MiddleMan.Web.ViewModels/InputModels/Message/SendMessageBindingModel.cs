namespace MiddleMan.Web.ViewModels.InputModels.Message
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MiddleMan.Web.ViewModels.ViewModels.Messages;

    public class SendMessageBindingModel
    {
        public SendMessageViewModel ViewModel { get; set; }

        public SendMessageInputModel InputModel { get; set; }
    }
}
