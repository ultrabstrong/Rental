using System.ComponentModel.DataAnnotations;
using rm = Resources.WebsiteModels.Application;

namespace ApartmentWeb.Enums
{
    public enum HowOften
    {
        [Display(Name = nameof(rm.ENUM_HOWOFTEN_OCCASIONAL), ResourceType = typeof(rm))]
        Occasionally = 1,
        [Display(Name = nameof(rm.ENUM_HOWOFTEN_MODERATE), ResourceType = typeof(rm))]
        Moderately = 2,
        [Display(Name = nameof(rm.ENUM_HOWOFTEN_FREQUENT), ResourceType = typeof(rm))]
        Frequently = 3
    }
}
