using FIleService.Application.Interfaces;
using FIleService.Application.Services;
using FIleService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FIleService.Presentation.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddSwaggerDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Description = @"Enter JWT Token please.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                }
            );
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        new List<string>()
                    }
                }
            );
        });
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString("ConnectionString");
        builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });
        builder.Services.AddScoped<ApplicationDbContext>();
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IFileGeneratorService, FileGeneratorService>();
        builder.Services.AddScoped<IFileMergeService, FileMergeService>();
        builder.Services.AddScoped<IStatisticsService, StatisticsService>();
        builder.Services.AddScoped<IFileImportService, FileImportService>();
        builder.Services.AddSignalR();
        builder.Services.AddControllers();
    }
    
}
