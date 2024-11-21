using FIleService.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDatabase();
builder.AddServices();
builder.AddSwaggerDocumentation();

var app = builder.Build();

app.AddSwagger();
app.AddApplicationMiddleware();
app.Run();