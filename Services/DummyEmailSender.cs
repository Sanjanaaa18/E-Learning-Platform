using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System;

namespace ELearningPlatform.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Just log to console (no real sending)
            Console.WriteLine($"[EmailSender] To: {email}, Subject: {subject}");
            return Task.CompletedTask;
        }
    }
}
