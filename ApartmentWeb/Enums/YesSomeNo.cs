using System.ComponentModel.DataAnnotations;

namespace ApartmentWeb.Enums
{
    public enum YesSomeNo
    {
        [Display(Name = "No")]
        No = 1,
        [Display(Name = "Yes")]
        Yes = 2,
        [Display(Name = "Some")]
        Some = 3
    }
}
