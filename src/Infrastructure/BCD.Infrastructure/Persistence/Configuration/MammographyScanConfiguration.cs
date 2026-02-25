using BCD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCD.Infrastructure.Persistence.Configuration
{
    internal class MammographyScanConfiguration : IEntityTypeConfiguration<MammographyScan>
    {
        public void Configure(EntityTypeBuilder<MammographyScan> builder)
        {
            builder.ToTable("MammographyScans");

            builder.HasKey(m => m.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(m => m.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.PredictionResult)
                .HasMaxLength(50);

            builder.Property(m => m.ConfidenceScore)
                .HasPrecision(18, 2);

            builder.Property(m => m.CreatedAt)
                .IsRequired();

            builder.HasOne(m => m.Patient)
                .WithMany()
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
