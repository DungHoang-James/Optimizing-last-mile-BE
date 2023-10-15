using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Models.Commons;
using OptimizingLastMile.Models.Requests.Auths;
using OptimizingLastMile.Models.Response.Auths;
using OptimizingLastMile.Services.Accounts;
using OptimizingLastMile.Services.Auths;

namespace OptimizingLastMile.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAccountService _accountService;

    public AuthController(IAuthService authService,
        IAccountService accountService)
    {
        this._authService = authService;
        this._accountService = accountService;
    }

    [HttpPost("login/username")]
    public async Task<IActionResult> LoginByUsernamePassword([FromBody] LoginUsernamePayload payload)
    {
        var account = await _accountService.GetByUsername(payload.Username);

        if (account is null)
        {
            var error = Errors.Auth.UsernameNotExist();
            return BadRequest(EnvelopResponse.Error(error));
        }

        var checkStatus = _authService.CheckStatusActive(account.Status);
        if (checkStatus.IsFail)
        {
            return BadRequest(EnvelopResponse.Error(checkStatus.Error));
        }

        var isCorrectPass = _authService.IsCorrectPassword(payload.Password, account.Password);

        if (!isCorrectPass)
        {
            var error = Errors.Auth.PasswordIncorrect();
            return BadRequest(EnvelopResponse.Error(error));
        }

        var jwtToken = _authService.GenerateToken(account);
        var res = new JwtTokenResponse()
        {
            JwtToken = jwtToken
        };

        return Ok(EnvelopResponse.Ok(res));
    }

    [HttpPost("register/username")]
    public async Task<IActionResult> RegisterByUsername([FromBody] RegisterByUsernamePayload payload)
    {
        var result = await _accountService.RegisterByUsername(payload.Username, payload.Password, payload.Role);

        if (result.IsFail)
        {
            return BadRequest(EnvelopResponse.Error(result.Error));
        }

        var jwtToken = _authService.GenerateToken(result.Data);
        var res = new JwtTokenResponse()
        {
            JwtToken = jwtToken
        };

        return Ok(EnvelopResponse.Ok(res));
    }
}

