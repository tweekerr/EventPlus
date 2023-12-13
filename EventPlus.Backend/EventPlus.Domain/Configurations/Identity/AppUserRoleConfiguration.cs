using EventPlus.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventPlus.Domain.Configurations.Identity;

internal class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.ToTable($"{nameof(AppUserRole)}s").HasKey(e => new
        {
            e.UserId,
            e.RoleId
        });

        builder.HasOne(e => e.Role)
            .WithMany(e => e.UserRoles)
            .HasForeignKey(e => e.RoleId)
            .IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.User)
            .WithMany(e => e.UserRoles)
            .HasForeignKey(e => e.UserId)
            .IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}