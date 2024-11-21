using System.Data.Common;
using FIleService.Application.Interfaces;
using FIleService.Domain.Models;
using FIleService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FIleService.Application.Services;

public class StatisticsService : IStatisticsService
{
    private readonly ApplicationDbContext _context;

    public StatisticsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Statistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var connection = _context.Database.GetDbConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM calculate_statistics()"; 

        if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            return new Statistics(
                reader.GetInt64(0),   
                reader.GetDecimal(1) 
            );
        }

        return new Statistics(0, 0); 
    }
    
}