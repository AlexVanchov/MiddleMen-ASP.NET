namespace MiddleMan.Services.Messaging
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using MiddleMan.Data;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;
        private readonly ApplicationDbContext context;

        public EmailSender(
            IOptions<AuthMessageSenderOptions> optionsAccessor,
            IConfiguration configuration,
            IHostingEnvironment env,
            ApplicationDbContext context)
        {
            this.Options = optionsAccessor.Value;
            this.configuration = configuration;
            this.env = env;
            this.context = context;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return this.Execute(this.configuration["SendGridKey"], subject, message, email);
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            //var client = new SendGridClient(apiKey);
            //var from = new EmailAddress("middleman.alex@gmail.com", "MiddleMan");
            //var to = new EmailAddress(email, "Middleman User");
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response = await client.SendEmailAsync(msg);

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("middleman.alex@gmail.com", "middleman"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = await this.PopulateBody(message, subject),
            };
            msg.AddTo(new EmailAddress(email));

            // disable click tracking.
            // see https://sendgrid.com/docs/user_guide/settings/tracking.html
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }

        private async Task<string> PopulateBody(string url, string subject)
        {
            string body = string.Empty;

            var webRoot = env.WebRootPath;

            var file = "";

            if (subject == "Confirm your email")
            {
                file = webRoot + "/EmailTemplates/EmailConfirm.htm";
            }
            else if (subject == "Reset Password")
            {
                file = webRoot + "/EmailTemplates/PasswordReset.htm";
            }
            else if (subject == "Order Details")
            {
                file = webRoot + "/EmailTemplates/OrderDetails.htm";
            }

            using (StreamReader reader = new StreamReader(file))
            {
                body = reader.ReadToEnd();
            }

            if (subject == "Confirm your email" || subject == "Reset Password")
            {
                body = body.Replace("{Url}", url);
            }
            else if (subject == "Order Details")
            {
                var orderId = int.Parse(url);
                var order = await this.context.Orders
                    .FirstOrDefaultAsync(x => x.Id == orderId);

                var offer = await this.context.Offers
                    .FirstOrDefaultAsync(x => x.Id == order.OfferId);

                var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == order.UserId);
                var username = user.UserName;

                body = body.Replace("{purchase_date}", order.CreatedOn.ToString("dd/M/yy H:mm"));
                body = body.Replace("{name}", username);
                body = body.Replace("{receipt_id}", order.OfferName +" - " + order.Id.ToString());
                body = body.Replace("{order_secret_details}", offer.BuyContent);
                body = body.Replace("{order_offer_desc}", offer.Description);
                body = body.Replace("{total}", order.OfferPrice.ToString("f2"));
            }

            return body;
        }
    }
}
