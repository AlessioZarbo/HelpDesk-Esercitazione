using HelpDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure;

/// <summary>
/// Represents the database context for the HelpDesk application, providing access to the Tickets table and managing database operations.
/// </summary>
public class HelpDeskDbContext : DbContext {

	/// <summary>
	/// Initializes a new instance of the <see cref="HelpDeskDbContext"/> class with the specified options.
	/// </summary>
	/// <param name="options"></param>
	public HelpDeskDbContext(DbContextOptions<HelpDeskDbContext> options) : base(options) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="HelpDeskDbContext"/> class.
	/// </summary>
	public HelpDeskDbContext() { }

	/// <summary>
	/// Gets or sets the DbSet representing the Tickets table in the database.
	/// </summary>
	public DbSet<Ticket> Tickets { get; set; }
}
