using Core.Entities;
using System.Collections.Generic;

namespace Mvc.Models.ViewModels
{
    public class UserPosts
    {
        public AppUser AppUser { get; set; }
        public List<Post> Posts { get; set; }
    }
}
