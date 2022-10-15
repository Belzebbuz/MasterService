using Shared.Messages.Identity;
using Shared.Wrapper;

namespace WebUI.Application.Services.Managers;

public interface IAccountManager
{
    Task<IResult> ChangePasswordAsync(IDM_008 model);
    Task<string> GetCurrentUserProfileImageAsync(string userId);
    Task<IResult<string>> GetProfilePictureAsync(string userId);
    Task<IResult> UpdateProfileAsync(IDM_006 profileModel);
    Task<IResult<string>> UpdateProfilePictureAsync(IDM_007 request, string userId);
}
