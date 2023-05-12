namespace Domain
{
    public class AdvertReaction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AdvertId { get; set; }
        public User? User { get; set; }
        public Advert? Advert { get; set; }
        public Reaction Reaction { get; set; }
    }

    public enum Reaction
    {
        Like = 1,
        Dislike = -1
    }
}
