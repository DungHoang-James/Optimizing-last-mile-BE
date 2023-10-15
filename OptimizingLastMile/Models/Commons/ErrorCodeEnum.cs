using System;
namespace OptimizingLastMile.Models.Commons;

public enum ErrorCodeEnum
{
    // Auth
    USERNAME_NOT_EXIST,
    PASSWORD_NOT_CORRECT,
    ACCOUNT_IS_DISABLE,
    ACCOUNT_REJECTED,
    ACCOUNT_ALREADY_EXIST,
    ROLE_NOT_ALLOW_RESGISTER_BY_USERNAME,

    // Error on AccountProfile
    INVALID_BIRTHDAY,
    DRIVER_HAVE_ORDER,

    // Common
    METHOD_NOT_ALLOW
}

