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
		var timetable = DayTimetable.Create(startWorkTime, endWorkTime, true, interval);
		Assert.NotNull(timetable.ClientOrders);
		Assert.IsTrue(timetable.ClientOrders.Count() == expectedOrders);
	}

	[Test]
	public void ShouldNotOverrideWithOverlappingTime()
	{
		var startWorkTime = DateTime.Parse("Oct 25, 2022").AddHours(9);
		var endWorkTime = DateTime.Parse("Oct 25, 2022").AddHours(18);
		var timetable = DayTimetable.Create(startWorkTime, endWorkTime, false);
		timetable.AddEmptyClientOrder(startWorkTime, startWorkTime.AddHours(1));
		Assert.NotNull(timetable.ClientOrders);
		Assert.IsTrue(timetable.ClientOrders.Count == 1);

		timetable.AddEmptyClientOrder(startWorkTime.AddHours(1), startWorkTime.AddHours(2));
		Assert.IsTrue(timetable.ClientOrders.Count == 2);

		Assert.That(() => timetable.AddEmptyClientOrder(startWorkTime, startWorkTime.AddHours(1.5)), Throws.Exception);
		Assert.That(() => timetable.AddEmptyClientOrder(startWorkTime.AddHours(1), startWorkTime.AddHours(1.5)), Throws.Exception);
		Assert.That(() => timetable.AddEmptyClientOrder(startWorkTime, startWorkTime.AddHours(3)), Throws.Exception);
		Assert.That(() => timetable.AddEmptyClientOrder(startWorkTime.AddHours(-1), startWorkTime.AddHours(0.5)), Throws.Exception);

		timetable.AddEmptyClientOrder(startWorkTime.AddHours(2), startWorkTime.AddHours(3));
		Assert.IsTrue(timetable.ClientOrders.Count == 3);
		Assert.IsTrue(timetable.ClientOrders.Last().StartTime == startWorkTime.AddHours(2));
		Assert.IsTrue(timetable.ClientOrders.Last().EndTime == startWorkTime.AddHours(3));
	}
}
