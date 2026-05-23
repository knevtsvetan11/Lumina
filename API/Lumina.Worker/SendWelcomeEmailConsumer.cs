using Lumina.Common.Shared;
using Lumina.Worker.Configuration;
using Lumina.Worker.Data;
using Lumina.Worker.Data.Models;
using MailKit.Net.Smtp;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Models;
using MimeKit;

namespace Worker.Consumers;

public class SendWelcomeEmailConsumer
    : IConsumer<UserRegisteredEvent>
{
    private readonly WorkerDbContext _customDb;
    private readonly MailtrapSettings _mailSettings;

    public SendWelcomeEmailConsumer(WorkerDbContext customDb, IOptions<MailtrapSettings> mailSettings)
    {
        _customDb = customDb;
        _mailSettings = mailSettings.Value;
    }


    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {

        var userData = context.Message;

        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Cinema App Admin", "no-reply@cinema.com"));
            email.To.Add(new MailboxAddress(userData.Username, userData.Email));
            email.Subject = "Welcome to the Cinema App! 🍿";

            var htmlBody = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px;'>
                    <h2>Hello, {userData.Username}!</h2>
                    <p>Thank you for joining our Cinema platform. Your account is ready.</p>
                    <hr/>
                    <h3>🔥 Trending This Week:</h3>
                    <ul>
                        <li>Inception</li>
                        <li>Interstellar</li>
                        <li>The Dark Knight</li>
                    </ul>
                    <p>Log in now and start building your watchlist!</p>
                </div>";

            email.Body = new TextPart("html") { Text = htmlBody };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, false);
                await client.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);

                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }


            var existEmailLog = await _customDb.EmailLogs.FirstOrDefaultAsync(x => x.RecipientEmail == userData.Email);
            if (existEmailLog is null)
            {
                _customDb.EmailLogs.Add(new EmailLog
                {
                    RecipientEmail = userData.Email,
                    SentAt = DateTime.UtcNow
                });

                await _customDb.SaveChangesAsync();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to process email for {userData.Email}: {ex.Message}");
            throw;
        }
    }
}