using System;
using System.ComponentModel.DataAnnotations;

namespace OptimizingLastMile.Models.Requests.Managers;

public class ManagerAccCreatePayload
{
    [Required]
    [StringLength(500)]
    public string Email { get; set; }

    [Required]
    [StringLength(50)]
    public string Password { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    public DateTime? BirthDay { get; set; }

    [StringLength(200)]
    public string? Province { get; set; }

    [StringLength(200)]
    public string? District { get; set; }

    [StringLength(200)]
    public string? Ward { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(11)]
    [Phone]
    public string PhoneContact { get; set; }
}

