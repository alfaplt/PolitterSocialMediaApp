using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Controllers
{
    public class MessagesController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly AppDbContext _context;
        public MessagesController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> ChatPage(string userName)
		{
			AppUser userWithChatting =await _userManager.FindByNameAsync(userName);
            AppUser senderUser = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.receiverUserName = userName;
			ViewBag.senderUserName = senderUser.UserName;
			ViewBag.userWithChattingPhoto = userWithChatting.ProfilePicture;
			ViewBag.senderId = senderUser.Id;
			ViewBag.receiverId = userWithChatting.Id;
		
			var messages = _context.Messages.Include(x => x.Sender).Include(x => x.Receiver).Where(x => x.SenderId == senderUser.Id && x.ReceiverId == userWithChatting.Id || x.ReceiverId == senderUser.Id && x.SenderId == userWithChatting.Id).ToList();

			return View(messages);
		}

		[HttpPost]
		public async Task<IActionResult> SendMessage(Message message, string userName)
		{
			AppUser senderUser = await _userManager.FindByNameAsync(User.Identity.Name);
			AppUser receiverUser = await _userManager.FindByNameAsync(userName);
            ViewBag.receiverUserName = receiverUser.UserName;
			message.SendDate = DateTime.Now;
			message.SenderId = senderUser.Id;
			message.ReceiverId = receiverUser.Id;
			_context.Messages.Add(message);
			_context.SaveChanges();

			return NoContent();
		}

		public IActionResult _ListPeople()
		{
            return PartialView();
		}

		
		public async Task<IActionResult> DeleteMessagesAsync(int id, string userName)
		{
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            var messages = _context.Messages.Where(x => x.SenderId == id && x.ReceiverId == user.Id || x.SenderId == user.Id && x.ReceiverId == id).ToList();
            _context.Messages.RemoveRange(messages);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		public IActionResult Index()
		{
			return View();
		}
        
    }
}
