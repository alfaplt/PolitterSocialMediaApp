namespace Core.Entities
{
    public class Follow
    {
        public int FollowingId { get; set; }
        public int FollowedId { get; set; }
        public AppUser Following { get; set; }
        public AppUser Followed { get; set; }
    }
}
