using HelpDesk.API.Providers;
using HelpDesk.Auth;
using HelpDesk.Domain.Interfaces;
using HelpDesk.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHelpDeskJwtAuth(builder.Configuration);
builder.Services.AddSingleton<IFixHoursProvider, ConfigFixHoursProvider>();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
