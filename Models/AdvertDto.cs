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
        public Guid Id { get; } = Guid.NewGuid();

        public Guid UserId { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; }

        public DateTime TimeCreated { get; } = DateTime.UtcNow;

        public DateTime ExpirationDate { get; set; }

    }
}
