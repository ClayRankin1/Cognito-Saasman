using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Cognito.Shared.Options;
using Cognito.Web.Services.Abstract;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AmazonEmailContent = Amazon.SimpleEmail.Model.Content;
using AmazonEmailMessage = Amazon.SimpleEmail.Model.Message;

namespace Cognito.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<AppOptions> _appOptions;
        private readonly IOptions<AwsOptions> _awsOptions;
        private readonly IOptions<SendGridOptions> _sendGridOptions;
        private readonly ISendGridClient _client;
        private readonly IAmazonSimpleEmailService _amazonSimpleEmailService;

        public EmailSender(
            IOptions<AppOptions> appOptions,
            IOptions<AwsOptions> awsOptions,
            IOptions<SendGridOptions> sendGridOptions,
            ISendGridClient client,
            IAmazonSimpleEmailService amazonSimpleEmailService)
        {
            _appOptions = appOptions;
            _awsOptions = awsOptions;
            _sendGridOptions = sendGridOptions;
            _client = client;
            _amazonSimpleEmailService = amazonSimpleEmailService;
        }

        public Task<Response> SendEmailAsync(SendGridMessage message)
        {
            message.SetFrom(_sendGridOptions.Value.WebmasterEmail, _sendGridOptions.Value.WebmasterName);
            return _client.SendEmailAsync(message);
        }

        public Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request)
        {
            request.Source = _awsOptions.Value.WebmasterEmail;
            return _amazonSimpleEmailService.SendEmailAsync(request);
        }

        public Task<Response> SendResetPasswordEmailAsync(string email, string token)
        {
            var message = new SendGridMessage();
            message.AddTo(email);
            message.SetTemplateId(_sendGridOptions.Value.PasswordResetTemplateId);
            message.SetTemplateData(new
            {
                access_token = HttpUtility.UrlEncode(token)
            });

            return SendEmailAsync(message);
        }

        public Task<SendEmailResponse> SendInvitationEmailAsync(string email, string token)
        {
            var encodedEmail = HttpUtility.UrlEncode(email);
            var encodedToken = HttpUtility.UrlEncode(token);
            var link = $"{_appOptions.Value.BaseUrl}password-confirm?email={encodedEmail}&token={encodedToken}";
            var body = new Body
            {
                Html = new AmazonEmailContent
                {
                    Charset = Encoding.UTF8.WebName,
                    // todo: Add workspace...
                    Data = $"You have been invited to join the Cognito workspace. Click <a href=\"{link}\">here</a> to complete your account setup."
                }
            };

            var emailRequest  = new SendEmailRequest
            {
                Destination = new Destination
                {
                    ToAddresses = new List<string> { email }
                },
                Message = new AmazonEmailMessage
                {
                    Subject = new AmazonEmailContent("Welcome to Cognito!"),
                    Body = body
                }
            };

            return SendEmailAsync(emailRequest);
        }
    }
}
