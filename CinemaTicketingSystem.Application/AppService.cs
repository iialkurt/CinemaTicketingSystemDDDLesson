using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;

namespace CinemaTicketingSystem.Application
{
    public class AppService(ILocalizer localizer) : IScopedDependency
    {


        public string L(string key)
        {
            return localizer.L(key);
        }
        public string L(string key, params object[] data)
        {
            return localizer.L(key, data);
        }

    }
}
