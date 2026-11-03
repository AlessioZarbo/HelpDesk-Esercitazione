using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using HelpDesk.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HelpDesk.Infrastructure.Perstistence;

public class TicketRepository : ITicketRepository {

	/// <summary>
	/// The database context used for accessing the ticket data.
	/// </summary>
	private readonly HelpDeskDbContext _db;

	/// <summary>
	/// Initializes a new instance of the <see cref="TicketRepository"/> class with the specified database context.
	/// </summary>
	/// <param name="db">The database context to use.</param>
	public TicketRepository(HelpDeskDbContext db) {
		_db = db;
	}

	/// <summary>
	/// Gets all tickets asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains the list of all tickets.</returns>
	/// <exception cref="NotImplementedException"></exception>
	public async Task<List<Ticket>> GetAllAsync() {
		return await _db.Tickets.ToListAsync();
	}

	public Task<Ticket> GetByIdAsync(int id) {
		throw new NotImplementedException();
	}

	public Task CreateAsync(Ticket ticket) {
		throw new NotImplementedException();
	}

	public Task SetStatusAsync(string ticketId, Status status) {
		throw new NotImplementedException();
	}
}
