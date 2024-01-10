using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Mvc.ViewComponents.Post
{
    public class RightColumn : ViewComponent
    {
        public IViewComponentResult Invoke()
        {   
            return View();
        }
    }
}
