using Microsoft.AspNetCore.Components;
using Shared.Messages.Master;
using System;
using System.Threading.Tasks;

namespace WebUI.Client.Pages.Services;

public partial class MasterServiceCard
{
    [Parameter] public MasterServiceResponse Service { get; set; }

    public void GoToMasterServiceProfileAsync()
    {
        _navigationManager.NavigateTo($"/service-profile/{Service.ServiceId}");
    }
}