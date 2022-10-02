namespace Shared.Exceptions.ModelsExceptions;

public class WrongClientOrderTimeRange : Exception
{
	public WrongClientOrderTimeRange()
		:base("Неверно указан временной диапазон")
	{

	}
}
