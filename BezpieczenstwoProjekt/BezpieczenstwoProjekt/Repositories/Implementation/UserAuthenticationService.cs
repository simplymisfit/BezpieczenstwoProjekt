using System.Security.Claims;
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
            var status = new Status();
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                status.StatusCode = 0;
                status.StatusMessage = "Invalid username";
                return status;
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.StatusMessage = "Invalid password";
                return status;
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

                status.StatusCode = 1;
                status.StatusMessage = "Logged in successfully";
                return status;
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.StatusMessage = "User locked out";
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.StatusMessage = "Error on logging in";
                return status;
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
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
                FirstName = model.FirstName,
                LastName = model.LastName,
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
        
        public async Task<Status> ChangePasswordAsync(ChangePassword model,string username)
        {
            var status = new Status();
            
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.StatusMessage = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.StatusMessage = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.StatusMessage = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;

        }
    }
}
