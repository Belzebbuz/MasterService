namespace Shared.Messages.Master;

public class MasterServiceResponse
{
	public MasterServiceResponse(Guid serviceId, string name, List<ImageDataResponse> images, string description, decimal currentPrice)
	{
		ServiceId = serviceId;
		Name = name;
		Images = images;
		Description = description;
		CurrentPrice = currentPrice;
	}

	public Guid ServiceId { get; private set; }
	public string Name { get; private set; }
	public List<ImageDataResponse> Images { get; private set; }
	public string Description { get; private set; }
	public decimal CurrentPrice { get; private set; }
}
public class ImageDataResponse
{
	public ImageDataResponse(Guid id, string data)
	{
		Id = id;
		Data = data;
	}

	public Guid Id { get; private set; }
	public string Data { get; private set; }
}