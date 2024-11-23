using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StatementProcessingService.Application.Interfaces.Services;
using StatementProcessingService.Application.MappingProfiles;
using StatementProcessingService.Application.Services;
using StatementProcessingService.Domain.Interfaces.Repositories;
using StatementProcessingService.Domain.Interfaces.UnitOfWork;
using StatementProcessingService.Infrastructure.Data;
using StatementProcessingService.Infrastructure.Repositories;
using StatementProcessingService.Infrastructure.UnitOfWork;
using StatementProcessingService.Presentation.Validators;

namespace StatementProcessingService.Presentation.Extensions;

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

    public static void AddMapping(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(
            typeof(BankStatementEntryProfile).Assembly,
            typeof(BankStatementFileProfile).Assembly
        );
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString("ConnectionString");
        builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });
        builder.Services.AddScoped<ApplicationDbContext>();
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IBankStatementEntryRepository, BankStatementEntryRepository>();
        builder.Services.AddScoped<IBankStatementFileRepository, BankStatementFileRepository>();
        builder.Services.AddScoped<IFileService,FileService >();
        builder.Services.AddScoped<IExcelParserService, ExcelParserService>();
        builder.Services.AddScoped<IBankStatementEntryService, BankStatementEntryService>();
        builder.Services.AddScoped<IBankStatementFileService, BankStatementFileService>();
        builder.Services.AddControllers();
    }

    public static void AddValidation(this WebApplicationBuilder builder)
    {
        builder
            .Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<ExportBankStatementRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<GetBankStatementsRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UploadBankStatementRequestValidator>();
    }
}
