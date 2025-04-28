using MimeKit;

using MailKit.Net.Smtp;

using System.Threading.Tasks;



public class EmailService

{

    private readonly string _smtpHost = "smtp.gmail.com";  // Change this to your SMTP host

    private readonly int _smtpPort = 587;  // Typically 587 for TLS

    private readonly string _smtpUser = "ruchibalapada2816@gmail.com";  // Your email address

    private readonly string _smtpPassword = "kksf cfgm vlgk ekar";  // Your email password

    public async Task SendEmailAsync(string recipientEmail, string subject, string body)

    {

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("Stock Portfolio Tracker", _smtpUser));

        message.To.Add(new MailboxAddress(recipientEmail));

        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = body };

        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())

        {

            await client.ConnectAsync(_smtpHost, _smtpPort, false);

            await client.AuthenticateAsync(_smtpUser, _smtpPassword);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);

        }

    }

}

 