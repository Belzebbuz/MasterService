namespace Shared.Messages.Master;

public class MSR_004 
{
	public MasterServiceResponse MasterService { get; private set; }

	public MSR_004(MasterServiceResponse masterService)
	{
		if (masterService == null) throw new ArgumentNullException(nameof(masterService));
		MasterService = masterService;
	}
}
