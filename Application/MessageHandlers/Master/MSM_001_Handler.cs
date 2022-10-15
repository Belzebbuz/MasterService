using Application.Persistance;
using Ardalis.Specification;
using Domain.Models;
using MediatR;
using Shared.Messages.Master;
using Shared.Wrapper;

namespace Application.MessageHandlers.Master;

public class MSM_001_Handler : IRequestHandler<MSM_001, IResult<MSR_001>>
{
	private readonly IReadRepository<AppUser> _repository;

	public MSM_001_Handler(IReadRepository<AppUser> repository)
	{
		_repository = repository;
	}
	public async Task<IResult<MSR_001>> Handle(MSM_001 request, CancellationToken cancellationToken)
	{
		var user = await _repository.FirstOrDefaultAsync(new ReadAppUserByIdSpec(request.UserId));
		var services = new List<MasterServiceResponse>();

		foreach (var service in user!.Services)
		{
			var currentPrice = service.GetPrice(DateTime.Now);
			var picturesData = new List<ImageDataResponse>();
			var convertedImages = service.GetImagesAsData();
			convertedImages.ForEach(x => picturesData.Add(new(x.Id, x.data)));
			services.Add(new(service.Id, service.Name, picturesData, service.Description, currentPrice));
		}

		return await Result<MSR_001>.SuccessAsync(data: new(services));
	}
}

public class ReadAppUserByIdSpec : Specification<AppUser>, ISingleResultSpecification
{
	public ReadAppUserByIdSpec(string userId) =>
		Query
		.AsNoTracking()
		.Where(b => b.Id == userId)
		.Include(x => x.Services)
		.ThenInclude(x => x.Prices)
		.Include(b => b.Services)
		.ThenInclude(s => s.Examples);
}