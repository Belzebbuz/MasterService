using Application.Persistance;
using Domain.Models;
using MediatR;
using Shared.Messages.Master;
using Shared.Wrapper;

namespace Application.MessageHandlers.Master;

public class MSM_006_Handler : IRequestHandler<MSM_006, IResult>
{
	private readonly IRepository<MasterService> _repository;

	public MSM_006_Handler(IRepository<MasterService> repository)
	{
		_repository = repository;
	}
	public async Task<IResult> Handle(MSM_006 request, CancellationToken cancellationToken)
	{
		var service = await _repository.FirstOrDefaultAsync(new MasterServiceByIdSpec(request.ServiceId));
		service = service ?? throw new ArgumentNullException(nameof(service));
		service.RemoveExamplePhoto(request.ImageId);
		await _repository.SaveChangesAsync();
		return await Result.SuccessAsync();
	}
}
