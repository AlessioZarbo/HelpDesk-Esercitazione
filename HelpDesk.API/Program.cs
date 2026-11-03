using System.Text.Json.Serialization;
using HelpDesk.API.Providers;
using HelpDesk.Application;
using HelpDesk.Auth;
using HelpDesk.Domain.Interfaces;
using HelpDesk.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Permette di popolare il campo dell'enum in formato string
builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.Converters.Add(
		new JsonStringEnumConverter(allowIntegerValues: false));
});
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(
			new JsonStringEnumConverter(allowIntegerValues: false));
	});

builder.Services.AddOpenApi();

builder.Services.AddHelpDeskJwtAuth(builder.Configuration);
builder.Services.AddSingleton<IFixHoursProvider, FixHoursProvider>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapScalarApiReference();
app.Run();
