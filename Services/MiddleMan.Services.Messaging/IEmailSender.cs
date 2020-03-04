namespace MiddleMan.Services.Messaging
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        //
        // Summary:
        //     This API supports the ASP.NET Core Identity default UI infrastructure and is
        //     not intended to be used directly from your code. This API may change or be removed
        //     in future releases.
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
