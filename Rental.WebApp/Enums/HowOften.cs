using System.ComponentModel.DataAnnotations;

namespace Rental.WebApp.Enums;

public enum HowOften
{
    [Display(Name = "Occasionally")]
    Occasionally = 1,

    [Display(Name = "Moderately")]
    Moderately = 2,

    [Display(Name = "Frequently")]
    Frequently = 3,
}
