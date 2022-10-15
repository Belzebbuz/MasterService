using Application.Common;
using Application.Persistance;
using Ardalis.Specification;
using Domain.Models;
using MediatR;
using Shared.Messages.Master;
using Shared.Wrapper;

namespace Application.MessageHandlers.Master;

public class MSM_005_Handler : IRequestHandler<MSM_005, IResult>
{
	private readonly IRepository<MasterService> _repository;

	public MSM_005_Handler(IRepository<MasterService> repository)
	{
		_repository = repository;
	}
	public async Task<IResult> Handle(MSM_005 request, CancellationToken cancellationToken)
	{
		var service = await _repository.FirstOrDefaultAsync(new MasterServiceByIdSpec(request.ServiceId));
		service = service ?? throw new ArgumentNullException(nameof(service));
		service.Update(request.Name, request.Description);
		service.AddPrice((DateTime)request.NewPriceDateTime, request.NewPriceValue);
		await _repository.SaveChangesAsync();
		return await Result.SuccessAsync();
	}
}
public class MasterServiceByIdSpec : Specification<MasterService>, ISingleResultSpecification
{
	public MasterServiceByIdSpec(Guid serviceId) =>
		Query
		.Where(b => b.Id == serviceId)
		.Include(x => x.Examples)
		.Include(x => x.Prices);
}