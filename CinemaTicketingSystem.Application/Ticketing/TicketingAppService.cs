using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Domain.Ticketing.Repositories;
using CinemaTicketingSystem.Domain.Ticketing.Tickets;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Application.Ticketing;

public class TicketingAppService(AppDependencyService appDependencyService,ITicketPurchaseRepository ticketPurchaseRepository,IUserContext userContext) : IScopedDependency, ITicketingAppService
{
    
    public async Task<AppResult> PurchaseTicket(PurchaseTicketRequest request)
    {

        var ticketPurchaseList = ticketPurchaseRepository.GetTicketsPurchaseByScheduleId(request.ScheduleId);
        
        // hallId sahip hall toplam kaç kişi alıyor
        // seçilen koltuk ilgili hall'de bulunuyor mu?
        // seçilen koltuk daha önce satın alınmış mı?
        //cinema name,hall name,show time bilgileri lazım (scheduleId üzerinden alınabilir)
     
        
        
        
        var ticket = new TicketPurchase(request.ScheduleId,userContext.UserId);

       // await ticketPurchaseRepository.AddAsync(ticket);

        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }
    
    
    
    
    
}

public interface ITicketingAppService


{
}
