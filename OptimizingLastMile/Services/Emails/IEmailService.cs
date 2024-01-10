using System;
using OptimizingLastMile.Models.Commons;

namespace OptimizingLastMile.Services.Emails;

public interface IEmailService
{
    Task<GenericResult> SendEmail(string email, string password);
}