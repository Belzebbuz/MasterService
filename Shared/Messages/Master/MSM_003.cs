using MediatR;
using Shared.Messages.Identity;
using Shared.Wrapper;

namespace Shared.Messages.Master;

/// <summary>
/// Запрос на добавление изображения услуги
/// </summary>
public class MSM_003 : UploadRequest, IRequest<IResult>
{
	public Guid ServiceId { get; set; }
}
