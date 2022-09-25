using Domain.Models;
using Domain.Models.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
	public void Configure(EntityTypeBuilder<IdentityRole> builder)
	{
		builder.HasData(
			new IdentityRole
			{
				Name = UserRoles.Master,
				NormalizedName = UserRoles.Master.ToUpper(),
			},
			new IdentityRole
			{
				Name = UserRoles.Admin,
				NormalizedName = UserRoles.Admin.ToUpper(),
			},
			new IdentityRole
			{
				Name = UserRoles.Basic,
				NormalizedName = UserRoles.Basic.ToUpper(),
			});
	}
}
