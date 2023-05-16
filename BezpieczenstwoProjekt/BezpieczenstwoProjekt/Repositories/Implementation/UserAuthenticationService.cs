using BezpieczenstwoProjekt.Models.Dto;
using BezpieczenstwoProjekt.Repositories.Abstract;

namespace BezpieczenstwoProjekt.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        public Task<Status> LoginAsync(Login model)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Status> RegisterAsync(Registration model)
        {
            throw new NotImplementedException();
        }
    }
}
