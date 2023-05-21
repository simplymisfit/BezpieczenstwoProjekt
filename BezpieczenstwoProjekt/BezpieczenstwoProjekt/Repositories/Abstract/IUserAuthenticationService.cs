using BezpieczenstwoProjekt.Models.Dto;

namespace BezpieczenstwoProjekt.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(Login model);
        Task<Status> RegisterAsync(Registration model);
        Task LogoutAsync();
        Task<Status> ChangePasswordAsync(ChangePassword model, string username);
    }
}
