using Business.Abstract;
using Core.Entities;
using MailKit.Net.Smtp;
using MimeKit;

namespace Business.Concrete
{
	public class MailService : IMailService
	{
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
			client.Authenticate("xyzsocialapp@polattest.site", "***REMOVED***");
			client.Send(mimeMessage);
			client.Disconnect(true);
		}
	}
}
