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
	/// The unit of work used for managing transactions and saving changes to the database.
	/// </summary>
	private readonly IUnitOfWork _unitOfWork;

	/// <summary>
	/// Initializes a new instance of the <see cref="TicketRepository"/> class with the specified database context.
	/// </summary>
	/// <param name="db">The database context to use.</param>
	/// <param name="unitOfWork">The unit of work to use.</param>
	public TicketRepository(HelpDeskDbContext db, IUnitOfWork unitOfWork) {

		_db = db;
		_unitOfWork = unitOfWork;
	}

	/// <summary>
	/// Gets all tickets asynchronously.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the list of all tickets.</returns>
	public async Task<List<Ticket>> GetAllAsync(CancellationToken cancellationToken) {

		return await _db.Tickets.ToListAsync(cancellationToken);
	}

	/// <summary>
	/// Gets all tickets for a specific user asynchronously.
	/// </summary>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<Ticket>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken) {

		return await _db.Tickets
			.Where(t => t.CreatedByUserId == userId)
			.ToListAsync(cancellationToken);
	}

	public Task<Ticket> GetByIdAsync(int id) {
		throw new NotImplementedException();
	}

	/// <summary>
	/// Creates a new ticket asynchronously.
	/// </summary>
	/// <param name="ticket"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken) {
		_db.Add(ticket);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return await Task.FromResult(ticket);
	}

	public Task SetStatusAsync(string ticketId, Status status) {
		throw new NotImplementedException();
	}
}
