﻿using System;
using System.ComponentModel.DataAnnotations;

namespace OptimizingLastMile.Models.Requests.Auths;

public class ChangePasswordEmailPayload
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(50)]
    public string NewPassword { get; set; }

    [Required]
    [StringLength(50)]
    public string ConfirmPassword { get; set; }
}