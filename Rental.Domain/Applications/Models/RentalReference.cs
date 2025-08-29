using Rental.Domain.Enums;
using System;
namespace Rental.Domain.Applications.Models;

public class RentalReference
{
    public bool AllowElectiveRequire { get; set; }
    public string? ElectiveRequireDisplay { get; set; }
    public YesNo? ElectiveRequireValue { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? LandlordName { get; set; }
    public string? LandlordPhoneNum { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string? ReasonForMoving { get; set; }
}
