using Shared.Messages.Identity;
using Shared.Wrapper;
using System.Security.Claims;

namespace WebUI.Application.Services.Managers;

public interface IAuthenticationManager : ITransientService
{
	Task<IResult> Login(IDM_002 tokenRequest);
	Task<IResult> Logout();
	Task<ClaimsPrincipal> CurrentUser();
}
