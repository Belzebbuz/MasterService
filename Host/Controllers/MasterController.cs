using Application.Common;
using Domain.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Messages.Master;

namespace Host.Controllers;

[Route("api/master")]
public class MasterController : BaseApiController
{
	private readonly ICurrentUser _currentUser;

	public MasterController(ICurrentUser currentUser) => _currentUser = currentUser;

	[HttpGet("all/{id}")]
	[Authorize]
	public async Task<Shared.Wrapper.IResult<MSR_001>> GetMastersServicesAsync(string id)
		=> await Mediator.Send(new MSM_001(id));

	[HttpGet("single/{id}")]
	[Authorize(Roles = UserRoles.Master)]
	public async Task<Shared.Wrapper.IResult<MSR_004>> GetAsync(Guid id)
		=> await Mediator.Send(new MSM_004(_currentUser.UserId, id));

	[HttpPost]
	[Authorize(Roles = UserRoles.Master)]
	public async Task<Shared.Wrapper.IResult> CreateMasterServiceAsync(MSM_002 message)
		=> await Mediator.Send(message);

	[HttpPut]
	[Authorize(Roles = UserRoles.Master)]
	public async Task<Shared.Wrapper.IResult> UpdateMasterServiceAsync(MSM_005 message)
		=> await Mediator.Send(message);

	[HttpPost("service-picture")]
	[Authorize(Roles = UserRoles.Master)]
	public async Task<Shared.Wrapper.IResult> AddServicePictureAsync(MSM_003 message)
		=> await Mediator.Send(message);

	[HttpDelete("service-picture/{serviceId}/{imageId}")]
	[Authorize(Roles = UserRoles.Master)]
	public async Task<Shared.Wrapper.IResult> AddServicePictureAsync(Guid serviceId, Guid imageId)
		=> await Mediator.Send(new MSM_006(imageId, serviceId));
}
