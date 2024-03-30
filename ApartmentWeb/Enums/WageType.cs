using System.ComponentModel.DataAnnotations;
using rm = Resources.WebsiteModels.Application;

namespace ApartmentWeb.Enums
{
    public enum WageType
    {
        [Display(Name = nameof(rm.ENUM_WAGE_HOURLY), ResourceType = typeof(rm))]
        Hourly = 1,
        [Display(Name = nameof(rm.ENUM_WAGE_SALARY), ResourceType = typeof(rm))]
        Salary = 2
    }
}
