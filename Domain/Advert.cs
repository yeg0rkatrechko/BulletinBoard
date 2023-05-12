namespace Domain
{
    public class Advert
    {
        public Guid Id { get; set; }

        public string? Text { get; set; } = null!;
        
        public string? Heading { get; set; } = null!;
        
        public bool IsDraft { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<AdvertImage>? AdvertImages { get; set; }
        public ICollection<AdvertReaction>? AdvertReaction { get; set; } = null;
    }
}