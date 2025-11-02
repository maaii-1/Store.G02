using Microsoft.AspNetCore.Identity;
using Store.G02.Domain.Entities.Identity;
using Store.G02.Domain.Exceptions.BadRequest;
using Store.G02.Domain.Exceptions.NotFound;
using Store.G02.Domain.Exceptions.Unauthorized;
using Store.G02.Services.Abstraction.Auth;
using Store.G02.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager) : IAuthService
    {
        public async Task<UserResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) throw new UserNotFoundException(request.Email);

            var flag = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!flag) throw new UnauthorizedException();

            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = request.Email,
                Token = "TODO"
            };

        }

        public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                DisplayName = request.DisplayName,
                PhoneNumber = request.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if(!result.Succeeded) throw new RegistrationBadRequestException(result.Errors.Select(E => E.Description).ToList());

            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = request.Email,
                Token = "TODO"
            };

        }
    }
}
