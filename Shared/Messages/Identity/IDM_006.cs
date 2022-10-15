using FluentValidation;

namespace Shared.Messages.Identity;

/// <summary>
/// Запрос на обновление данных профиля
/// </summary>
public class IDM_006
{
	public string FullName { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
}

public class IDM_006_Validator : AbstractValidator<IDM_006>
{
	public IDM_006_Validator()
	{
		RuleFor(request => request.FullName)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Поле должно быть заполнено.");
		RuleFor(request => request.Email)
			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Поле должно быть заполнено.");
	}
}