namespace HelpDesk.Infrastructure.Interfaces;

/// <summary>
/// Defines a contract for a unit of work that encapsulates the operations to be performed on the database context.
/// </summary>
public interface IUnitOfWork {

	/// <summary>
	/// Saves all changes made in the context to the database asynchronously.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task SaveChangesAsync(CancellationToken cancellationToken);
}
