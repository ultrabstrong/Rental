using System.ComponentModel.DataAnnotations;

namespace ApartmentWeb.Enums
{
    public enum YesNo
    {
        [Display(Name = "No")]
        No = 1,
        [Display(Name = "Yes")]
        Yes = 2
    }
}
