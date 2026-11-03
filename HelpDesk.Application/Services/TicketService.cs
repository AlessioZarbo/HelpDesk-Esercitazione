using HelpDesk.Application.Dto.Ticket;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using HelpDesk.Domain.Interfaces;
using HelpDesk.Infrastructure.Interfaces;

namespace HelpDesk.Application.Services;

public class TicketService : ITicketService {

	/// <summary>
	/// The repository used for accessing ticket data.
	/// </summary>
	private readonly ITicketRepository _repository;

	/// <summary>
	/// Initializes a new instance of the <see cref="TicketService"/> class with the specified ticket repository.
	/// </summary>
	/// <param name="repository"></param>
	public TicketService(ITicketRepository repository) {
		_repository = repository;
	}

	/// <summary>
	/// Gets all tickets asynchronously.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation. The task result contains the list of all tickets.</returns>
	public Task<IEnumerable<TicketResponseDto>> GetAllAsync() {

		return _repository.GetAllAsync().ContinueWith(t => t.Result.Select(MapToDto));
	}

	public Task<TicketResponseDto> GetByIdAsync(int id) {
		throw new NotImplementedException();
	}

	public Task CreateAsync(TicketRequestDto ticket) {
		throw new NotImplementedException();
	}

	public Task SetStatusAsync(string ticketId, Status status) {
		throw new NotImplementedException();
	}

	public static TicketResponseDto MapToDto(Ticket entity) {

		return new TicketResponseDto() {
			Id = entity.TicketId,
			Title = entity.Title,
			Description = entity.Description,
			Priority = entity.Priority,
			Status = entity.Status,
			CreatedByUserId = entity.CreatedByUserId,
			AssignedToUserId = entity.AssignedToUserId,
			CreatedAt = entity.CreatedAt,
			DueDate = entity.DueDate,

		};
	}
}
