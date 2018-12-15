using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading.Tasks;

namespace ListsAPI.Infrastructure
{
    public interface IEmailMessenger
    {
        Task SendAsync(string toAddress, string subject, string messageBody);
    }

    public class EmailMessenger : IEmailMessenger
    {
        private readonly IConfigurationValueProvider _configurationValueProvider;

        public EmailMessenger(IConfigurationValueProvider configurationValueProvider)
        {
            _configurationValueProvider = configurationValueProvider;
        }

        public async Task SendAsync(string toAddress, string subject, string messageBody)
        {
            var apiKey = _configurationValueProvider.GetValue<string>("SendGridApiKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_configurationValueProvider.GetValue<string>("FromEmailAddress"), "Shine");
            var to = new EmailAddress(toAddress);
            var plainTextContent = messageBody;
            var htmlContent = messageBody;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new WebException("Unable to send email message");
            }
        }
    }
}