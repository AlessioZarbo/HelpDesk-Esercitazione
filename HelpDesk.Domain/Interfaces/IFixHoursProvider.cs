using HelpDesk.Domain.Enums;

namespace HelpDesk.Domain.Interfaces;

/// <summary>
/// Provides a method to retrieve the number of hours associated with a given ticket priority.
/// </summary>
public interface IFixHoursProvider {

	/// <summary>
	/// Retrieves the number of hours associated with the specified ticket priority.
	/// </summary>
	/// <param name="priority"></param>
	/// <returns></returns>
	int GetHours(Priority priority);
}
