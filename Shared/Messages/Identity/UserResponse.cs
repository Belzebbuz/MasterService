namespace Shared.Messages.Identity;

public class UserResponse
{
	public string Id { get; set; }
	public string FullName { get; set; }
	public string Email { get; set; }
	public string? PhoneNumber { get; set; }
	public long? TelegramUserId { get; set; }
	public long? TelegramChatId { get; set; }
}
