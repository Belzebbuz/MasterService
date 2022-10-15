using FluentValidation;
using MediatR;
using Shared.Wrapper;

namespace Shared.Messages.Master;
/// <summary>
/// Запрос на создание услуги мастера
/// </summary>
public class MSM_002 : MasterServiceUpdateRequest, IRequest<IResult>
{
	public string UserId { get; set; }
}

public class MSM_002_Validator : AbstractValidator<MSM_002>
{
	private const string _mustBeenFeeledMessage = "Поле должно быть заполнено!";
	public MSM_002_Validator()
	{
		RuleFor(x => x.Name)
			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(_mustBeenFeeledMessage);
		RuleFor(x => x.Description)
			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(_mustBeenFeeledMessage);
		RuleFor(x => x.NewPriceDateTime)
			.NotNull().WithMessage(_mustBeenFeeledMessage);
	}
}