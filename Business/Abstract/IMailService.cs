using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IMailService
	{
		void SendMailForResetPassword(AppUser user, string passwordResetTokenLink);
        void SendMailForIncomingMessage(AppUser senderUser, AppUser receiverUser, Message message);
    }
}
