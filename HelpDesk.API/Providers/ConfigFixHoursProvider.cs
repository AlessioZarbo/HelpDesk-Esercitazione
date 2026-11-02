using HelpDesk.Domain.Enums;
using HelpDesk.Domain.Interfaces;

namespace HelpDesk.API.Providers;

/// <summary>
/// Provides the number of hours to fix a ticket based on its priority, using configuration settings.
/// </summary>
public class ConfigFixHoursProvider : IFixHoursProvider {

	/// <summary>
	/// Gets the configuration instance used to retrieve fix hours for different priorities.
	/// </summary>
	private readonly IConfiguration _configuration;

	/// <summary>
	/// Initializes a new instance of the <see cref="ConfigFixHoursProvider"/> class with the specified configuration.
	/// </summary>
	/// <param name="configuration"></param>
	public ConfigFixHoursProvider(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	/// <summary>
	/// Gets the number of hours to fix a ticket based on its priority.
	/// </summary>
	/// <param name="priority"></param>
	/// <returns></returns>
	public int GetHours(Priority priority)
	{
		var key = $"FixHours:{priority}";
		var hours = _configuration.GetValue<int?>(key);

		if (hours is null)
			throw new InvalidOperationException($"Configurazione mancante: '{key}' in appsettings.json.");

		if (hours <= 0)
			throw new InvalidOperationException($"Il valore di '{key}' deve essere maggiore di zero.");

		return hours.Value;
	}
}
