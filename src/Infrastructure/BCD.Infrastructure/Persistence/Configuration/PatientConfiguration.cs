using BCD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCD.Infrastructure.Persistence.Configuration
{
    internal class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");

            builder.HasKey(p => p.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.NHSNumber)
                .IsRequired()
                .HasMaxLength(12);

            builder.HasIndex(p => p.NHSNumber)
                .IsUnique();

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Gender)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.PostCode)
                .HasMaxLength(10);

            builder.Property(p => p.CreatedAt)
                .IsRequired();


            builder.Property(p => p.IsActive)
                .HasDefaultValue(true)
                .IsRequired();
        }
    }
}
