using MediatR;
using Shared.Messages.Identity;
using Shared.Messages.Master;
using Shared.Wrapper;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Application.Services.Managers;
using WebUI.Infrastructure.Endpoints;

namespace WebUI.Infrastructure.Managers;

public class MasterManager : IMasterManager
{
	private readonly HttpClient _httpClient;

	public MasterManager(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<IResult<MSR_001>> GetAllServicesAsync(string userId)
    {
        var response = await _httpClient.GetAsync(MasterEnpoints.GetAllServices(userId));
        return await response.ToResult<MSR_001>();
    }

	public async Task<IResult> CreateServiceAsync(MSM_002 message)
	{
		var response = await _httpClient.PostAsJsonAsync(MasterEnpoints.MasterService, message);
		return await response.ToResult();
	}

	public async Task<IResult> AddExamplePictureAsync(MSM_003 request)
	{
		var response = await _httpClient.PostAsJsonAsync(MasterEnpoints.ServicePicture, request);
		return await response.ToResult();
	}

	public async Task<IResult<MSR_004>> GetServiceAsync(Guid id)
	{
		var response = await _httpClient.GetAsync(MasterEnpoints.GetSingleService(id.ToString()));
		return await response.ToResult<MSR_004>();
	}

	public async Task<IResult> UpdateServiceAsync(MSM_005 message)
	{
		var response = await _httpClient.PutAsJsonAsync(MasterEnpoints.MasterService, message);
		return await response.ToResult();
	}

	public async Task<IResult> DeleteExamplePictureAsync(Guid serviceId, Guid picId)
	{
		var response = await _httpClient.DeleteAsync(MasterEnpoints.DeleteServicePicture(serviceId.ToString(), picId.ToString()));
		return await response.ToResult();
	}
}
