using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;

namespace HelpDesk.Infrastructure.Interfaces;

public interface ITicketRepository {

	/// <summary>
	/// Gets all tickets asynchronously
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains the list of all tickets.</returns>
	public Task<List<Ticket>> GetAllAsync();

	/// <summary>
	/// Gets a ticket by its ID asynchronously
	/// </summary>
	/// <param name="id"></param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the ticket with the specified ID.</returns>
	public Task<Ticket> GetByIdAsync(int id);

	/// <summary>
	///	Creates a new ticket asynchronously
	/// </summary>
	/// <param name="ticket"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken);

	/// <summary>
	/// Sets the status of a ticket asynchronously
	/// </summary>
	/// <param name="ticketId"></param>
	/// <param name="status"></param>
	/// <returns></returns>
	public Task SetStatusAsync(string ticketId, Status status);
}
