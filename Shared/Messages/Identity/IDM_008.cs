using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Shared.Messages.Identity;
/// <summary>
/// Запрос на изменение пароля
/// </summary>
public class IDM_008
{
	public string Password { get; set; }

	public string NewPassword { get; set; }

	public string ConfirmNewPassword { get; set; }
}

public class IDM_008_Validator : AbstractValidator<IDM_008>
{
	private const string _mustBeenFeeledMessage = "Поле должно быть заполнено!";
	public IDM_008_Validator()
	{
		RuleFor(request => request.Password)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(_mustBeenFeeledMessage);
		RuleFor(request => request.NewPassword)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(_mustBeenFeeledMessage)
				.MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов")
				.Matches(@"[A-Z]").WithMessage("Пароль должен содержать минимум один символ верхнего регистра")
				.Matches(@"[a-z]").WithMessage("Пароль должен содержать минимум один символ нижнего регистра")
				.Matches(@"[0-9]").WithMessage("Пароль должен содержать минимум одну цифру");
		RuleFor(request => request.ConfirmNewPassword)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(_mustBeenFeeledMessage)
				.Equal(request => request.NewPassword).WithMessage("Пароли должны совпадать");
	}
}