using Application.Persistance;
using Ardalis.Specification;
using Domain.Models;
using MediatR;
using Shared.Messages.Master;
using Shared.Wrapper;

namespace Application.MessageHandlers.Master;

public class MSM_002_Handler : IRequestHandler<MSM_002, IResult>
{
	private readonly IRepository<AppUser> _repository;

	public MSM_002_Handler(IRepository<AppUser> repository) => _repository = repository;
	public async Task<IResult> Handle(MSM_002 request, CancellationToken cancellationToken)
	{
		var user = await _repository.FirstOrDefaultAsync(new AppUserByIdSpec(request.UserId));
		if (user == null)
			throw new Exception("User not found");

		user.AddNewMasterService(MasterService.Create(request.Name, request.Description, request.NewPriceValue, (DateTime)request.NewPriceDateTime));
		await _repository.UpdateAsync(user);
		return Result.Success();
	}
}
public class AppUserByIdSpec : Specification<AppUser>, ISingleResultSpecification
{
	public AppUserByIdSpec(string userId) =>
		Query
		.Where(b => b.Id == userId)
		.Include(x => x.Services)
		.ThenInclude(x => x.Prices);
}