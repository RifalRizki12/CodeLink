using API.Contracts;
using System.Net.Mail;
using System.Net.Mime;

namespace API.Utilities.Handler
{
    public class EmailHandler : IEmailHandler
    {
        private string _server;
        private int _port;
        private string _fromEmailAddress;

        public EmailHandler(string server, int port, string fromEmailAddress)
        {
            _server = server;
            _port = port;
            _fromEmailAddress = fromEmailAddress;
        }

        public void Send(string subject, string body, string toEmail)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(_fromEmailAddress),
                Subject = subject,
                IsBodyHtml = true
            };

            // Add the body of the email
            message.Body = body;

            // Attach images as linked resources
            var inlineLogo1 = new LinkedResource("utilities/File/Logo/MIILogo.jpg");
            inlineLogo1.ContentId = "MIILogo";
            var inlineLogo2 = new LinkedResource("utilities/File/Logo/metrodata-logo.jpg");
            inlineLogo2.ContentId = "MetrodataLogo";

            // Create the HTML view with linked resources
            var htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            htmlView.LinkedResources.Add(inlineLogo1);
            htmlView.LinkedResources.Add(inlineLogo2);

            // Add the HTML view to the email
            message.AlternateViews.Add(htmlView);

            // Add the recipient
            message.To.Add(new MailAddress(toEmail));

            using (var smtpClient = new SmtpClient(_server, _port))
            {
                smtpClient.Send(message);
            }
        }
    }
}
