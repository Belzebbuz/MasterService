using Domain.Models;
using Newtonsoft.Json.Bson;
using NUnit.Framework;

namespace Domain.Tests;

public class DayTimetableTests
{
	[Test]
	[TestCase(9,18,1,9)]
	[TestCase(9,18,1.5,6)]
	[TestCase(9,18.5,1.5,6)]
	public void SetIntervalShoudCreateExpectedCountOrders(double startDate, double endDate, double interval, int expectedOrders)
	{
		var startWorkTime = DateTime.Parse("Oct 25, 2022").AddHours(startDate);
		var endWorkTime = DateTime.Parse("Oct 25, 2022").AddHours(endDate);
		var timetable = new DayTimetable(startWorkTime, endWorkTime);
		timetable.FillEmptySchedule(interval);
		Assert.IsTrue(timetable.Schedule.Count() == expectedOrders);
	}

}
