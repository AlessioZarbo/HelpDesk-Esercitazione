using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using HelpDesk.Infrastructure.Interfaces;

namespace HelpDesk.Infrastructure.Perstistence;

public class TicketRepository : ITicketRepository {
	public Task GetAllAsync() {
		throw new NotImplementedException();
	}

	public Task GetByIdAsync(int id) {
		throw new NotImplementedException();
	}

	public Task CreateAsync(Ticket ticket) {
		throw new NotImplementedException();
	}

	public Task SetStatusAsync(string ticketId, Status status) {
		throw new NotImplementedException();
	}
}
