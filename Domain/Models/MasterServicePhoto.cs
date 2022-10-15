using Domain.Common;

namespace Domain.Models;

public class MasterServicePhoto : AuditableEntity<Guid>
{
	public string ImagePath { get; private set; }

	internal MasterServicePhoto(string imagePath)
	{
		ImagePath = imagePath;
	}
}
