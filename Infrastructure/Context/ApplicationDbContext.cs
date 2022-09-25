using Application.Common;
using Domain.Common;
using Domain.Models;
using Infrastructure.Audititing;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Context;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
	private readonly ICurrentUser _currentUser;
	private readonly ISerializerService _serializer;
	private readonly DatabaseSettings _dbSettings;

	public DbSet<DayTimetable> DayTimetables => Set<DayTimetable>();
	public DbSet<ClientOrder> ClientOrders => Set<ClientOrder>();
	public DbSet<OrderComment> OrderComments => Set<OrderComment>();
	public DbSet<MasterService> MasterServices => Set<MasterService>();
	public DbSet<MasterServicePhoto> MasterServicePhotos => Set<MasterServicePhoto>();
	public DbSet<MasterServicePrice> Prices => Set<MasterServicePrice>();
	public DbSet<Trail> AuditTrails => Set<Trail>();

	public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options, 
		ICurrentUser currentUser, 
		IOptions<DatabaseSettings> dbSettings,
		ISerializerService serializer) 
		: base(options)
	{
		_currentUser = currentUser;
		_serializer = serializer;
		_dbSettings = dbSettings.Value;
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.EnableSensitiveDataLogging();
		optionsBuilder.UseDatabase(_dbSettings.DBProvider!, _dbSettings.ConnectionString!);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{
		var auditEntries = HandleAuditingBeforeSaveChanges(_currentUser.GetUserId());

		int result = await base.SaveChangesAsync(cancellationToken);

		await HandleAuditingAfterSaveChangesAsync(auditEntries, cancellationToken);
		return result;
	}

	private List<AuditTrail> HandleAuditingBeforeSaveChanges(Guid userId)
	{
		foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedBy = userId;
					entry.Entity.LastModifiedBy = userId;
					break;

				case EntityState.Modified:
					entry.Entity.LastModifiedOn = DateTime.UtcNow;
					entry.Entity.LastModifiedBy = userId;
					break;

				case EntityState.Deleted:
					if (entry.Entity is ISoftDelete softDelete)
					{
						softDelete.DeletedBy = userId;
						softDelete.DeletedOn = DateTime.UtcNow;
						entry.State = EntityState.Modified;
					}

					break;
			}
		}

		ChangeTracker.DetectChanges();

		var trailEntries = new List<AuditTrail>();
		foreach (var entry in ChangeTracker.Entries<IAuditableEntity>()
					 .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
					 .ToList())
		{
			var trailEntry = new AuditTrail(entry, _serializer)
			{
				TableName = entry.Entity.GetType().Name,
				UserId = userId
			};
			trailEntries.Add(trailEntry);
			foreach (var property in entry.Properties)
			{
				if (property.IsTemporary)
				{
					trailEntry.TemporaryProperties.Add(property);
					continue;
				}

				string propertyName = property.Metadata.Name;
				if (property.Metadata.IsPrimaryKey())
				{
					trailEntry.KeyValues[propertyName] = property.CurrentValue;
					continue;
				}

				switch (entry.State)
				{
					case EntityState.Added:
						trailEntry.TrailType = TrailType.Create;
						trailEntry.NewValues[propertyName] = property.CurrentValue;
						break;

					case EntityState.Deleted:
						trailEntry.TrailType = TrailType.Delete;
						trailEntry.OldValues[propertyName] = property.OriginalValue;
						break;

					case EntityState.Modified:
						if (property.IsModified && entry.Entity is ISoftDelete && property.OriginalValue == null && property.CurrentValue != null)
						{
							trailEntry.ChangedColumns.Add(propertyName);
							trailEntry.TrailType = TrailType.Delete;
							trailEntry.OldValues[propertyName] = property.OriginalValue;
							trailEntry.NewValues[propertyName] = property.CurrentValue;
						}
						else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
						{
							trailEntry.ChangedColumns.Add(propertyName);
							trailEntry.TrailType = TrailType.Update;
							trailEntry.OldValues[propertyName] = property.OriginalValue;
							trailEntry.NewValues[propertyName] = property.CurrentValue;
						}

						break;
				}
			}
		}

		foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
		{
			AuditTrails.Add(auditEntry.ToAuditTrail());
		}

		return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
	}
	private Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries, CancellationToken cancellationToken = new())
	{
		if (trailEntries == null || trailEntries.Count == 0)
		{
			return Task.CompletedTask;
		}

		foreach (var entry in trailEntries)
		{
			foreach (var prop in entry.TemporaryProperties)
			{
				if (prop.Metadata.IsPrimaryKey())
				{
					entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
				}
				else
				{
					entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
				}
			}

			AuditTrails.Add(entry.ToAuditTrail());
		}

		return SaveChangesAsync(cancellationToken);
	}

}
