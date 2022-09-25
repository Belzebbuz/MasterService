using Application.Common.Contracts;
using Shared.Messages.Identity;
using Shared.Wrapper;

namespace Application.Common;

public interface IUserService : ITransientService
{
	Task<IResult> CreateUserAsync(IDM_001 message);
}
