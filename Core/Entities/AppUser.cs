﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class AppUser : IdentityUser<int>
	{
		public DateTime RegisterDate { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ProfilePicture { get; set; }
		public List<Post> Posts { get; set; }
		public List<Comment> Comments { get; set; }
		public List<Favorite> Favorites { get; set; }
	}
}
