using Application.Persistance;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Common;
using Infrastructure.Context;
using Mapster;

namespace Infrastructure.Repository;

public class ApplicationDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
	where T : class, IAggregateRoot
{
	public ApplicationDbRepository(ApplicationDbContext dbContext)
		: base(dbContext)
	{
	}

	protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
		ApplySpecification(specification, false)
			.ProjectToType<TResult>();
}
