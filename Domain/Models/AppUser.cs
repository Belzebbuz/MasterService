using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Models;

public class AppUser : IdentityUser
{
	public string FullName { get; set; } = default!;
	public bool IsActive { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime RefreshTokenExpiryTime { get; set; }
	public long? TelegramUserId { get; set; }
	public long? TelegramChatBotId { get; set; }
	public List<DayTimetable>? Timetable { get; set; }
	public List<MasterService>? Services { get; set; }
	public List<ClientOrder>? Orders { get; set; }
	public List<OrderComment>? OrderComments { get; set; }
}
