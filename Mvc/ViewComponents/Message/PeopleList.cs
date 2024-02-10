using Core.Entities;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.ViewComponents.Message
{
    public class PeopleList : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public PeopleList(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppUser authUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var messages = _context.Messages.Include(x => x.Sender).Include(x => x.Receiver).Where(x => x.SenderId == authUser.Id || x.ReceiverId == authUser.Id).ToList();
            List<AppUser> users = new List<AppUser>();

            if (authUser.SendedMessages != null)
            {
                foreach (var item in authUser.SendedMessages)
                {
                    if(!users.Contains(item.Receiver))
                    {
                        users.Add(item.Receiver);
                    }
                }
            }

            if (authUser.ReceivedMessages != null)
            {
                foreach (var item in authUser.ReceivedMessages)
                {
                    if(!users.Contains(item.Sender))
                    {
                        users.Add(item.Sender);
                    }
                }
            }

            return View(users);
        }
    }
}
