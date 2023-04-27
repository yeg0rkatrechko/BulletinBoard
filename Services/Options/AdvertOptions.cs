namespace Services.Options
{
    public class AdvertOptions
    {
        public const string Options = "AdvertOptions";
        public int MaxAdvertsPerUser { get; set; }
        public int ExpirationDate { get; set; }
    }
}
