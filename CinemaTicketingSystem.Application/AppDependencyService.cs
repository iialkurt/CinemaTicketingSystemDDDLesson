using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Application;

public class AppDependencyService(IUnitOfWork unitOfWork, ILocalizer localizer, ILocalizeErrorService localizeErrorService, IUserContext userContext)
{
    public IUnitOfWork UnitOfWork => unitOfWork;
    public IUserContext UserContext => userContext;
    public ILocalizer L => localizer;
    public ILocalizeErrorService LocalizeError => localizeErrorService;




}