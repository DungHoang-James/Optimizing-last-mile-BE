using System;
namespace OptimizingLastMile.Models.Commons;

public class Errors
{
    public static class Auth
    {
        public static ErrorObject UsernameNotExist()
        {
            return new ErrorObject(ErrorCodeEnum.USERNAME_NOT_EXIST.ToString(),
                "Username not exist");
        }

        public static ErrorObject PasswordIncorrect()
        {
            return new(ErrorCodeEnum.PASSWORD_NOT_CORRECT.ToString(),
                "Password incorrect");
        }

        public static ErrorObject AccountIsDisable()
        {
            return new(ErrorCodeEnum.ACCOUNT_IS_DISABLE.ToString(),
                "Account is inactive");
        }

        public static ErrorObject AccountIsReject()
        {
            return new(ErrorCodeEnum.ACCOUNT_REJECTED.ToString(),
                "Account rejected");
        }

        public static ErrorObject UsernameAlreadyExist()
        {
            return new(ErrorCodeEnum.ACCOUNT_ALREADY_EXIST.ToString(),
                "Account already exist");
        }

        public static ErrorObject RoleNotAllowRegisterByUsername()
        {
            return new(ErrorCodeEnum.ROLE_NOT_ALLOW_RESGISTER_BY_USERNAME.ToString(),
                "Not allow this role to register account by username");
        }
    }

    public static class AccountProfile
    {
        public static ErrorObject InvalidBirthDay()
        {
            return new(ErrorCodeEnum.INVALID_BIRTHDAY.ToString(),
                "Invalid birth day");
        }

        public static ErrorObject NotDeactiveDriver()
        {
            return new(ErrorCodeEnum.DRIVER_HAVE_ORDER.ToString(),
                "Driver still have order need to be deliver. Cannot deactive right now");
        }
    }

    public static class Common
    {
        public static ErrorObject MethodNotAllow()
        {
            return new(ErrorCodeEnum.METHOD_NOT_ALLOW.ToString(),
                "Method not allow this perform");
        }
    }
}