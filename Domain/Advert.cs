namespace Domain
{
    public class Advert
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = null!;
        
        // todo index in entity type configuration
        // todo поиск по нему в сервисе
        public string Heading { get; set; } = null!;

        // todo index in entity type configuration
        public bool IsDraft { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public Guid UserId { get; set; }

        public User? User { get; set; }

        public ICollection<AdvertImage>? AdvertImages { get; set; }
        public ICollection<AdvertReaction>? AdvertReaction { get; set; } = null;
    }
}