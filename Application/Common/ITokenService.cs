using Application.Common.Contracts;
using Shared.Messages.Identity;
using Shared.Wrapper;

namespace Application.Common;

public interface ITokenService : ITransientService
{
	Task<IResult<IDR_002>> GetTokenAsync(IDM_002 request, string ipAddress, CancellationToken cancellationToken);
}
