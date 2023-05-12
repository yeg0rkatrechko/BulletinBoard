using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EntityConfiguration
{
    public class AdvertMap: IEntityTypeConfiguration<Advert>
    {
        public void Configure(EntityTypeBuilder<Advert> entityBuilder)
        {
            entityBuilder.HasKey(a => a.Id);

            entityBuilder.Property(a => a.Text)
                .IsRequired();

            entityBuilder.Property(a => a.Heading)
                .IsRequired();
        
            entityBuilder.HasIndex(a => a.Heading);

            entityBuilder.Property(a => a.IsDraft)
                .IsRequired();
        
            entityBuilder.HasIndex(a => a.IsDraft);

            entityBuilder.Property(a => a.TimeCreated)
                .IsRequired();

            entityBuilder.Property(a => a.ExpirationDate)
                .IsRequired();

            entityBuilder.HasOne(a => a.User)
                .WithMany(u => u.Adverts)
                .HasForeignKey(a => a.UserId);

            entityBuilder
                .HasMany(a => a.AdvertImages)
                .WithOne(ai => ai.Advert)
                .HasForeignKey(ai => ai.AdvertId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        
            entityBuilder.HasMany(a => a.AdvertReaction)
                .WithOne(ar => ar.Advert)
                .HasForeignKey(ar => ar.AdvertId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

