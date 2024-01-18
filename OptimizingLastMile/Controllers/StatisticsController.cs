using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Models.Commons;
using OptimizingLastMile.Models.Params.Statistics;
using OptimizingLastMile.Models.Response.Statistics;
using OptimizingLastMile.Repositories.Accounts;
using OptimizingLastMile.Services.Statistics;
using OptimizingLastMile.Utils;

namespace OptimizingLastMile.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticService _statisticService;
    private readonly IAccountRepository _accountRepository;

    public StatisticsController(IStatisticService statisticService,
        IAccountRepository accountRepository)
    {
        this._statisticService = statisticService;
        this._accountRepository = accountRepository;
    }

    [HttpGet("managers/{id}/orders")]
    [Authorize(Roles = "ADMIN,MANAGER")]
    public async Task<IActionResult> GetStatisticOrderOfManager([FromRoute] long id, [FromQuery] ManagerOrderParam param)
    {
        if (param.StartTime > param.EndTime)
        {
            var error = Errors.Common.StartTimeGreaterThanEndTime();
            return BadRequest(EnvelopResponse.Error(error));
        }

        var authorId = MyTools.GetUserOfRequest(User.Claims);

        var author = await _accountRepository.GetById(authorId);

        if (author.Role == RoleEnum.MANAGER && authorId != id)
        {
            return Forbid();
        }

        var result = await _statisticService.GetStatisticOrderEachMonthOfMananager(id, param.StartTime.Value, param.EndTime.Value);

        return Ok(EnvelopResponse.Ok(result));
    }

    [HttpGet("driverOfTheMonth")]
    [Authorize(Roles = "MANAGER")]
    public async Task<IActionResult> GetDriverOfTheMonth([FromQuery][Required] DateTime? monthAndYear)
    {
        var listDriver = await _accountRepository.GetAllDriverIncludeOrder();

        var listTopDriver = listDriver.OrderByDescending(a => a.CountOrderDeliverySuccess(monthAndYear.Value)).Take(3).ToList();

        var result = new List<DriverOfTheMonthResponse>();

        foreach (var account in listTopDriver)
        {
            var driver = new DriverOfTheMonthResponse
            {
                DriverId = account.Id,
                DriverName = account.DriverProfile.Name,
                AvatarUrl = account.DriverProfile.AvatarUrl,
                TotalOrderDeliverySuccess = account.CountOrderDeliverySuccess(monthAndYear.Value)
            };

            result.Add(driver);
        }

        return Ok(EnvelopResponse.Ok(result));
    }
}

