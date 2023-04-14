using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Models
{
    public class AdvertDto
    {
        public string UserName { get; set; }
        public string Text { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public virtual ICollection<AdvertImage> AdvertImages { get; set; }

        public virtual ICollection<AdvertReaction> Reactions { get; set; }

    }
}
