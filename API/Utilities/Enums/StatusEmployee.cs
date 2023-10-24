using System.ComponentModel.DataAnnotations;

namespace API.Utilities.Enums;

public enum StatusEmployee
{
    [Display(Name = "Idle")]
    idle = 0,

    [Display(Name = "Owner")]
    owner = 1,

    [Display(Name = "On Site")]
    onsite = 2,
}