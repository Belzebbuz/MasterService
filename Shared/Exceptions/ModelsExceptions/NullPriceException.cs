namespace Shared.Exceptions.ModelsExceptions;

public class NullServicesPriceException : Exception
{
	public NullServicesPriceException(string? name, DateTime dateTime) 
		: base($"На дату: {dateTime} цена для услуги: {name} цена не установлена")
	{
	}
}
