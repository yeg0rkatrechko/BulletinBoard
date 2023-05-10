using System.Data.Entity.ModelConfiguration;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EntityConfiguration;

public class AdvertReactionMap : EntityTypeConfiguration<AdvertReaction>
{
    public AdvertReactionMap(EntityTypeBuilder<AdvertReaction> entityBuilder)
    {
        ToTable(nameof(AdvertReaction), "BulletinBoard");
        entityBuilder.HasKey(x => x.Id);

        entityBuilder.Property(x => x.Id).IsRequired();

        entityBuilder.Property(x => x.UserId).IsRequired();
        entityBuilder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
        
        
        entityBuilder.Property(x => x.AdvertId).IsRequired();
        entityBuilder
            .HasOne(x => x.Advert)
            .WithMany(x => x.AdvertReaction)
            .HasForeignKey(x => x.AdvertId);
        
        
        entityBuilder.Property(x => x.IsLike).IsRequired();

    }
}