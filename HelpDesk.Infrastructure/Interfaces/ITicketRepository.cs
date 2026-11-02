using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;

namespace HelpDesk.Infrastructure.Interfaces;

public interface ITicketRepository {

	/// <summary>
	/// Gets all tickets asynchronously
	/// </summary>
	/// <returns></returns>
	public Task GetAllAsync();

	/// <summary>
	/// Gets a ticket by its ID asynchronously
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Task GetByIdAsync(int id);

	/// <summary>
	///	Creates a new ticket asynchronously
	/// </summary>
	/// <param name="ticket"></param>
	/// <returns></returns>
	public Task CreateAsync(Ticket ticket);

	/// <summary>
	/// Sets the status of a ticket asynchronously
	/// </summary>
	/// <param name="ticketId"></param>
	/// <param name="status"></param>
	/// <returns></returns>
	public Task SetStatusAsync(string ticketId, Status status);
}
