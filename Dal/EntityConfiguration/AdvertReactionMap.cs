using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EntityConfiguration
{
    public class AdvertReactionMap : IEntityTypeConfiguration<AdvertReaction>
    {
        public void Configure (EntityTypeBuilder<AdvertReaction> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Id).IsRequired();

            entityBuilder.Property(x => x.UserId).IsRequired();
            entityBuilder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.Property(x => x.AdvertId).IsRequired();
        
            entityBuilder
                .HasOne(x => x.Advert)
                .WithMany(x => x.AdvertReaction)
                .HasForeignKey(x => x.AdvertId)
                .OnDelete(DeleteBehavior.Cascade);

            entityBuilder.Property(ar => ar.Reaction)
                .IsRequired()
                .HasConversion<int>();
        
        }
    }
}

