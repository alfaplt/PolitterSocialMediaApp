using Core.Entities;

namespace Business.Abstract
{
    public interface IMailService
	{
		void SendMailForResetPassword(AppUser user, string passwordResetTokenLink);
        void SendMailForIncomingMessage(AppUser senderUser, AppUser receiverUser, Message message);
		void SendMailForLogging(AppUser user);
    }
}
