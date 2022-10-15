using Domain.Common;

namespace Domain.Models;

public class OrderComment : AuditableEntity<Guid>
{
	public string Value { get; private set; }
	public bool IsChanged { get; private set; }
	public DateTime? ValueChangedTime { get; set; }
	public string AuthorId { get; private set; }
	public virtual AppUser Author { get; private set; }
	internal OrderComment(string value, string authorId)
	{
		Value = value;
		AuthorId = authorId;
	}

	public void ChangeCommentValue(string? value)
	{
		if (value is not null && Value?.Equals(value) is not true)
		{
			Value = value;
			IsChanged = true;
			ValueChangedTime = DateTime.Now;
		}
	}
}
