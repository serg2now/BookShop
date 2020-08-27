using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BookShop.Api.DAL.Models.Auth;
using BookShop.Api.DAL.Repositories;
using BookShop.Api.DTOs.Auth;
using BookShop.Api.Extensions;
using BookShop.Api.Helpers;
using BookShop.Api.Helpers.GuidUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace BookShop.Api.Controllers.Api.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private SecurityConfigurations _securityConfigurations;
        private IRepository<RefreshToken> _tokenRepository;
        private ILogger<AuthController> _logger;

        private IMapper _mapper;

        public AuthController(
            IOptions<SecurityConfigurations> securityConfigurations,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IRepository<RefreshToken> tokenRepository,
            ILogger<AuthController> logger, 
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _securityConfigurations = securityConfigurations.Value;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(UserRegisterForm userRegisterForm)
        {
            if (ModelState.IsValid)
            {
                var userToCreate = _mapper.Map<User>(userRegisterForm);

                var result = await _userManager.CreateAsync(userToCreate, userRegisterForm.Password);

                var userProfile = _mapper.Map<UserProfileDto>(userToCreate);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userToCreate, "Client");

                    _logger.LogInformation($"New user with id {userToCreate.Id} successfully created.");

                    return Ok(new { userProfile });
                }

                return BadRequest(result.Errors);
            }

            _logger.LogError("Invalid request model, some fields are missed or have invalid format.");

            return BadRequest(ModelState.Values);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserLoginForm loginForm)
        {
            var user = await _userManager.FindByNameAsync(loginForm.UserName);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginForm.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(
                    u => u.NormalizedUserName == loginForm.UserName.ToUpper());

                var roles = await _userManager.GetRolesAsync(appUser);

                var userProfile = _mapper.Map<UserProfileDto>(appUser);

                var tokenKey = _securityConfigurations.tokenKey;
                var issuer = _securityConfigurations.Issuer;
                var appKey = _securityConfigurations.appKey;

                var token = TokensGenerator.GenerateJwtToken(appUser, roles, tokenKey, issuer);
                var refreshToken = TokensGenerator.GenerateRefreshToken();

                HttpContext.AddCookies(token, appKey);
                HttpContext.AddCookies(refreshToken, $"{appKey}Refresh");

                var existingToken = await _tokenRepository.FindItemAsync(
                    t => t.UserId == appUser.Id &&
                    t.DeviceName == Request.Headers["device-info"].ToString());

                if (existingToken != null)
                {
                    _logger.LogWarning($"User with Id {appUser.Id} has already logged in from this device, old refresh token will be removed.");

                    await _tokenRepository.RemoveItemAsync(existingToken);

                    _logger.LogInformation($"Old refresh token for user with Id {appUser.Id} removed from database.");
                }

                await _tokenRepository.AddItemAsync(
                    new RefreshToken
                    {
                        Id = GuidCreator.CreateGuid(),
                        TokenValue = refreshToken,
                        DeviceName = Request.Headers["device-info"],
                        UserId = appUser.Id
                    });
                
               _logger.LogInformation($"User with id {appUser.Id} successfully logged in.");

                return Ok(new { user = userProfile, token, refreshToken });
            }

            return Unauthorized();
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromHeader] string RefreshToken)
        {
            var refreshToken = await _tokenRepository.FindItemAsync(
                t => t.TokenValue == RefreshToken);

            if (refreshToken != null)
            {
                await _tokenRepository.RemoveItemAsync(refreshToken);
                refreshToken.TokenValue = TokensGenerator.GenerateRefreshToken();
                await _tokenRepository.AddItemAsync(refreshToken);

                var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
                var roles = await _userManager.GetRolesAsync(user);
                var userProfile = _mapper.Map<UserProfileDto>(user);

                var tokenKey = _securityConfigurations.tokenKey;
                var issuer = _securityConfigurations.Issuer;
                var appKey = _securityConfigurations.appKey;

                var token = TokensGenerator.GenerateJwtToken(user, roles, tokenKey, issuer);

                HttpContext.AddCookies(token, appKey);
                HttpContext.AddCookies(refreshToken.TokenValue, $"{appKey}Refresh");

                _logger.LogInformation($"Token for user {refreshToken.UserId} successfully refreshed.");

                return Ok(new { user = userProfile, token, refreshToken.TokenValue });
            }

            _logger.LogError($"Token {RefreshToken} doesn't exist in database.!");

            return StatusCode(401);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromHeader] string RefreshToken)
        {
            var token = await _tokenRepository.FindItemAsync(t => t.TokenValue == RefreshToken);

            if (token != null)
            {
                await _tokenRepository.RemoveItemAsync(token);

                _logger.LogInformation($"User with Id {token.UserId} successfully logged out.");
            }

            _logger.LogWarning($"Refresh token {RefreshToken} doesn't exist in database.");

            return NoContent();
        }
    }
}
