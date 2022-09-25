using Domain.Common;
using Shared.Exceptions.ModelsExceptions;

namespace Domain.Models;

public class MasterService : AuditableEntity<Guid>, IAggregateRoot
{
	public string Name { get; private set; }
	public List<MasterServicePrice>? Prices { get; private set; }
	public List<MasterServicePhoto>? Examples { get; set; }
	public string Description { get; private set; }

	public MasterService(string name, string description)
	{
		Name = name;
		Description = description;
	}

	public MasterService Update(string? name, string? description)
	{
		if (name is not null && Name?.Equals(name) is not true) Name = name;
		if (description is not null && Description?.Equals(description) is not true) Description = description;
		return this;
	}

	public void UpdatePriceValue(Guid id, decimal value)
	{
		var price = Prices.FirstOrDefault(x => x.Id == id);
		if (price == null)
			throw new ArgumentNullException($"{nameof(price)} is null of service: {Name} have no price with id: {id}");
		price.Value = value;
	}

	public decimal GetPrice(DateTime dateTime)
	{
		var value = Prices?
			.Where(x => x.Date < dateTime)?
			.OrderByDescending(x => x.Date)?
			.FirstOrDefault()?.Value;
		if (value == null)
		{
			throw new NullServicesPriceException(Name, dateTime);
		}

		return (decimal)value;
	}

	public void AddPrice(DateTime dateTime, decimal Value)
	{
		if (Prices == null)
			Prices = new();
		Prices.Add(new MasterServicePrice(dateTime, Value));
	}

	public void AddExamplePhoto(string url)
	{
		if(string.IsNullOrEmpty(url))
			throw new ArgumentNullException("url");

		if(Examples == null)
			Examples = new List<MasterServicePhoto>();

		Examples.Add(new MasterServicePhoto(url));
	}

	public void RemoveExamplePhoto(Guid id)
	{
		if (Examples == null)
			throw new ArgumentNullException($"{nameof(Examples)} is null of service: {Name}");

		var examplePhoto = Examples.FirstOrDefault(x => x.Id == id);
		if (examplePhoto == null)
			throw new ArgumentNullException($"{nameof(Examples)} of service: {Name} have no photo with id: {id}");
		
		Examples.Remove(examplePhoto);
	}
}
