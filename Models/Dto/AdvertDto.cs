﻿using Models.DbModels;

namespace Models.Dto
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