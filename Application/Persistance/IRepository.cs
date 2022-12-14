using Ardalis.Specification;
using Domain.Common;

namespace Application.Persistance;

public interface IRepository<T> : IRepositoryBase<T>
	where T : class, IAggregateRoot
{
}


public interface IReadRepository<T> : IReadRepositoryBase<T>
	where T : class, IAggregateRoot
{
}

