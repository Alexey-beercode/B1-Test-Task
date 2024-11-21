using FIleService.Application.Interfaces;
using FIleService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FIleService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public async Task<ActionResult<Statistics>> GetStatistics(CancellationToken cancellationToken)
    {
        return Ok(await _statisticsService.GetStatisticsAsync(cancellationToken));
    }
}