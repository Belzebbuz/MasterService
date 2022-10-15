using Domain.Common;
using Shared.Exceptions.ModelsExceptions;
using System;

namespace Domain.Models;

public class MasterService : AuditableEntity<Guid>, IAggregateRoot
{
	public string Name { get; private set; }
	public string Description { get; private set; }

	private HashSet<MasterServicePrice> _prices;
	public IReadOnlyCollection<MasterServicePrice> Prices => _prices.ToList();

	public HashSet<MasterServicePhoto> _examples;
	public IReadOnlyCollection<MasterServicePhoto> Examples => _examples.ToList();

	private MasterService()
	{
	}

	public static MasterService Create(string name, string description, decimal startCost, DateTime startPriceDateTime)
	{
		if(string.IsNullOrEmpty(name))
			throw new ArgumentNullException(nameof(name));
		if(string.IsNullOrEmpty(description))
			throw new ArgumentNullException(nameof(description));

		return new()
		{
			Name = name,
			Description = description,
			_prices = new()
			{
				new(startPriceDateTime, startCost)
			}
		};
	}

	public MasterService Update(string? name, string? description)
	{
		if (name is not null && Name?.Equals(name) is not true) Name = name;
		if (description is not null && Description?.Equals(description) is not true) Description = description;
		return this;
	}

	public void UpdatePriceValue(Guid id, decimal value)
	{
		if(_prices == null)
			throw new ObjectWasNotLoadedException(nameof(_prices));

		var price = _prices.OrderByDescending(x => x.Date).First();
		if (price == null) throw new ArgumentNullException($"Service: {Name} has no have prices");
		if (price.Id != id) throw new InvalidOperationException("You can update only last price's value");
		price.SetValue(value);
	}

	public decimal GetPrice(DateTime dateTime)
	{
		if (_prices == null)
			throw new ObjectWasNotLoadedException(nameof(_prices));

		var value = _prices!
			.Where(x => x.Date < dateTime)
			.OrderByDescending(x => x.Date)
			.FirstOrDefault()?.Value;
		if (value == null)
		{
			throw new NullServicesPriceException(Name, dateTime);
		}

		return (decimal)value;
	}

	public void AddPrice(DateTime dateTime, decimal Value)
	{
		if (_prices == null) throw new ObjectWasNotLoadedException(nameof(_prices));
		var lastPrice = _prices.OrderByDescending(x => x.Date).FirstOrDefault();
		if (lastPrice!.Value == Value && lastPrice.Date <= dateTime)
			return;

		_prices.Add(new (dateTime, Value));
	}

	public void AddExamplePhoto(string url)
	{
		if (_examples == null) throw new ObjectWasNotLoadedException(nameof(_examples));

		_examples.Add(new (url));
	}

	public void RemoveExamplePhoto(Guid id)
	{
		if (_examples == null)
			throw new ObjectWasNotLoadedException(nameof(_examples));

		var examplePhoto = _examples.SingleOrDefault(x => x.Id == id);
		if (examplePhoto == null)
			throw new ArgumentNullException($"{nameof(Examples)} of service: {Name} have no photo with id: {id}");
		
		_examples.Remove(examplePhoto);
		if (File.Exists(examplePhoto.ImagePath))
			File.Delete(examplePhoto.ImagePath);
	}

	public List<(Guid Id,string data)> GetImagesAsData()
	{
		var data = new List<(Guid Id, string data)>();
		foreach (var example in _examples)
		{
			var bytes = File.ReadAllBytes(example.ImagePath);
			var pictureData = Convert.ToBase64String(bytes, 0, bytes.Length);
			data.Add(new(example.Id, pictureData));
		}
		return data;
	}
}
