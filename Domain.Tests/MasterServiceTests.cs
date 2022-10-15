using Domain.Models;

namespace Domain.Tests
{
	public class Tests
	{
		private List<MasterService> _masterServices;
		[SetUp]
		public void Setup()
		{
			_masterServices = new()
			{
				MasterService.Create("Service #1", "short description", 1000M, DateTime.Now.AddDays(-1)),
				MasterService.Create("Service #2", "short description", 1200M, DateTime.Now.AddDays(-1)),
				MasterService.Create("Service #3", "short description", 1400M, DateTime.Now.AddDays(-1)),
			};
		}

		[Test]
		public void GetPriceShouldNotBeNull()
		{
			var service = _masterServices.FirstOrDefault(x => x.Name == "Service #1");
			service!.AddPrice(DateTime.Now, 1500M);
			var actualPrice = service?.GetPrice(DateTime.Now);
			Assert.IsNotNull(actualPrice);
			Assert.IsTrue(actualPrice.HasValue);
			Assert.That(actualPrice.Value, Is.EqualTo(1500M));
		}

		[Test]
		public void AfterAddPriceGetPriceShouldBeActual()
		{
			var service = _masterServices.FirstOrDefault(x => x.Name == "Service #1");
			service.AddPrice(DateTime.Now, 1500M);
			service.AddPrice(DateTime.Now.AddDays(1), 2000M);
			var featurePrice = service?.GetPrice(DateTime.Now.AddDays(2));
			Assert.IsNotNull(featurePrice);
			Assert.IsTrue(featurePrice.HasValue);
			Assert.That(featurePrice.Value, Is.EqualTo(2000M));

			var actualPrice = service?.GetPrice(DateTime.Now);
			Assert.IsNotNull(actualPrice);
			Assert.IsTrue(actualPrice.HasValue);
			Assert.That(actualPrice.Value, Is.EqualTo(1500M));
		}
	}
}