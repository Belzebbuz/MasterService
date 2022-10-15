using Application.Common;
using Application.Persistance;
using Ardalis.Specification;
using Domain.Models;
using MediatR;
using Shared.Messages.Master;
using Shared.Wrapper;

namespace Application.MessageHandlers.Master;

public class MSM_003_Handler : IRequestHandler<MSM_003, IResult>
{
	private readonly IRepository<MasterService> _repository;
	private readonly IUploadService _uploadService;

	public MSM_003_Handler(IRepository<MasterService> repository, IUploadService uploadService)
	{
		_repository = repository;
		_uploadService = uploadService;
	}
	public async Task<IResult> Handle(MSM_003 request, CancellationToken cancellationToken)
	{
		var service = await _repository.FirstOrDefaultAsync(new MasterServiceByIdSpec(request.ServiceId));
		if (service == null)
			throw new ArgumentNullException(nameof(service));
		string filePath = _uploadService.Upload(request);
		service.AddExamplePhoto(filePath);
		await _repository.SaveChangesAsync();
		return Result.Success();
	}
}
