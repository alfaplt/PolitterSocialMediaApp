using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public AppUser AppUser { get; set; }
        public Post Post { get; set; }
    }
}
