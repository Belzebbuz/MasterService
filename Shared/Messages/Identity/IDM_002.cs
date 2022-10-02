using MediatR;
using Shared.Wrapper;
using System.ComponentModel.DataAnnotations;

namespace Shared.Messages.Identity;

public class IDM_002 
{
	[Required(ErrorMessage = "Поле должно быть заполнено!")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Поле должно быть заполнено!")]
	public string Password { get; set; }
}

public class IDR_002
{
	public string Token { get; set; }
	public string RefreshToken { get; set; }
	public DateTime RefreshTokenExpiryTime { get; set; }
}