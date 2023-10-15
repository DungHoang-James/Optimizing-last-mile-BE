using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OptimizingLastMile.Entites;
using OptimizingLastMile.Entites.Enums;
using OptimizingLastMile.Models.Commons;
using OptimizingLastMile.Models.Params.Drivers;
using OptimizingLastMile.Models.Requests.AccountProfiles;
using OptimizingLastMile.Models.Requests.Drivers;
using OptimizingLastMile.Models.Response.AccountProfile;
using OptimizingLastMile.Repositories.Accounts;
using OptimizingLastMile.Utils;

namespace OptimizingLastMile.Controllers;

[ApiController]
[Route("api/account-profile")]
public class AccountProfileController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public AccountProfileController(IAccountRepository accountRepository,
        IMapper mapper)
    {
        this._accountRepository = accountRepository;
        this._mapper = mapper;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "ADMIN,MANAGER,CUSTOMER")]
    public async Task<IActionResult> GetDetailProfile([FromRoute] long id)
    {
        var authorId = MyTools.GetUserOfRequest(User.Claims);

        var account = await _accountRepository.GetByIdIncludeProfile(id);

        if (account is null)
        {
            return NotFound();
        }

        if (account.Id != authorId)
        {
            return Forbid();
        }

        if (account.Status == StatusEnum.INACTIVE)
        {
            var error = Errors.Auth.AccountIsDisable();
            return BadRequest(EnvelopResponse.Error(error));
        }

        var profileDetail = _mapper.Map<ProfileDetailResponse>(account);

        return Ok(EnvelopResponse.Ok(profileDetail));
    }

    [HttpGet("drivers/{id}")]
    [Authorize(Roles = "DRIVER,MANAGER")]
    public async Task<IActionResult> GetDetailDriverProfile([FromRoute] long id)
    {
        var authorId = MyTools.GetUserOfRequest(User.Claims);
        var authorRoleStr = MyTools.GetRoleOfAuthRequest(User.Claims);

        var authorRole = Enum.Parse<RoleEnum>(authorRoleStr);

        var account = await _accountRepository.GetByIdIncludeProfile(id);

        if (account is null)
        {
            return NotFound();
        }

        if (authorRole == RoleEnum.DRIVER && account.Id != authorId)
        {
            return Forbid();
        }

        if (account.Role != RoleEnum.DRIVER)
        {
            return Forbid();
        }

        if (account.Status == StatusEnum.INACTIVE)
        {
            var error = Errors.Auth.AccountIsDisable();
            return BadRequest(EnvelopResponse.Error(error));
        }

        if (account.Status == StatusEnum.REJECT)
        {
            var error = Errors.Auth.AccountIsReject();
            return BadRequest(EnvelopResponse.Error(error));
        }

        var driverProfile = _mapper.Map<DriverProfileResponse>(account);

        return Ok(EnvelopResponse.Ok(driverProfile));
    }

    [HttpGet("drivers")]
    [Authorize(Roles = "MANAGER")]
    public async Task<IActionResult> GetManagerProfileList([FromQuery] DriverAccParam param)
    {
        var accPagination = await _accountRepository.GetPaginationAccountIncludeProfile(param.Search, RoleEnum.DRIVER, param.Page, param.Limit);

        var dataResponse = _mapper.Map<MultiObjectResponse<DriverProfileResponse>>(accPagination);

        return Ok(EnvelopResponse.Ok(dataResponse));
    }

    [HttpPut("drivers/{id}")]
    [Authorize(Roles = "DRIVER")]
    public async Task<IActionResult> UpdateDriverProfile([FromRoute] long id, [FromBody] DriverProfileUpdatePayload payload)
    {
        var authorId = MyTools.GetUserOfRequest(User.Claims);

        var account = await _accountRepository.GetByIdIncludeProfile(id);

        if (account is null)
        {
            return NotFound();
        }

        if (account.Id != authorId)
        {
            return Forbid();
        }

        if (account.Status == StatusEnum.INACTIVE)
        {
            var error = Errors.Auth.AccountIsDisable();
            return BadRequest(EnvelopResponse.Error(error));
        }

        if (account.Status == StatusEnum.REJECT)
        {
            var error = Errors.Auth.AccountIsReject();
            return BadRequest(EnvelopResponse.Error(error));
        }

        if (account.DriverProfile is null)
        {
            // Create
            var createResult = DriverProfile.Create(payload.Name,
                payload.BirthDay.Value.ToDateTime(TimeOnly.MinValue),/////////////////////////////////////
                payload.AvatarUrl,
                payload.Province,
                payload.District,
                payload.Ward,
                payload.Address,
                payload.PhoneContact,
                payload.IdentificationCardFrontUrl,
                payload.IdentificationCardBackUrl,
                payload.DrivingLicenseFrontUrl,
                payload.DrivingLicenseBackUrl,
                payload.VehicleRegistrationCertificateFrontUrl,
                payload.VehicleRegistrationCertificateBackUrl);

            if (createResult.IsFail)
            {
                return BadRequest(EnvelopResponse.Error(createResult.Error));
            }

            var driverProfile = createResult.Data;
            account.DriverProfile = driverProfile;
        }
        else
        {
            // Update
            var driverProfile = account.DriverProfile;

            driverProfile.SetName(payload.Name);
            driverProfile.SetBirthDay(payload.BirthDay.Value.ToDateTime(TimeOnly.MinValue));/////////////////////////////////////
            driverProfile.SetAvatarUrl(payload.AvatarUrl);
            driverProfile.SetProvince(payload.Province);
            driverProfile.SetDistrict(payload.District);
            driverProfile.SetWard(payload.Ward);
            driverProfile.SetAddress(payload.Address);
            driverProfile.SetPhoneContact(payload.PhoneContact);
            driverProfile.SetIdentificationCardFrontUrl(payload.IdentificationCardFrontUrl);
            driverProfile.SetIdentificationCardBackUrl(payload.IdentificationCardBackUrl);
            driverProfile.SetDrivingLicenseFrontUrl(payload.DrivingLicenseFrontUrl);
            driverProfile.SetDrivingLicenseBackUrl(payload.DrivingLicenseBackUrl);
            driverProfile.SetVehicleRegistrationCertificateFrontUrl(payload.VehicleRegistrationCertificateFrontUrl);
            driverProfile.SetVehicleRegistrationCertificateBackUrl(payload.VehicleRegistrationCertificateBackUrl);
        }

        if (account.Status == StatusEnum.NEW)
        {
            account.Status = StatusEnum.PENDING_APPROVE;
        }

        await _accountRepository.SaveAsync();

        return NoContent();
    }

    [HttpPut("drivers/{id}/status")]
    [Authorize(Roles = "MANAGER")]
    public async Task<IActionResult> UpdateDriverStatus([FromRoute] long id, [FromBody] DriverStatusUpdatePayload payload)
    {
        var driverAcc = await _accountRepository.GetAccountIncludeOrderShipping(id);

        if (driverAcc.Role != RoleEnum.DRIVER)
        {
            return Forbid();
        }

        switch (payload.Status)
        {
            case StatusEnum.ACTIVE:
                {
                    if (driverAcc.Status == StatusEnum.ACTIVE ||
                        driverAcc.Status == StatusEnum.INACTIVE ||
                        driverAcc.Status == StatusEnum.PENDING_APPROVE)
                    {
                        driverAcc.Status = StatusEnum.ACTIVE;
                        break;
                    }
                    var error = Errors.Common.MethodNotAllow();
                    return BadRequest(EnvelopResponse.Error(error));
                }
            case StatusEnum.INACTIVE:
                {
                    if (driverAcc.Status == StatusEnum.ACTIVE || driverAcc.Status == StatusEnum.INACTIVE)
                    {
                        var deactiveResult = driverAcc.DeactiveDriver();
                        if (deactiveResult.IsFail)
                        {
                            return BadRequest(EnvelopResponse.Error(deactiveResult.Error));
                        }
                        break;
                    }
                    var error = Errors.Common.MethodNotAllow();
                    return BadRequest(EnvelopResponse.Error(error));
                }
            case StatusEnum.REJECT:
                {
                    if (driverAcc.Status == StatusEnum.PENDING_APPROVE)
                    {
                        driverAcc.Status = StatusEnum.REJECT;
                        break;
                    }
                    var error = Errors.Common.MethodNotAllow();
                    return BadRequest(EnvelopResponse.Error(error));
                }
            default:
                {
                    var error = Errors.Common.MethodNotAllow();
                    return BadRequest(EnvelopResponse.Error(error));
                }
        }

        await _accountRepository.SaveAsync();

        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,MANAGER,CUSTOMER")]
    public async Task<IActionResult> UpdateProfile([FromRoute] long id, ProfileUpdatePayload payload)
    {
        var authorId = MyTools.GetUserOfRequest(User.Claims);

        var account = await _accountRepository.GetByIdIncludeProfile(id);

        if (account is null)
        {
            return NotFound();
        }

        if (account.Id != authorId)
        {
            return Forbid();
        }

        if (account.Status == StatusEnum.INACTIVE)
        {
            var error = Errors.Auth.AccountIsDisable();
            return BadRequest(EnvelopResponse.Error(error));
        }

        if (account.AccountProfile is null)
        {
            // Create
            var createResult = AccountProfile.Create(payload.Name,
                payload.BirthDay,
                payload.Province,
                payload.District,
                payload.Ward,
                payload.Address,
                payload.PhoneContact);

            if (createResult.IsFail)
            {
                return BadRequest(EnvelopResponse.Error(createResult.Error));
            }

            var profile = createResult.Data;
            account.AccountProfile = profile;
        }
        else
        {
            // Update
            var profile = account.AccountProfile;

            profile.SetName(payload.Name);
            profile.SetBirthDay(payload.BirthDay);
            profile.SetProvince(payload.Province);
            profile.SetDistrict(payload.Province);
            profile.SetWard(payload.Ward);
            profile.SetAddress(payload.Address);
            profile.SetPhoneContact(payload.PhoneContact);
        }

        if (account.Status == StatusEnum.NEW)
        {
            account.Status = StatusEnum.ACTIVE;
        }

        await _accountRepository.SaveAsync();

        return NoContent();
    }
}

