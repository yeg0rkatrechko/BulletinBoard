using Domain;

namespace Services.Models
{
    public class AdvertDto
    {
        public string UserName { get; set; } = null!;
        
        public string Heading { get; set; } = null!;
        
        public string Text { get; set; } = null!;

        public DateTime TimeCreated { get; set; }

        public DateTime ExpirationDate { get; set; }
        public int ReactionSum { get; set; }
        public virtual List<string>? AdvertImages { get; set; }
        
    }
}
