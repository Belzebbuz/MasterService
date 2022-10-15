using FluentValidation;
using MediatR;
using Shared.Wrapper;

namespace Shared.Messages.Master;

/// <summary>
/// Запрос на обновление данных услуги
/// </summary>
public class MSM_005 : MasterServiceUpdateRequest, IRequest<IResult>
{
	public Guid ServiceId { get; set; }
}

public class MSM_005_Validator : AbstractValidator<MSM_005>
{
	private const string _mustBeenFeeledMessage = "Поле должно быть заполнено!";
	public MSM_005_Validator()
	{
		RuleFor(x => x.Name)
			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(_mustBeenFeeledMessage);
		RuleFor(x => x.Description)
			.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(_mustBeenFeeledMessage);
		RuleFor(x => x.NewPriceDateTime)
			.NotNull().WithMessage(_mustBeenFeeledMessage);
	}
}