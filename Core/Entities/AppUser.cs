﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class AppUser : IdentityUser<int>
	{
		public DateTime RegisterDate { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ProfilePicture { get; set; }
        public string About { get; set; }
        public bool Isdeleted { get; set; }
        public List<Post> Posts { get; set; }
		public List<Comment> Comments { get; set; }
		public List<Favorite> Favorites { get; set; }
		public List<Follow> Followings { get; set; }
		public List<Follow> Followeds { get; set; }
        public List<Message> SendedMessages { get; set; }
        public List<Message> ReceivedMessages { get; set; }
        
    }
}
