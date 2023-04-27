using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DbModels
{
    public class AdvertImage
    {
        [Key]
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FilePath { get; set; }

        [ForeignKey("AdvertId")]
        public Advert Advert { get; set; }
    }
}
