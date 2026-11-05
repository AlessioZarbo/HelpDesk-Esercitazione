using System.Text.Json.Serialization;
using FluentValidation;
using HelpDesk.API.Middlewares;
using HelpDesk.API.Providers;
using HelpDesk.Application;
using HelpDesk.Application.Validators;
using HelpDesk.Auth;
using HelpDesk.Domain.Interfaces;
using HelpDesk.Infrastructure;
using HelpDesk.Infrastructure.Managers;
using Scalar.AspNetCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

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


builder.Services.AddValidatorsFromAssemblyContaining<TicketRequestDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Logging
	.ClearProviders()
	.AddSimpleConsole(options => {
		options.IncludeScopes = true;
		options.SingleLine = true;
		options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
	});

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
	app.MapOpenApi();
}

await MigrationManager.ApplyMigrationsAsync(app.Services);

app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.MapScalarApiReference();
app.Run();
