using System.ComponentModel.DataAnnotations;

namespace Rental.WebApp.Enums;

public enum YesNo
{
    [Display(Name = "No")]
    No = 1,
    [Display(Name = "Yes")]
    Yes = 2
}
