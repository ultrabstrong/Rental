using ApartmentWeb.Enums;
using ApartmentWeb.Models.Application;
using ApartmentWeb.Models.Site;
using Domain.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;

namespace ApartmentWeb
{
    public static class Shared
    {
        private static readonly string _dataDir = $"{HttpContext.Current.Server.MapPath("~")}/data/";
        private static readonly string _configFile = $"{HttpContext.Current.Server.MapPath("~")}/data/serviceconfig.xml";

        private static object ConfigLock { get; set; } = new object();

        static Shared()
        {
            if (!Directory.Exists(_dataDir))
            {
                Directory.CreateDirectory(_dataDir);
            }

            try
            {
                if (!File.Exists(_configFile))
                {
                    Configuration = new SiteDetails()
                    {
                        CompanyName = "CompanyName",
                        CompanyShortName = "CompanyShortName",
                        EmailAddress = "EmailAddress",
                        PhoneNumber = "PhoneNumber",
                        Address = "Address",
                        MailSettings = new MailSettings()
                        {
                            SMTPServer = "SMTPServer",
                            SMTPUsername = "SMTPUsername",
                            SMTPPw = "SMTPPw",
                            SMTPPort = 0,
                            SMTPTo = "SMTPTo"
                        },
                        ShowDownloadApplication = true,
                        TenantInfoShowTrash = false,
                        TenantInfoPostOfficeAddress = "PostOfficeAddress",
                        TenantInfoDocs = new List<TenantInfoDoc>()
                        {
                            new TenantInfoDoc() {DisplayName = "DisplayName", FileName = "FileName" }
                        }
                    };

                    SaveConfiguration();
                }
                else
                {
                    LoadConfiguration();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Failed to get service configuration", ex);
            }
        }

        public static SiteDetails Configuration { get; set; }

        public static string Version => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        // Sample application with realistic test data for testing purposes
        public static Application TestApplication => new Application
        {
            RentalAddress = "1234 Apartment Way, Apt #301, Portland, OR 97201",
            OtherApplicants = "Sarah Johnson, Michael Williams",
            PersonalInfo = new PersonalInfo
            {
                FirstName = "Jane",
                MiddleName = "",
                LastName = "Doe",
                DriverLicense = "OR12345678",
                DriverLicenseStateOfIssue = "OR",
                SSN = "123-45-6789",
                PhoneNum = "(503) 555-7890",
                Email = "janedoe@example.com",
                DisplayName = "Personal Information"
            },
            PrimaryEmployment = new EmploymentInfo
            {
                DisplayName = "Tell us about your job",
                AllowElectiveRequire = true,
                ElectiveRequireDisplay = "Do you have a job?",
                ElectiveRequireValue = YesNo.Yes,
                Company = "Portland City Hospital",
                ContactPhone = "(503) 555-1234",
                ContactName = "Dr. Robert Smith",
                IsPermenant = YesNo.Yes,
                EmploymentLength = "3 years",
                WageType = WageType.Hourly,
                Wage = 32.50m,
                HoursPerWeek = 40
            },
            SecondaryEmployment = new EmploymentInfo
            {
                DisplayName = "Tell us about your second job",
                AllowElectiveRequire = true,
                ElectiveRequireDisplay = "Do you have a second job?",
                ElectiveRequireValue = YesNo.Yes,
                Company = "Portland Coffee House",
                ContactPhone = "(503) 555-8765",
                ContactName = "Maria Garcia",
                IsPermenant = YesNo.Yes,
                EmploymentLength = "1 year",
                WageType = WageType.Hourly,
                Wage = 18.75m,
                HoursPerWeek = 12
            },
            ParentInfo = new ParentInfo
            {
                DisplayName = "Parent Information",
                ElectiveRequireValue = YesNo.Yes,
                FirstName = "Robert",
                MiddleName = "James",
                LastName = "Doe",
                PhoneNum = "(971) 555-1234",
                Street = "42 Willow Lane",
                City = "Seattle",
                State = "WA",
                Zip = "98101"
            },
            ConsiderOtherIncome = YesNo.Yes,
            OtherIncomeExplain = "I receive $500 monthly from investments and a trust fund that pays $1200 quarterly.",
            Automobile = new Automobile
            {
                DisplayName = "What kind of vehicle do you own?",
                AllowElectiveRequire = true,
                ElectiveRequireValue = YesNo.Yes,
                Make = "Honda",
                Model = "Civic",
                Year = "2019",
                State = "OR",
                LicenseNum = "ABC 123",
                Color = "Blue"
            },
            CurrentRental = new RentalReference
            {
                DisplayName = "Current Rental",
                ElectiveRequireValue = YesNo.Yes,
                Street = "789 Previous St",
                City = "Portland",
                State = "OR",
                Zip = "97214",
                Start = new DateTime(2021, 5, 1),
                LandlordName = "Green Property Management",
                LandlordPhoneNum = "(503) 555-4321",
                ReasonForMoving = "Seeking a larger apartment with more amenities and closer to work."
            },
            PriorRentRef1 = new RentalReference
            {
                DisplayName = "Previous Rental",
                AllowElectiveRequire = true,
                ElectiveRequireDisplay = "Add rental history",
                ElectiveRequireValue = YesNo.Yes,
                Street = "456 Old Home Lane",
                City = "Salem",
                State = "OR",
                Zip = "97301",
                Start = new DateTime(2018, 6, 1),
                End = new DateTime(2021, 4, 30),
                LandlordName = "Summit Properties",
                LandlordPhoneNum = "(503) 555-9876",
                ReasonForMoving = "Relocated to Portland for work opportunities."
            },
            PriorRentRef2 = new RentalReference
            {
                DisplayName = "Rental Reference #3",
                AllowElectiveRequire = true,
                ElectiveRequireDisplay = "Add rental history",
                ElectiveRequireValue = YesNo.Yes,
                Street = "123 College Ave",
                City = "Eugene",
                State = "OR",
                Zip = "97403",
                Start = new DateTime(2016, 9, 1),
                End = new DateTime(2018, 5, 31),
                LandlordName = "University Heights Apartments",
                LandlordPhoneNum = "(541) 555-4242",
                ReasonForMoving = "Graduated from college and moved to Salem for first job."
            },
            PersonalReference1 = new PersonalReference
            {
                DisplayName = "Personal reference #1",
                AllowElectiveRequire = true,
                ElectiveRequireDisplay = "Add personal reference",
                ElectiveRequireValue = YesNo.Yes,
                Name = "Emily Rogers",
                Relationship = "Colleague",
                PhoneNum = "(503) 555-5678"
            },
            PersonalReference2 = new PersonalReference
            {
                DisplayName = "Personal reference #2",
                AllowElectiveRequire = true,
                ElectiveRequireDisplay = "Add personal reference",
                ElectiveRequireValue = YesNo.Yes,
                Name = "James Wilson",
                Relationship = "Family Friend",
                PhoneNum = "(971) 555-3456"
            },
            AnticipatedDuration = "At least 2 years",
            HasCriminalRecord = YesNo.Yes,
            ExplainCriminalRecord = "One minor parking violation in 2020 that resulted in community service.",
            HasBeenEvicted = YesNo.Yes,
            ExplainBeenEvicted = "There was an error with my previous apartment building's management in 2017 where they incorrectly processed an eviction notice, but it was resolved and removed from my record.",
            MarijuanaCard = YesNo.Yes,
            Smokers = YesNo.Yes,
            SmokersCount = 1,
            Drinkers = YesNo.Yes,
            HowOftenDrink = HowOften.Occasionally,
            AnyPets = YesNo.Yes,
            DescribePets = "One cat named Luna, 3 years old, spayed, indoor only, 7 pounds, domestic short hair.",
            AnyNonHuman = YesNo.Yes,
            DescribeNonHuman = "One 10-gallon freshwater aquarium with 5 small fish (neon tetras and guppies).",
            AttendCollege = YesNo.Yes,
            CollegeYearsAttended = 4,
            PlanToGraduate = "Graduated last year with a BSN in Nursing",
            NeedReasonableAccommodation = YesNo.Yes,
            DescribeReasonableAccommodation = "I have occasional migraines and would prefer a unit away from high-traffic or noisy areas if possible.",
            CertificationAndAuthorization = Yes.Yes,
            AdditionalComments = "I'm a registered nurse working the day shift at Portland City Hospital. I'm looking for a quiet apartment close to work and public transportation. I'm a responsible tenant with a stable income and excellent rental history."
        };

        public static void LoadConfiguration()
        {
            lock (ConfigLock)
            {
                var xmlSerializer = new XmlSerializer(typeof(SiteDetails));
                using (var fs = new FileStream(_configFile, FileMode.Open, FileAccess.Read))
                {
                    Configuration = (SiteDetails)xmlSerializer.Deserialize(fs);
                }
            }
        }

        public static void SaveConfiguration()
        {
            lock (ConfigLock)
            {
                var xmlSerializer = new XmlSerializer(typeof(SiteDetails));
                using (var fs = new FileStream(_configFile, FileMode.Create, FileAccess.Write))
                {
                    xmlSerializer.Serialize(fs, Configuration);
                }
            }
        }
    }
}