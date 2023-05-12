using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal.EntityConfiguration
{
    public class AdvertImageMap : IEntityTypeConfiguration<AdvertImage>
    {
        public void Configure(EntityTypeBuilder<AdvertImage> entityBuilder)
        {
            entityBuilder.HasKey(ai => ai.Id);

            entityBuilder.Property(ai => ai.FileName)
                .IsRequired();

            entityBuilder.Property(ai => ai.FileType)
                .IsRequired();

            entityBuilder.Property(ai => ai.FilePath)
                .IsRequired();

            entityBuilder.HasOne(ai => ai.Advert)
                .WithMany(a => a.AdvertImages)
                .HasForeignKey(ai => ai.AdvertId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}