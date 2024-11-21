using FIleService.Domain.Models;

namespace FIleService.Application.Interfaces;

public interface IStatisticsService
{
    Task<Statistics> GetStatisticsAsync(CancellationToken cancellationToken = default);
}