using Rental.WebApp.Enums;
using Rental.WebApp.Validation;
using System.ComponentModel.DataAnnotations;

namespace Rental.WebApp.Models.Application;

public class EmploymentInfo
{
    public string? DisplayName { get; set; }

    public bool AllowElectiveRequire { get; set; }

    public string? ElectiveRequireDisplay { get; set; }

    [Range(1, 2, ErrorMessage = "Please indicate if you have a job")]
    public YesNo ElectiveRequireValue { get; set; }

    [Display(Name = "Company")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the company name")]
    public string? Company { get; set; }

    [Display(Name = "Contact Name")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the contact name")]
    public string? ContactName { get; set; }

    [Display(Name = "Contact Phone #")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter the contact phone number")]
    public string? ContactPhone { get; set; }

    [Display(Name = "Length of employment")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter your length of employment")]
    public string? EmploymentLength { get; set; }

    [Display(Name = "Is this a permanent position")]
    [RangeIfEnum("1", 0, "2", nameof(ElectiveRequireValue), YesNo.Yes, "Please indicate if this is a permanent position")]
    public YesNo? IsPermenant { get; set; }

    [Display(Name = "Salary or hourly wage?")]
    [RangeIfEnum("1", 0, "2", nameof(ElectiveRequireValue), YesNo.Yes, "Please select salary or hourly wage")]
    public WageType? WageType { get; set; }

    [Display(Name = "Wage earned")]
    [RangeIfEnum("0.01", 2, nameof(ElectiveRequireValue), YesNo.Yes, "Please enter a wage greater than 0")]
    [DataType(DataType.Currency)]
    public decimal? Wage { get; set; }

    [Display(Name = "How many hours do you work each week?")]
    [RequireIfEnum(nameof(ElectiveRequireValue), YesNo.Yes, "Please enter your hours per week")]
    [RangeIfEnum("1", 0, nameof(ElectiveRequireValue), YesNo.Yes, "Please enter a number of hours greater than 0")]
    public int? HoursPerWeek { get; set; }
}
