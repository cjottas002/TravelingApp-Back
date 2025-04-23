using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelingApp.Domain.Entities;

namespace TravelingApp.Infraestructure.Context.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users", "Travel");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                  .HasColumnName("IdUsuario")
                  .IsRequired();

            entity.Property(u => u.IsActive)
                  .HasColumnName("IsActive")
                  .HasColumnType("bit")
                  .HasDefaultValue(false);

            entity.Property(u => u.UserName)
                  .HasColumnName("Nombre")
                  .HasColumnType("nvarchar(256)")
                  .IsRequired(false)
                  .HasDefaultValue(string.Empty);

            entity.Property(u => u.NormalizedUserName)
                  .HasColumnName("NormalizedUserName")
                  .HasColumnType("nvarchar(256)")
                  .IsRequired(false)
                  .HasDefaultValue(string.Empty);

            entity.Property(u => u.Email)
                  .HasColumnName("Email")
                  .HasColumnType("nvarchar(256)")
                  .IsRequired(false)
                  .HasDefaultValue(string.Empty);

            entity.Property(u => u.NormalizedEmail)
                  .HasColumnName("NormalizedEmail")
                  .HasColumnType("nvarchar(256)")
                  .IsRequired(false)
                  .HasDefaultValue(string.Empty);

            entity.Property(u => u.EmailConfirmed)
                  .HasColumnName("EmailConfirmed")
                  .HasColumnType("bit")
                  .HasDefaultValue(false);

            entity.Property(u => u.PasswordHash)
                  .HasColumnName("Password")
                  .HasColumnType("nvarchar(max)")
                  .IsRequired(false)
                  .HasDefaultValue(string.Empty);

            entity.Property(u => u.SecurityStamp)
                  .HasColumnName("SecurityStamp")
                  .HasColumnType("nvarchar(max)")
                  .IsRequired(false)
                  .HasDefaultValue(string.Empty);

            entity.Property(u => u.ConcurrencyStamp)
                  .HasColumnName("ConcurrencyStamp")
                  .HasColumnType("nvarchar(max)")
                  .IsRequired(false)
                  .HasDefaultValue(string.Empty);

            entity.Property(u => u.PhoneNumber)
                  .HasColumnName("PhoneNumber")
                  .HasColumnType("nvarchar(max)")
                  .IsRequired(false)
                  .HasDefaultValue(string.Empty);

            entity.Property(u => u.PhoneNumberConfirmed)
                  .HasColumnName("PhoneNumberConfirmed")
                  .HasColumnType("bit")
                  .HasDefaultValue(false);

            entity.Property(u => u.TwoFactorEnabled)
                  .HasColumnName("TwoFactorEnabled")
                  .HasColumnType("bit")
                  .HasDefaultValue(false);

            entity.Property(u => u.LockoutEnd)
                  .HasColumnName("LockoutEnd")
                  .HasColumnType("datetimeoffset")
                  .IsRequired(false);

            entity.Property(u => u.LockoutEnabled)
                  .HasColumnName("LockoutEnabled")
                  .HasColumnType("bit")
                  .HasDefaultValue(false);

            entity.Property(u => u.AccessFailedCount)
                  .HasColumnName("AccessFailedCount")
                  .HasColumnType("int")
                  .HasDefaultValue(0);
        }
    }
}
