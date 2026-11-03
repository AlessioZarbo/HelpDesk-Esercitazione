using HelpDesk.API.Providers;
using HelpDesk.Application;
using HelpDesk.Auth;
using HelpDesk.Domain.Interfaces;
using HelpDesk.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
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
