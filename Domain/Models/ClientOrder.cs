using Domain.Common;

namespace Domain.Models;

public class ClientOrder : AuditableEntity<Guid>, IAggregateRoot
{
	public DateTime StartTime { get; private set; }
	public DateTime EndTime { get; private set; }
	public List<MasterService>? Services { get; set; }
	public AppUser? Client { get; set; }
	public bool Confirmed { get; set; }
	public bool Complited { get; set; }
	public bool Canceled { get; set; }
	public List<OrderComment>? Comments { get; set; }
	public ClientOrder(DateTime startTime, DateTime endTime)
	{
		if (startTime > endTime)
			throw new ArgumentOutOfRangeException($"{nameof(StartTime)} can't be more than {nameof(EndTime)}");

		StartTime = startTime;
		EndTime = endTime;
	}

	public void AddComment(string value, string authorId)
	{
		if(string.IsNullOrEmpty(value))
			throw new ArgumentNullException(nameof(value));

		if(Comments == null)
			Comments = new List<OrderComment>();

		Comments.Add(new OrderComment(value, authorId));
	}

	public void ChangeCommentValue(Guid id, string value)
	{
		if (Comments == null)
			throw new ArgumentNullException(nameof(Comments));

		var comment = Comments.FirstOrDefault(x => x.Id == id);
		if (comment == null)
			throw new ArgumentException($"Client order:{Id}. {nameof(Comments)} have no comment with id: {id}");
		
		comment.ChangeCommentValue(value);
	}
	
	public void RemoveComment(Guid id)
	{
		if (Comments == null)
			throw new ArgumentNullException(nameof(Comments));

		var comment = Comments.FirstOrDefault(x => x.Id == id);
		if (comment == null)
			throw new ArgumentException($"Client order:{Id}. {nameof(Comments)} have no comment with id: {id}");

		Comments.Remove(comment);
	}
}
