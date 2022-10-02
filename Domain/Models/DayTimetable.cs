using Domain.Common;
using Shared.Exceptions.ModelsExceptions;

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
			Schedule.Add(new (newOrderTime, endOrderTime));
			newOrderTime = endOrderTime;
			endOrderTime = newOrderTime.AddHours(interval);
		}
		return Schedule;
	}

	public void AddEmptyClientOrder(DateTime startTime, DateTime endTime)
	{
		if (Schedule == null)
			Schedule = new ();

		if(Schedule.Any(x => x.StartTime < endTime && startTime < x.EndTime))
			throw new WrongClientOrderTimeRange();

		Schedule.Add(new(startTime, endTime));
	}
}
