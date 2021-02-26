using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using KurbSide.Models;
using System.Net;
using System.Net.Mail;

namespace KurbSide.Service
{
    public class SendGridMailer : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = Environment.GetEnvironmentVariable("sendgrid");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("NoReply@kurbsi.de", "No Reply");
            var to = new EmailAddress(email, email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlMessage, htmlMessage);
            //var response = await client.SendEmailAsync(msg);
            return client.SendEmailAsync(msg);
        }
    }
}
