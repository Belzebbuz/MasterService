using Domain.Common;

namespace Domain.Models;

public class DayTimetable : AuditableEntity<Guid>, IAggregateRoot
{
	public DateTime StartWorkTime { get; private set; }
	public DateTime EndWorkTime { get; private set; }
	public List<ClientOrder>? Schedule { get; private set; }

	public DayTimetable(DateTime startWorkTime, DateTime endWorkTime)
	{
		StartWorkTime = startWorkTime;
		EndWorkTime = endWorkTime;
	}
	public List<ClientOrder> FillEmptySchedule(double interval)
	{
		Schedule = new List<ClientOrder>();
		DateTime newOrderTime = StartWorkTime;
		var endOrderTime = newOrderTime.AddHours(interval);

		while (endOrderTime <= EndWorkTime)
		{
			Schedule.Add(new ClientOrder(newOrderTime, endOrderTime));
			newOrderTime = endOrderTime;
			endOrderTime = newOrderTime.AddHours(interval);
		}
		return Schedule;
	}
}
