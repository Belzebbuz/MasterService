using Domain.Common;
using Shared.Exceptions.ModelsExceptions;

namespace Domain.Models;

public class DayTimetable : AuditableEntity<Guid>, IAggregateRoot
{
	public DateTime StartWorkTime { get; private set; }
	public DateTime EndWorkTime { get; private set; }
	private HashSet<ClientOrder> _orders;
	public IReadOnlyCollection<ClientOrder> ClientOrders => _orders.ToList();

	private DayTimetable(){}

	public static DayTimetable Create(DateTime startWorkTime, DateTime endWorkTime, bool fillDefaultOrders, double interval = 2)
	{
		if (startWorkTime > endWorkTime) throw new ArgumentOutOfRangeException($"{nameof(StartWorkTime)} can't be greater than {nameof(EndWorkTime)}");
		return new()
		{
			StartWorkTime = startWorkTime,
			EndWorkTime = endWorkTime,
			_orders = fillDefaultOrders ? FillEmptySchedule(startWorkTime, endWorkTime, interval) : new()
		};
	}

	private static HashSet<ClientOrder> FillEmptySchedule(DateTime startWorkTime, DateTime endWorkTime, double interval)
	{
		var defaultOrdersSet = new	HashSet<ClientOrder>();
		DateTime newOrderTime = startWorkTime;
		var endOrderTime = newOrderTime.AddHours(interval);

		while (endOrderTime <= endWorkTime)
		{
			defaultOrdersSet.Add(ClientOrder.CreateEmpty(newOrderTime, endOrderTime));
			newOrderTime = endOrderTime;
			endOrderTime = newOrderTime.AddHours(interval);
		}
		return defaultOrdersSet;
	}

	public void AddEmptyClientOrder(DateTime startTime, DateTime endTime)
	{
		if (_orders == null)
			throw new InvalidOperationException("Orders not loaded");

		if(_orders.Any(x => x.StartTime < endTime && startTime < x.EndTime))
			throw new WrongClientOrderTimeRange();

		_orders.Add(ClientOrder.CreateEmpty(startTime, endTime));
	}
}
