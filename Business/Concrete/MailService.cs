using Business.Abstract;
using Core.Entities;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;

namespace Business.Concrete
{
	public class MailService : IMailService
	{
		private readonly IConfiguration _configuration;
		
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMailForIncomingMessage(AppUser senderUser, AppUser receiverUser, Message message)
        {
            MimeMessage mimeMessage = new();
            MailboxAddress mailboxAddressFrom = new("Politter", _configuration["Emails:PolitterEmail:mailAdress"]);
            mimeMessage.From.Add(mailboxAddressFrom);
            MailboxAddress mailboxAddressTo = new(receiverUser.FirstName, receiverUser.Email);
            mimeMessage.To.Add(mailboxAddressTo);
            BodyBuilder bodyBuilder = new();
            bodyBuilder.TextBody = $"{senderUser.FirstName} {senderUser.LastName} kullanıcısı size bir mesaj gönderdi. '{message.Content}' " +
                $" Cevap vermek için tıklayın: https://www.politter.com.tr";
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = "Yeni mesajınız var!";
            SmtpClient client = new();
            client.Connect(_configuration["Emails:PolitterEmail:mailClient"], 587, false);
            // Password coming from user secrets
            client.Authenticate(_configuration["Emails:PolitterEmail:mailAdress"], _configuration["Emails:PolitterEmail:mailPassword"]);
            client.Send(mimeMessage);
            client.Disconnect(true);
        }

        public void SendMailForLogging(AppUser user)
        {
            MimeMessage mimeMessage = new();
            MailboxAddress mailboxAddressFrom = new("Politter", _configuration["Emails:PolitterEmail:mailAdress"]);
            mimeMessage.From.Add(mailboxAddressFrom);
            MailboxAddress mailboxAddressTo = new("Politter Admin", _configuration["Emails:AdminEmail:mailAdress"]);
            mimeMessage.To.Add(mailboxAddressTo);
            BodyBuilder bodyBuilder = new();
            bodyBuilder.TextBody = $"{user.FirstName} {user.LastName} - {user.Email} - {user.UserName}";
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            mimeMessage.Subject = "Politter'a Yeni Kullanıcı Kaydoldu!";
            SmtpClient client = new();
            client.Connect(_configuration["Emails:PolitterEmail:mailClient"], 587, false);
            // Password coming from user secrets
            client.Authenticate(_configuration["Emails:PolitterEmail:mailAdress"], _configuration["Emails:PolitterEmail:mailPassword"]);
            client.Send(mimeMessage);
            client.Disconnect(true);
        }

        public void SendMailForResetPassword(AppUser user, string passwordResetTokenLink)
		{
			MimeMessage mimeMessage = new();
			MailboxAddress mailboxAddressFrom = new("Politter", _configuration["Emails:PolitterEmail:mailAdress"]);
			mimeMessage.From.Add(mailboxAddressFrom);
			MailboxAddress mailboxAddressTo = new(user.FirstName, user.Email);
			mimeMessage.To.Add(mailboxAddressTo);
			BodyBuilder bodyBuilder = new();
			bodyBuilder.TextBody = $"Parolanızı sıfırlamak için linke tıklayın:  {passwordResetTokenLink}";
			mimeMessage.Body = bodyBuilder.ToMessageBody();
			mimeMessage.Subject = "Parola Sıfırlama Talebi";
			SmtpClient client = new();
			client.Connect(_configuration["Emails:PolitterEmail:mailClient"], 587, false);
			// Password coming from user secrets
            client.Authenticate(_configuration["Emails:PolitterEmail:mailAdress"], _configuration["Emails:PolitterEmail:mailPassword"]);
			client.Send(mimeMessage);
			client.Disconnect(true);
		}
	}
}
