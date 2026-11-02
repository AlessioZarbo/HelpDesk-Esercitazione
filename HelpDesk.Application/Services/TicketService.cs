using HelpDesk.Application.Dto.Ticket;
using HelpDesk.Domain.Enums;
using HelpDesk.Domain.Interfaces;

namespace HelpDesk.Application.Services;

public class TicketService : ITicketService {

	public Task<IEnumerable<TicketResponseDto>> GetAllAsync() {
		throw new NotImplementedException();
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
}
