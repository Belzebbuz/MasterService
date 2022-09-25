using Domain.Common;

namespace Domain.Models;

public class MasterServicePhoto : AuditableEntity<Guid>, IAggregateRoot
{
	public string ImagePath { get; private set; }

	public MasterServicePhoto(string imagePath)
	{
		ImagePath = imagePath;
	}
}
