using SendGrid;
using SendGrid.Helpers.Mail;
using System.Web;

namespace PoseidonAPI.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;

        public EmailService(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task<bool> sendResetPasswordEmail(string token, string toEmail)
        {
            var client = new SendGridClient(_configuration.GetValue<string>("ApiKeySendGrid"));
            var from = new EmailAddress("bserre15250@gmail.com", "Poseidon Inc");
            var subject = "Forgot password mail";
            var to = new EmailAddress(toEmail, "Example User");
            var urlToken = HttpUtility.UrlEncode(token);
            string link = "https://localhost:7102/ResetPassword/Index/?token=" + urlToken;
            //var callbackUrl = Url.Action("ResetPassword", "Auth", new { code = token });
            var plainTextContent = "reset password link : " + link;
            string htmlContent = @"<strong>reset password link : <a href=" + link + ">click here</a></strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response.IsSuccessStatusCode;
        }
    }
}
