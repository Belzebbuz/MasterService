using Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Domain.Models;

public class AppUser : IdentityUser, IAggregateRoot
{
	public string FullName { get; private set; }
	public bool IsActive { get; private set; }
	public string? RefreshToken { get; private set; }
	public DateTime? RefreshTokenExpiryTime { get; private set; }
	public long? TelegramUserId { get; private set; }
	public long? TelegramChatBotId { get; private set; }

	[Column(TypeName = "text")]
	public string? ProfilePictureDataUrl { get; private set; }

	private HashSet<DayTimetable> _timetable;
	public IReadOnlyCollection<DayTimetable> Timetable => _timetable.ToList();

	private HashSet<MasterService> _services;
	public IReadOnlyCollection<MasterService> Services => _services.ToList();

	private HashSet<ClientOrder> _orders;
	public IReadOnlyCollection<ClientOrder> Orders => _orders.ToList();

	private AppUser()
	{
	}
	public static AppUser Create(string fullName, string email, string? phoneNumber)
	{
		if (string.IsNullOrEmpty(fullName)) throw new ArgumentNullException(nameof(fullName));
		if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

		return new()
		{
			FullName = fullName,
			Email = email,
			UserName = email,
			PhoneNumber = phoneNumber,
			IsActive = true
		};
	}

	public void Update(string fullName, string phoneNumber)
	{
		if(string.IsNullOrEmpty(fullName)) throw new ArgumentNullException(nameof(fullName));

		FullName = fullName;
		PhoneNumber = phoneNumber;
	}

	public void SetTelegramData(long tgUserId, long tgChatId)
	{
		TelegramUserId = tgUserId; 
		TelegramChatBotId = tgChatId;
	}

	public void SetProfilePictureFilePath(string path)
	{
		ProfilePictureDataUrl = path;
	}

	public void SetRefreshToken(string refreshToken, DateTime expiryTime)
	{
		if(string.IsNullOrEmpty(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));
		RefreshToken = refreshToken;
		RefreshTokenExpiryTime = expiryTime;
	}

	public void SetIsActiveStatus(bool activateUser)
	{
		IsActive = activateUser;
	}
	
	public void AddNewMasterService(MasterService masterService)
	{
		if(masterService == null) throw new ArgumentNullException(nameof(masterService));
		if(_services == null) throw new InvalidOperationException("Services not loaded");

		if (_services.Any(x => x.Name == masterService.Name)) 
			throw new InvalidOperationException($"Serives already has service with name: {masterService.Name}");

		_services.Add(masterService);
	}

	public void AddNewTimetable(DayTimetable timetable)
	{
		if (timetable == null) throw new ArgumentNullException(nameof(timetable));
		if (_timetable == null) throw new InvalidOperationException("Timetable not loaded");

		if (_timetable.Any(x => x.StartWorkTime < timetable.EndWorkTime && timetable.StartWorkTime < x.EndWorkTime))
			throw new InvalidOperationException("This day already has timetable");

		_timetable.Add(timetable);
	}
}
