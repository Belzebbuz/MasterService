using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Wrapper;
using IResult = Shared.Wrapper.IResult;

namespace Host.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
	private ISender _mediator = null!;

	protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

	protected async Task<IActionResult> HandleMessage(IRequest<IResult> request)
	{
		var result = await Mediator.Send(request);
		if(!result.Succeeded)
			return BadRequest(result);
		return Ok(result);
	}
}
