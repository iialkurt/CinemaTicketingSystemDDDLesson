using CinemaTicketingSystem.Domain.Accounts.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Accounts
{
    internal class RefreshToken : Entity<Guid>
    {

        public string Token { get; set; } = default!;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public UserId UserId { get; set; }


        protected RefreshToken() { }

        public RefreshToken(string token, DateTime expires, UserId userId)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
        }


    }
}
