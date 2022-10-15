namespace Shared.Messages.Master;

public class MasterServiceUpdateRequest
{
	public string Name { get; set; }
	public string Description { get; set; }
	public DateTime? NewPriceDateTime { get; set; } = DateTime.Today;
	public decimal NewPriceValue { get; set; }
}
