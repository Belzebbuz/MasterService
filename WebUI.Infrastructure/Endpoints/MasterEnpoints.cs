namespace WebUI.Infrastructure.Endpoints;

public class MasterEnpoints
{
	public const string MasterService = "api/master";
	public const string ServicePicture = "api/master/service-picture";
	public static string GetAllServices(string userId)
	{
		return $"api/master/all/{userId}";
	}

	public static string GetSingleService(string serviceId)
	{
		return $"api/master/single/{serviceId}";
	}

	public static string DeleteServicePicture(string serviceId, string picId)
	{
		return $"api/master/service-picture/{serviceId}/{picId}";
	}
}
