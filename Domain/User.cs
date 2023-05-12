namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public bool Admin { get; set; }

        public ICollection<Advert>? Adverts { get; set; }
    }
}
