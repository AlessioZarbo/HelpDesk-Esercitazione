using HelpDesk.Application.Mapper;
using HelpDesk.Application.Services;
using HelpDesk.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.Application;

public static class DependencyInjection {

	public static IServiceCollection AddApplication(
		this IServiceCollection services,
		IConfiguration configuration) {

		services.AddScoped<ITicketService, TicketService>();
		services.AddScoped<TicketMapper>();

		return services;
	}
}
