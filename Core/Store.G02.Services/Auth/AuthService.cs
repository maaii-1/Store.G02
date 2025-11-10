using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Store.G02.Domain.Entities.Identity;
using Store.G02.Domain.Exceptions.BadRequest;
using Store.G02.Domain.Exceptions.NotFound;
using Store.G02.Domain.Exceptions.Unauthorized;
using Store.G02.Services.Abstraction.Auth;
using Store.G02.Shared;
using Store.G02.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager, IOptions<JwtOptions> options, IMapper _mapper) : IAuthService
    {
        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }


        public async Task<UserResponse?> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) throw new UserNotFoundException(email);
            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user),
            };
        }


        public async Task<AddressDto?> GetCurrentUserAddressAsync(string email)
        {
            var user = await _userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email.ToLower() == email.ToLower());
            if (user is null) throw new UserNotFoundException(email);
            return _mapper.Map<AddressDto>(user.Address);
        }


        public async Task<AddressDto?> UpdateCurrentUserAddressAsync(AddressDto request, string email)
        {
            var user = await _userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email.ToLower() == email.ToLower());
            if (user is null) throw new UserNotFoundException(email);

            if(user.Address is null)
            {
                user.Address = _mapper.Map<Address>(request);
            }
            else
            {
                user.Address.FirstName = request.FirstName;
                user.Address.LastName = request.LastName;
                user.Address.City = request.City;
                user.Address.Street = request.Street;
                user.Address.Country = request.Country;
            }

            await _userManager.UpdateAsync(user);

            return _mapper.Map<AddressDto>(user.Address);
        }


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
                Token = await GenerateTokenAsync(user),
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
                Token = await GenerateTokenAsync(user),
            };

        }


        private async Task<string> GenerateTokenAsync(AppUser user)
        {
            // Token: 
            // 1. Header    --> (type, algo)
            // 2. Payload   --> (claims)
            // 3. Signature --> (Key)

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),

            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }


            var JwtOptions = options.Value;

            // STRONGSecurityKEYFORAUTHenticationSTRONGSecurityKEYFORAUTHenticationSTRONGSecurityKEYFORAUTHenticationSTRONGSecurityKEYFORAUTHentication
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecurityKey));

            var token = new JwtSecurityToken
                (
                    issuer: JwtOptions.Issuer,
                    audience: JwtOptions.Audience,
                    claims: authClaims,
                    expires: DateTime.Now.AddDays(JwtOptions.DurationinDays),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

                );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
}
