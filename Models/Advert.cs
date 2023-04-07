using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Advert
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Text { get; set; }

        public int Rating { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public virtual ICollection<AdvertImage> AdvertImages { get; set; }
    }
}