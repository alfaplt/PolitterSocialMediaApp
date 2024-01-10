using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Post 
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public DateTime CreatedDate { get; set; }      
        public string Content { get; set; }
        public AppUser AppUser { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Favorite> Favorites { get; set; }
    }
}
