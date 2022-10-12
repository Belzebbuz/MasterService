using FluentValidation;
using MediatR;
using Shared.Wrapper;
using System.ComponentModel.DataAnnotations;

namespace Shared.Messages.Identity;
public class IDM_001 
{
	public string FullName { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }
	
	public string ConfirmPassword { get; set; }
	public string? PhoneNumber { get; set; }
}

public class IDM_001_Validator : AbstractValidator<IDM_001>
{
	public const string MustBeenFeeledMessage = "Поле должно быть заполнено!";
	public IDM_001_Validator()
	{
		RuleFor(message => message.FullName)
			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(MustBeenFeeledMessage);
		RuleFor(message => message.Email)
			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(MustBeenFeeledMessage)
			.EmailAddress().WithMessage("Поле должно соответсвовать типу email");
		RuleFor(request => request.Password)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(MustBeenFeeledMessage)
				.MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов")
				.Matches(@"[A-Z]").WithMessage("Пароль должен содержать минимум один символ верхнего регистра")
				.Matches(@"[a-z]").WithMessage("Пароль должен содержать минимум один символ нижнего регистра")
				.Matches(@"[0-9]").WithMessage("Пароль должен содержать минимум одну цифру");
		RuleFor(request => request.ConfirmPassword)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(MustBeenFeeledMessage)
				.Equal(request => request.Password).WithMessage("Пароли должны совпадать");
	}
}