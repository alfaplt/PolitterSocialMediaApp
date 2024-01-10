using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Favorite
    {
        public int AppUserId { get; set; }
        public int PostId { get; set; }
        public AppUser User { get; set; }
        public Post Post { get; set; }
    }
}
