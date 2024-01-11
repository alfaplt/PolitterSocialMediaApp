using Business.Abstract;
using Core.Entities;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace Business.Concrete
{
	public class MailService : IMailService
	{
		private readonly IConfiguration _configuration;
		
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    
        public void SendMailForResetPassword(AppUser user, string passwordResetTokenLink)
		{
			MimeMessage mimeMessage = new();
			MailboxAddress mailboxAddressFrom = new("Politter", _configuration["Email:mailAdress"]);
			mimeMessage.From.Add(mailboxAddressFrom);
			MailboxAddress mailboxAddressTo = new(user.FirstName, user.Email);
			mimeMessage.To.Add(mailboxAddressTo);
			BodyBuilder bodyBuilder = new();
			bodyBuilder.TextBody = $"Parolanızı sıfırlamak için linke tıklayın:  {passwordResetTokenLink}";
			mimeMessage.Body = bodyBuilder.ToMessageBody();
			mimeMessage.Subject = "Parola Sıfırlama Talebi";
			SmtpClient client = new();
			client.Connect(_configuration["Email:mailClient"], 587, false);
			// Password coming from user secrets
            client.Authenticate(_configuration["Email:mailAdress"], _configuration["Email:mailPassword"]);
			client.Send(mimeMessage);
			client.Disconnect(true);
		}
	}
}
