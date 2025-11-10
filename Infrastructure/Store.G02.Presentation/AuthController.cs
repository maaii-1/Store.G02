using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.G02.Services.Abstraction;
using Store.G02.Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager _serviceManager) : ControllerBase
    {
        // Login

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _serviceManager.AuthService.LoginAsync(request);
            return Ok(result);
        }



        // Register

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _serviceManager.AuthService.RegisterAsync(request);
            return Ok(result);
        }


        // Check Email Exist
        [HttpGet("EmailExists")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var result = await _serviceManager.AuthService.CheckEmailExistAsync(email);
            return Ok(result);
        }

        // Get Current User
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email);
            var result = await _serviceManager.AuthService.GetCurrentUserAsync(email.Value);
            return Ok(result);
        }


        // Get Current User Address
        [Authorize]
        [HttpGet("Address")]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email);
            var result = await _serviceManager.AuthService.GetCurrentUserAddressAsync(email.Value);
            return Ok(result);
        }


        // Update Current User Address
        [Authorize]
        [HttpPut("Address")]
        public async Task<IActionResult> UpdateCurrentUserAddress(AddressDto request)
        {
            var email = User.FindFirst(ClaimTypes.Email);
            var result = await _serviceManager.AuthService.UpdateCurrentUserAddressAsync(request, email.Value);
            return Ok(result);
        }

    }
}
