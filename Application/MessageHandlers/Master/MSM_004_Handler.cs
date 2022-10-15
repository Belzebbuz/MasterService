using Application.Persistance;
using Domain.Models;
using MediatR;
using Shared.Messages.Master;
using Shared.Wrapper;

namespace Application.MessageHandlers.Master;

public class MSM_004_Handler : IRequestHandler<MSM_004, IResult<MSR_004>>
{
	private readonly IReadRepository<AppUser> _repository;

	public MSM_004_Handler(IReadRepository<AppUser> repository)
	{
		_repository = repository;
	}
	public async Task<IResult<MSR_004>> Handle(MSM_004 request, CancellationToken cancellationToken)
	{
		var user = await _repository.FirstOrDefaultAsync(new ReadAppUserByIdSpec(request.UserId));
		if (user == null)
			throw new Exception("User not found!");
		var service = user.Services.FirstOrDefault(x => x.Id == request.ServiceId);
		if (service == null)
			throw new Exception($"{user.Email} have no service with id: {request.ServiceId}");

		var currentPrice = service.GetPrice(DateTime.Now);
		var picturesData = new List<ImageDataResponse>();
		var convertedImages = service.GetImagesAsData();
		convertedImages.ForEach(x => picturesData.Add(new(x.Id, x.data)));
		var serviceDTO = new MasterServiceResponse(service.Id, service.Name, picturesData, service.Description, currentPrice);
		return await Result<MSR_004>.SuccessAsync(data: new(serviceDTO));
	}
}
