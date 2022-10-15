using MediatR;
using Shared.Wrapper;

namespace Shared.Messages.Master;

public class MSM_004 : IRequest<IResult<MSR_004>>
{
	public MSM_004(string userId, Guid serviceId)
	{
		UserId = userId;
		ServiceId = serviceId;
	}

	public string UserId { get; set; }
	public Guid ServiceId { get; set; }
}
