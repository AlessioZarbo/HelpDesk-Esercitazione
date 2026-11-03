using HelpDesk.Application.Dto.Ticket;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;

namespace HelpDesk.Domain.Interfaces;

/// <summary>
/// Interface for ticket service operations
/// </summary>
public interface ITicketService {

	/// <summary>
	/// Gets all tickets asynchronously
	/// </summary>
	/// <param name="isOperator"></param>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<TicketSimpleResponseDto>> GetAllAsync(bool isOperator, string? userId, CancellationToken cancellationToken);

	/// <summary>
	/// Gets a ticket by its ID asynchronously
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	Task<TicketSimpleResponseDto> GetByTicketIdAsync(int id);

	/// <summary>
	/// Creates a new ticket asynchronously
	/// </summary>
	/// <param name="ticket"></param>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<TicketResponseDto> CreateAsync(TicketRequestDto ticket, string? userId, CancellationToken cancellationToken);

	/// <summary>
	/// Sets the status of a ticket asynchronously
	/// </summary>
	/// <param name="ticketId"></param>
	/// <param name="status"></param>
	/// <returns></returns>
	Task SetStatusAsync(string ticketId, Status status);
}
