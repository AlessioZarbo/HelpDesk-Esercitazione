using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HelpDesk.Infrastructure.Managers;

/// <summary>
/// Provides methods for managing database migrations in the HelpDesk application.
/// </summary>
public static class MigrationManager
{
	/// <summary>
	/// Applies any pending migrations to the database asynchronously.
	/// </summary>
	/// <param name="serviceProvider"></param>
	/// <param name="cancellationToken"></param>
	public static async Task ApplyMigrationsAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
	{
		using var scope = serviceProvider.CreateScope();
		var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(MigrationManager));
		try
		{
			logger.LogInformation("Applying database migrations.");
			var dbContext = scope.ServiceProvider.GetRequiredService<HelpDeskDbContext>();
			await dbContext.Database.MigrateAsync(cancellationToken);
			logger.LogInformation("Database migrations applied successfully.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while migrating the database.");
			throw;
		}
	}
}
