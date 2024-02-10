using System;

namespace Core.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; }
        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}
