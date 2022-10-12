using Domain.Models;
using Mapster;
using Shared.Messages.Identity;

namespace Infrastructure.Mappings;

public class UserResponse_Mapping : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<AppUser, UserResponse>()
			.RequireDestinationMemberSource(true);
		config.NewConfig<AppUser, IDR_003>()
			.RequireDestinationMemberSource(true);
	}
}
