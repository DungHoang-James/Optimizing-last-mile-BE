using System;
using System.ComponentModel.DataAnnotations;

namespace OptimizingLastMile.Models.Requests.Auths;

public class LoginEmailPassPayload
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}