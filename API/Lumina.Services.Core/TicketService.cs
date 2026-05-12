using AutoMapper;
using Lumina.Data.Models;
using Lumina.Data.Repository;
using Lumina.Data.Repository.Interfaces;
using Lumina.Service.Common.DTO_s.Output.Ticket;
using Lumina.Services.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IScreeningRepository _screeningRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    public TicketService(ITicketRepository ticketRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IScreeningRepository screeningRepository)
    {
        this._ticketRepository = ticketRepository;
        this._screeningRepository = screeningRepository;
        this._mapper = mapper;
        this._userManager = userManager;
    }

    public async Task<Ticket> ExistTicket(Guid userId, Guid screeeningId)
    {
        return await this._ticketRepository
         .FirstOrDefaultAsync(t => t.UserId == userId && t.ScreeningId == screeeningId);
    }


    public async Task<IEnumerable<ReadTicketDto>> GetAllTicketsAsync(Guid userId)
    {
        IEnumerable<ReadTicketDto> userTickets = this._ticketRepository
         .GetAllAttached()
         .Where(t => t.UserId == userId)
         .Select(t => new ReadTicketDto
         {
             CinemaName = t.ScreeningProjection.Cinema.Name,
             ScreeningMovieName = t.ScreeningProjection.Movie.Title,
             Showtime = t.ScreeningProjection.Showtime,
             Price = t.Price,
             Count = t.Count
         })
         .ToList();
        return userTickets;
    }

    public async Task<bool> BuyAsync(Guid userId, Guid screeningId, int ticketsCount)
    {
        Screening screening = await this._screeningRepository.FirstOrDefaultAsync(s => s.Id == screeningId);
        if (screening == null)
            return false;

        Ticket existTicketInUserTickets = await this.ExistTicket(userId, screening.Id);
        if (existTicketInUserTickets != null)
        {
            if (screening.AvailableTickets >= ticketsCount)
            {
                screening.AvailableTickets -= ticketsCount;
                existTicketInUserTickets.Count += ticketsCount;
                await this._ticketRepository.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
        if (screening.AvailableTickets < ticketsCount)
            return false;
        screening.AvailableTickets -= ticketsCount;
        Ticket newTicket = new Ticket
        {
            Id = Guid.NewGuid(),
            Price = 7,
            ScreeningId = screening.Id,
            UserId = userId,
            Count = ticketsCount
        };

        await this._ticketRepository.AddAsync(newTicket);
        return true;
    }
}
