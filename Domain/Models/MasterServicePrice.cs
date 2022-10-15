using Domain.Common;

namespace Domain.Models;

public class MasterServicePrice : AuditableEntity<Guid>
{
	public DateTime Date { get; private set; }
	public decimal Value { get; private set; }
	internal MasterServicePrice(DateTime date, decimal value)
	{
		Date = date;
		Value = value;
	}

	public void SetValue(decimal value)
	{
		Value = value;
	}
}