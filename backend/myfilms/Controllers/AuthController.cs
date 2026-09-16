using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.NativeInterop;
using myfilms.DTOs;
using myfilms.Models;
using myfilms.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace myfilms.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration config, ITokenService tokenService,
        UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<AuthController> logger)
    {
        _configuration = config;
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    [HttpPost, Route("Login")]
    public async Task<IActionResult> Login([FromBody]LoginModelDTO loginModel)
    {
        var user = await _userManager.FindByNameAsync(loginModel.UserName!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, loginModel.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("id", user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            
            foreach(var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            
            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
            var resfreshToken = _tokenService.GenerateRefreshToken();
            _ = double.TryParse(_configuration["JWT:RefreshTokenValidityInHours"],
                out double refreshTokenValidityInHours);
            user.RefreshToken = resfreshToken;
            user.RefreshTokenExpires = DateTime.UtcNow.AddHours(refreshTokenValidityInHours);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = resfreshToken,
                Expiration = token.ValidTo
            });
        }
        
        return Unauthorized();
    }

    [HttpPost, Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModelDTO registerModel)
    {
        var user = await _userManager.FindByNameAsync(registerModel.UserName!);
        
        if(user != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response{ Status = "Error", Message = "User already exists" });
        }
        
        await RolesExist();

        User newUser = new User()
        {
            Email = registerModel.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerModel.UserName,
        };

        var result = await _userManager.CreateAsync(newUser, registerModel.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new Response{ Status = "Error", Message = $"User Creation Failed. {string.Join(", ", result.Errors.Select(e=> e.Description))}"});
        }
        
        await _userManager.AddToRoleAsync(newUser, "User");

        return Ok(new Response()
        {
            Status = "Success",
            Message = "User created successfully"
        });
    }

    [HttpPost, Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken(TokenModelDTO tokenModel)
    {
        if (tokenModel is null) 
            return BadRequest();

        string? accessToken = tokenModel.AccessToken
                             ?? throw new ArgumentNullException(nameof(tokenModel));

        string? refreshToken = tokenModel.RefreshToken
                               ?? throw new ArgumentNullException(nameof(tokenModel));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal == null)
            return BadRequest("Invalid access token/refresh token");

        string userName = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(userName);

        if (user == null
            || user.RefreshToken != refreshToken
            || user.RefreshTokenExpires <= DateTime.UtcNow)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    [HttpPost, Authorize(Policy = "SuperAdminOnly"), Route("Revoke/user")]
    public async Task<IActionResult> Revoke(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return BadRequest("Invalid userName");

        user.RefreshToken = null;

        await _userManager.UpdateAsync(user);

        return NoContent();
    }

    [HttpPost, Authorize(Policy = "SpecialUserOnly"), Route("CreateRole")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        
        if (!roleExist)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (roleResult.Succeeded)
            {   
                _logger.LogInformation(1, "Role Added");
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = $"Role {roleName} added successfully" });
            }
            else
            {
                _logger.LogInformation(2, "Error");
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = $"Issue adding the new {roleName} role" });
            }
        }
        
        return StatusCode(StatusCodes.Status400BadRequest,
            new Response { Status = "Error", Message = $"Role {roleName} already exists" });
    }

    [HttpPost, Authorize(Policy = "AdminOnly"), Route("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation(1, $"User {user.Email} added to {roleName} role");
                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = $"User {user.Email} added to the {roleName} role" });
            }
            else
            {
                _logger.LogInformation(2, "Error");
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = $"Unable to add user {user.Email} to the {roleName} role" });
            }
        }
        
        return BadRequest(new { error = "Unable to find user" });
    }

    private async Task RolesExist()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        if (!roles.Any())
        {
            var newRoles = new List<IdentityRole>
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Admin"),
                new IdentityRole("User")
            };

            foreach (var role in newRoles)
            {
                await _roleManager.CreateAsync(role);
            }
            
            // var superAdmin = await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            // var Admin = await _roleManager.CreateAsync(new IdentityRole("Admin"));
            // var user = await _roleManager.CreateAsync(new IdentityRole("User"));
        }
    }
}