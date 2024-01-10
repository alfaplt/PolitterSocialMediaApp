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
			MailboxAddress mailboxAddressFrom = new("XYZ Social App", "xyzsocialapp@polattest.site");
			mimeMessage.From.Add(mailboxAddressFrom);
			MailboxAddress mailboxAddressTo = new(user.FirstName, "***REMOVED***");
			mimeMessage.To.Add(mailboxAddressTo);
			BodyBuilder bodyBuilder = new();
			bodyBuilder.TextBody = $"Parolanızı sıfırlamak için linke tıklayın:  {passwordResetTokenLink}";
			mimeMessage.Body = bodyBuilder.ToMessageBody();
			mimeMessage.Subject = "Parola Sıfırlama Talebi";
			SmtpClient client = new();
			client.Connect("mail.polattest.site", 587, false);
			// Password coming from user secrets
            client.Authenticate("xyzsocialapp@polattest.site", _configuration["Passwords:mailPassword"]);
			client.Send(mimeMessage);
			client.Disconnect(true);
		}
	}
}
