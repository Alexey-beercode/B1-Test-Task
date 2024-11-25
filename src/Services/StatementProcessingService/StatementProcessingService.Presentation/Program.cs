using System.Text;
using StatementProcessingService.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabase();
builder.AddMapping();
builder.AddServices();
builder.AddValidation();
builder.AddSwaggerDocumentation();
System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var app = builder.Build();

app.AddSwagger();
app.AddApplicationMiddleware();

app.Run();