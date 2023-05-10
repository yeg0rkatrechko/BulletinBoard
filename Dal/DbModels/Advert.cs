﻿// using Microsoft.EntityFrameworkCore;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using Dal.DbModels;
//
// namespace Models.DbModels
// {
//     [Index(nameof(UserId))]
//     public class Advert
//     {
//         [Key]
//         public Guid Id { get; set; }
//
//         [Column(TypeName = "nvarchar(max)")]
//         public string Text { get; set; }
//
//         public bool IsDraft { get; set; }
//
//         public DateTime TimeCreated { get; set; }
//
//         public DateTime ExpirationDate { get; set; }
//
//         public Guid UserId { get; set; }
//
//         [ForeignKey("UserId")]
//         public User User { get; set; }
//
//         public virtual ICollection<AdvertImage>? AdvertImages { get; set; }
//         public virtual ICollection<AdvertReaction>? AdvertReaction { get; set; } = null;
//     }
// }