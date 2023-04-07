using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
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
