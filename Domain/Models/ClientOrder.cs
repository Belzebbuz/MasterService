using Domain.Common;

namespace Domain.Models;

public class ClientOrder : AuditableEntity<Guid>, IAggregateRoot
{
	public DateTime StartTime { get; private set; }
	public DateTime EndTime { get; private set; }
	public AppUser? Client { get; private set; }
	public bool Confirmed { get; private set; }
	public bool Complited { get; private set; }
	public bool Canceled { get; private set; }

	private HashSet<MasterService> _services;
	public IReadOnlyCollection<MasterService> Services => _services.ToList();

	private HashSet<OrderComment> _comments;
	public IReadOnlyCollection<OrderComment> Comments => _comments.ToList();


	private ClientOrder() { }

	public static ClientOrder CreateEmpty(DateTime startTime, DateTime endTime)
	{
		if (startTime > endTime)
			throw new ArgumentOutOfRangeException($"{nameof(StartTime)} can't be more than {nameof(EndTime)}");

		return new()
		{
			StartTime = startTime,
			EndTime = endTime
		};
	}

	public void AddComment(string value, string authorId)
	{
		if(string.IsNullOrEmpty(value))
			throw new ArgumentNullException(nameof(value));

		if(_comments == null)
			_comments = new();

		_comments.Add(new OrderComment(value, authorId));
	}

	public void ChangeCommentValue(Guid id, string value)
	{
		if (_comments == null) throw new InvalidOperationException($"{nameof(Comments)} not loaded");

		var comment = _comments.SingleOrDefault(x => x.Id == id);
		if (comment == null)
			throw new ArgumentException($"Client order:{Id}. {nameof(Comments)} have no comment with id: {id}");
		
		comment.ChangeCommentValue(value);
	}
	
	public void RemoveComment(Guid id)
	{
		if (_comments == null) throw new InvalidOperationException($"{nameof(Comments)} not loaded"); ;

		var comment = _comments.SingleOrDefault(x => x.Id == id);
		if (comment == null)
			throw new ArgumentException($"Client order:{Id}. {nameof(Comments)} have no comment with id: {id}");

		_comments.Remove(comment);
	}
}
