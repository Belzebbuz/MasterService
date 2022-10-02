using Shared.Messages.Identity;
using Shared.Wrapper;

namespace WebUI.Application.Services.Managers;

public interface IUserManager : ITransientService
{
	Task<IResult<IDR_003>> GetAsync(string userId);
}
