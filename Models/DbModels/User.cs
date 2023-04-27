using System.ComponentModel.DataAnnotations;

namespace Models.DbModels
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Admin { get; set; }

        public ICollection<Advert> Adverts { get; set; }
    }
}
