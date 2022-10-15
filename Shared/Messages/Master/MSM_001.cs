using MediatR;
using Shared.Wrapper;

namespace Shared.Messages.Master;

/// <summary>
/// Запрос на список всех услуг мастера
/// </summary>
public class MSM_001 : IRequest<IResult<MSR_001>>
{
	public string UserId { get; private set; }

	public MSM_001(string userId)
	{
		UserId = userId;
	}
}
