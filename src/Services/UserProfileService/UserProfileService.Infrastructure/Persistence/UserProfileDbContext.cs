using Microsoft.EntityFrameworkCore;
using UserProfileService.Domain.Entities;
using UserProfileService.Domain.ValueObjects;

namespace UserProfileService.Infrastructure.Persistence;

public sealed class UserProfileDbContext : DbContext
{
    public UserProfileDbContext(DbContextOptions<UserProfileDbContext> options) : base(options)
    {
    }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>(builder =>
        {
            builder.ToTable("user_profiles");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ExternalIdentityId)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex(x => x.ExternalIdentityId).IsUnique();

            builder.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(x => x.Country).HasColumnName("country").HasMaxLength(100).IsRequired();
                address.Property(x => x.City).HasColumnName("city").HasMaxLength(100).IsRequired();
                address.Property(x => x.Street).HasColumnName("street").HasMaxLength(150).IsRequired();
                address.Property(x => x.House).HasColumnName("house").HasMaxLength(50).IsRequired();
                address.Property(x => x.PostalCode).HasColumnName("postal_code").HasMaxLength(20).IsRequired();
            });
        });
    }
}
