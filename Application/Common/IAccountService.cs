using Application.Common.Contracts;
using Shared.Messages.Identity;
using Shared.Wrapper;

namespace Application.Common;

public interface IAccountService : IScopedService
{
    Task<IResult> ChangePasswordAsync(IDM_008 request, string userId);
    Task<IResult<string>> GetProfilePictureAsync(string userId);
    Task<IResult> UpdateProfileAsync(IDM_006 request, string userId);
    Task<IResult<string>> UpdateProfilePictureAsync(IDM_007 request, string userId);
}
