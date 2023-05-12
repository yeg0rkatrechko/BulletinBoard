namespace Domain
{
    public class AdvertImage
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = null!;

        public string FileType { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public Guid AdvertId { get; set; }

        public Advert? Advert { get; set; }
    }
}
