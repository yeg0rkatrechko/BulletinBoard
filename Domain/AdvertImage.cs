namespace Domain
{
    public class AdvertImage
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FilePath { get; set; }

        public Advert Advert { get; set; }
    }
}
