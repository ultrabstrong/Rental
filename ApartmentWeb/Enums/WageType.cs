using System.ComponentModel.DataAnnotations;

namespace ApartmentWeb.Enums
{
    public enum WageType
    {
        [Display(Name = "Hourly")]
        Hourly = 1,
        [Display(Name = "Salary")]
        Salary = 2
    }
}
