namespace Shared.Messages.Master;

/// <summary>
/// Список всех услуг мастера
/// </summary>
public class MSR_001
{
	public MSR_001(List<MasterServiceResponse> services)
	{
		if (services == null) throw new ArgumentNullException(nameof(services));
		Services = services;
	}

	public List<MasterServiceResponse> Services { get; private set; }
}

