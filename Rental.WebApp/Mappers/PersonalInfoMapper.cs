using Rental.Domain.Applications.Models;
using PersonalInfoViewModel = Rental.WebApp.Models.Application.PersonalInfo;

namespace Rental.WebApp.Mappers;

public static class PersonalInfoMapper
{
    public static PersonalInfo ToDomainModel(this PersonalInfoViewModel src) => new()
    {
        FirstName = src.FirstName,
        MiddleName = src.MiddleName,
        LastName = src.LastName,
        PhoneNum = src.PhoneNum,
        SSN = src.SSN,
        DriverLicense = src.DriverLicense,
        DriverLicenseStateOfIssue = src.DriverLicenseStateOfIssue,
        Email = src.Email
    };
}
