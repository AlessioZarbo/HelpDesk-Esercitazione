using HelpDesk.Infrastructure.Interfaces;
using HelpDesk.Infrastructure.Perstistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.Infrastructure;

public static class DependencyInjection {

	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration) {

		services.AddScoped<ITicketRepository, TicketRepository>();

		services.AddSqlServer<HelpDeskDbContext>(
			configuration.GetConnectionString("Docker"));

		return services;
	}
}
