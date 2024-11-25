using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace FIleService.Application.Hubs;

public class ImportProgressHub : Hub
{
    private readonly ILogger<ImportProgressHub> _logger;

    public ImportProgressHub(ILogger<ImportProgressHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation($"Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation($"Client disconnected: {Context.ConnectionId}. Exception: {exception?.Message}");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendProgress(int processed, int total, string status)
    {
        _logger.LogInformation($"Sending progress: {processed}/{total} - {status}");
        await Clients.All.SendAsync("ReceiveProgress", processed, total, status);
    }
}