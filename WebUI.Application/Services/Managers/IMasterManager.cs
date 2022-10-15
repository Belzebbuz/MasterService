using Shared.Messages.Master;
using Shared.Wrapper;

namespace WebUI.Application.Services.Managers;

public interface IMasterManager
{
    Task<IResult<MSR_001>> GetAllServicesAsync(string userId);
    Task<IResult> CreateServiceAsync(MSM_002 message);
    Task<IResult> AddExamplePictureAsync(MSM_003 request);
    Task<IResult<MSR_004>> GetServiceAsync(Guid id);
    Task<IResult> UpdateServiceAsync(MSM_005 message);
    Task<IResult> DeleteExamplePictureAsync(Guid serviceId, Guid picId);
}
