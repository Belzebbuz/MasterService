using MediatR;
using Shared.Wrapper;

namespace Shared.Messages.Master;
/// <summary>
/// Запрос на удаление изображения услуги
/// </summary>
public class MSM_006 : IRequest<IResult>
{
	public Guid ImageId { get; private set; }
	public Guid ServiceId { get; private set; }
	public MSM_006(Guid imageId, Guid serviceId)
	{
		ImageId = imageId;
		ServiceId = serviceId;
	}
}
