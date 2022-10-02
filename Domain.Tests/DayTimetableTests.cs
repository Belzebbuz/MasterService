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
		Assert.NotNull(timetable.Schedule);
		Assert.IsTrue(timetable.Schedule.Count() == expectedOrders);
	}

	[Test]
	public void ShouldNotOverrideWithOverlappingTime()
	{
		var startWorkTime = DateTime.Parse("Oct 25, 2022").AddHours(9);
		var endWorkTime = DateTime.Parse("Oct 25, 2022").AddHours(18);
		var timetable = new DayTimetable(startWorkTime, endWorkTime);

		timetable.AddEmptyClientOrder(startWorkTime, startWorkTime.AddHours(1));
		Assert.NotNull(timetable.Schedule);
		Assert.IsTrue(timetable.Schedule.Count == 1);

		timetable.AddEmptyClientOrder(startWorkTime.AddHours(1), startWorkTime.AddHours(2));
		Assert.IsTrue(timetable.Schedule.Count == 2);

		Assert.That(() => timetable.AddEmptyClientOrder(startWorkTime, startWorkTime.AddHours(1.5)), Throws.Exception);
		Assert.That(() => timetable.AddEmptyClientOrder(startWorkTime.AddHours(1), startWorkTime.AddHours(1.5)), Throws.Exception);
		Assert.That(() => timetable.AddEmptyClientOrder(startWorkTime, startWorkTime.AddHours(3)), Throws.Exception);
		Assert.That(() => timetable.AddEmptyClientOrder(startWorkTime.AddHours(-1), startWorkTime.AddHours(0.5)), Throws.Exception);

		timetable.AddEmptyClientOrder(startWorkTime.AddHours(2), startWorkTime.AddHours(3));
		Assert.IsTrue(timetable.Schedule.Count == 3);
		Assert.IsTrue(timetable.Schedule.Last().StartTime == startWorkTime.AddHours(2));
		Assert.IsTrue(timetable.Schedule.Last().EndTime == startWorkTime.AddHours(3));
	}
}
