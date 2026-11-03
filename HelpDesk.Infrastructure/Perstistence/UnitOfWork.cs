using HelpDesk.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure.Perstistence;

/// <summary>
/// Represents a unit of work for managing database operations.
/// </summary>
public class UnitOfWork : IUnitOfWork {

	/// <summary>
	/// The database context used for managing database operations.
	/// </summary>
	private readonly DbContext _db;

	/// <summary>
	/// Initializes a new instance of the <see cref="UnitOfWork"/> class with the specified database context.
	/// </summary>
	/// <param name="db"></param>
	public UnitOfWork(HelpDeskDbContext db) {

		_db = db;
	}

	/// <summary>
	/// Saves changes to the database asynchronously.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task SaveChangesAsync(CancellationToken cancellationToken) => _db.SaveChangesAsync(cancellationToken);
}
