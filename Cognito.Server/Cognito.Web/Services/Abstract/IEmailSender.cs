using Amazon.SimpleEmail.Model;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Cognito.Web.Services.Abstract
{
    public interface IEmailSender
    {
        Task<Response> SendEmailAsync(SendGridMessage message);

        Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request);

        Task<Response> SendResetPasswordEmailAsync(string email, string token);

        Task<SendEmailResponse> SendInvitationEmailAsync(string email, string token);
    }
}
