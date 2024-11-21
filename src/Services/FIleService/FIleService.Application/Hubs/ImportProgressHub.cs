using Microsoft.AspNetCore.SignalR;

namespace FIleService.Application.Hubs;

public class ImportProgressHub : Hub
{
    public async Task UpdateProgress(int processed, int total, string status)
    {
        await Clients.All.SendAsync("ReceiveProgress", processed, total, status);
    }
}