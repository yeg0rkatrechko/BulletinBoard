using Domain;

namespace Services.Models
{
    public class AdvertDto
    {
        public string UserName { get; set; } = null!;
        public string Text { get; set; } = null!;

        public DateTime TimeCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public virtual ICollection<AdvertImage>? AdvertImages { get; set; }

        public virtual ICollection<AdvertReaction>? Reactions { get; set; }

    }
}
