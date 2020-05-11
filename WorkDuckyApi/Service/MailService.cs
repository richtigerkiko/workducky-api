using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using WorkduckyLib.DataObjects.Configuration;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using WorkduckyLib.DataObjects;
using MimeKit.Text;

namespace WorkDuckyAPI.Service
{
    public class MailService
    {

        private readonly SmtpConfiguration configuration;
        private readonly ILogger logger;
        /// <summary>
        /// This service provides basic mail functionality
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        public MailService(IConfiguration config, ILogger logger)
        {
            configuration = new SmtpConfiguration();
            config.GetSection("smtp").Bind(configuration);
            this.logger = logger;
        }

        /// <summary>
        /// This task sends mail messages asynchronous.
        /// Mailserver configuration is extracted from appsettings.json
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMailMessageAsync(MimeMessage message)
        {
            try {
                using (var client = new SmtpClient())
                {
                    client.Connect(configuration.Server, configuration.Port, configuration.UseSSL);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    if (!configuration.Anonymous)
                    {
                        client.Authenticate(configuration.Username, configuration.Password);
                    }
                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical("Send Mail Failed", ex);
            }
        }

        public async Task SendMailActivationMessage(string email, string token)
        {
            var subject = "Workducky here! Please Activate Your Account.";
            var emailText = new EmailText() {
                Title = subject,
                Content1 = "Please click the button below to activate your workducky account.",
                Content2 = "",
                CTAText = "Activate",
                CTAURL = "https://workducky.fadr.de/Account/Activate?token=" + token,
                Greeting = "Hello friend,",
                Sendoff = "Bye!"
            };
            var mailBody = await ApplyMailTemplateAsync(emailText);
            var message = GenerateMailMessage(subject, mailBody, true, new string[] {email});
            await SendMailMessageAsync(message);
        }

        /// <summary>
        /// Generates an email message
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="mailAddresses"></param>
        /// <returns></returns>
        public MimeMessage GenerateMailMessage(string subject, string body, bool ishtml, string[] mailAddresses)
        {
            var message = new MimeMessage()
            {
                Subject = subject
            };

            foreach (var mailAddress in mailAddresses)
            {
                message.To.Add(new MailboxAddress(mailAddress));
            }

            if (ishtml)
            {
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                };
            }
            else
            {
                message.Body = new TextPart(TextFormat.Plain)
                {
                    Text = body
                };
            }
            message.Sender = new MailboxAddress("workducky@fadr.de");
            return message;
        }

        public async Task<string> ApplyMailTemplateAsync(EmailText email)
        {
            var template = await File.ReadAllTextAsync("email.html");
            template = Regex.Replace(template, "##TITLE", email.Title);
            template = Regex.Replace(template, "##HELLO", email.Greeting + ",");
            template = Regex.Replace(template, "##LINKURL", email.CTAURL);
            template = Regex.Replace(template, "##LINKTEXT", email.CTAText);
            template = Regex.Replace(template, "##CONTENT1", email.Content1);
            template = Regex.Replace(template, "##CONTENT2", email.Content2); 
            template = Regex.Replace(template, "##GOODBYE", email.Sendoff);
            return template;
        }

        public async Task SendRegistrationMessage(string sendmailto, string activationToken)
        {
            var subject = "Workducky here! Please Activate Your Account.";
            var emailText = new EmailText() {
                Title = subject,
                Content1 = "Please click the button below to activate your workducky account. Welcome!",
                Content2 = "",
                CTAText = "Activate",
                CTAURL = "https://workducky.fadr.de/Account/Activate?token=" + activationToken,
                Greeting = "Hello friend,",
                Sendoff = "Bye!"
            };
            var mailBody = await ApplyMailTemplateAsync(emailText);
            var message = GenerateMailMessage(subject, mailBody, true, new string[] {sendmailto});
            await SendMailMessageAsync(message);
        }
    }
}