using FIleService.Application.Hubs;
using FIleService.Presentation.Middleware;

namespace FIleService.Presentation.Extensions;

public static class WebApplicationExtension
{
    public static void AddSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    public static void AddApplicationMiddleware(this WebApplication app)
    {
        app.MapHub<ImportProgressHub>("/importProgressHub");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(builder =>
        {
            builder
                .WithOrigins("http://localhost:4200") // укажите URL вашего Angular приложения
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(host => true); 
        });
        app.MapControllers();

        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}