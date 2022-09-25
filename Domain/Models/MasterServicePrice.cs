using Domain.Common;

namespace Domain.Models;

public class MasterServicePrice : AuditableEntity<Guid>, IAggregateRoot
{
	public DateTime Date { get; private set; }
	public decimal Value { get; set; }
	public MasterServicePrice(DateTime date, decimal value)
	{
		Date = date;
		Value = value;
	}

	public static explicit operator MasterServicePrice(decimal value) => new MasterServicePrice(DateTime.MinValue, value);
}