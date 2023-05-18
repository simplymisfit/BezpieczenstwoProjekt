using BezpieczenstwoProjekt.Models.Domain;
using BezpieczenstwoProjekt.Models.Dto;
using BezpieczenstwoProjekt.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BezpieczenstwoProjekt.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserAuthenticationService(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Status> LoginAsync(Login model)
        {
            throw new NotImplementedException();
        }

        public async Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Status> RegisterAsync(Registration model)
        {
            var status = new Status();
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                status.StatusCode = 0;
                status.StatusMessage = "User already exists!";
                return status;
            }

            ApplicationUser user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.Username,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.StatusMessage = "User creation failed!";
                return status;
            }

            //Role management
            if (!await _roleManager.RoleExistsAsync(model.Role))
                await _roleManager.CreateAsync(new IdentityRole(model.Role));

            if (await _roleManager.RoleExistsAsync(model.Role))
                await _userManager.AddToRoleAsync(user, model.Role);

            status.StatusCode = 1;
            status.StatusMessage = "User has registered successfully";
            return status;
        }
    }
}
