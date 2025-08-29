using Rental.Domain.Email.Models;
using Rental.WebApp.Enums;
using Rental.WebApp.Validation;
using System.ComponentModel.DataAnnotations;

namespace Rental.WebApp.Models.Application;

public partial class Application : IEmailRequestBuilder
{
    [Display(Name = "Rental Address")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the rental address")]
    public string RentalAddress { get; set; } = string.Empty;

    [Display(Name = "Who else from your group is applying for this location?")]
    public string? OtherApplicants { get; set; }

    [Display(Name = "Tell us about you")]
    public PersonalInfo PersonalInfo { get; set; } = new PersonalInfo();

    public RentalReference CurrentRental { get; set; } = new RentalReference()
    {
        DisplayName = "Two year's rental references preferred. Please list your actual landlord, property manager, or RA.",
        ElectiveRequireValue = YesNo.Yes
    };

    public EmploymentInfo PrimaryEmployment { get; set; } = new EmploymentInfo()
    {
        DisplayName = "Tell us about your job",
        AllowElectiveRequire = true,
        ElectiveRequireDisplay = "Do you have a job?"
    };

    public EmploymentInfo SecondaryEmployment { get; set; } = new EmploymentInfo()
    {
        DisplayName = "Tell us about your second job",
        AllowElectiveRequire = true,
        ElectiveRequireDisplay = "Do you have a second job?"
    };

    public ParentInfo ParentInfo { get; set; } = new ParentInfo()
    {
        DisplayName = "Parent Information"
    };

    [Display(Name = "Is there another source of income you would like considered?")]
    [EnumDataType(typeof(YesNo))]
    [Required(ErrorMessage = "Please indicate if you have other sources of income")]
    public YesNo? ConsiderOtherIncome { get; set; }

    [Display(Name = "Please explain:")]
    [DataType(DataType.MultilineText)]
    [RequireIfEnum(nameof(ConsiderOtherIncome), YesNo.Yes, "Please explain your other sources of income")]
    public string? OtherIncomeExplain { get; set; }

    public Automobile Automobile { get; set; } = new Automobile()
    {
        DisplayName = "What kind of vehicle do you own?",
        AllowElectiveRequire = true
    };

    public RentalReference PriorRentRef1 { get; set; } = new RentalReference()
    {
        DisplayName = "Previous Rental",
        AllowElectiveRequire = true,
        ElectiveRequireDisplay = "Add rental history?"
    };

    public PersonalReference PersonalReference1 { get; set; } = new PersonalReference()
    {
        DisplayName = "Personal reference #1",
        AllowElectiveRequire = true,
        ElectiveRequireDisplay = "Add personal reference?"
    };

    public PersonalReference PersonalReference2 { get; set; } = new PersonalReference()
    {
        DisplayName = "Personal reference #2",
        AllowElectiveRequire = true,
        ElectiveRequireDisplay = "Add personal reference?"
    };

    [Display(Name = "How long do you anticipate leasing this dwelling?")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please indicate your anticipated length of stay")]
    public string AnticipatedDuration { get; set; } = string.Empty;

    [Display(Name = "Does anyone applying for this apartment have a criminal record?")]
    [EnumDataType(typeof(YesNo))]
    [Required(ErrorMessage = "Please indicate if anyone applying has a criminal record")]
    public YesNo? HasCriminalRecord { get; set; }

    [Display(Name = "Please explain")]
    [DataType(DataType.MultilineText)]
    [RequireIfEnum(nameof(HasCriminalRecord), YesNo.Yes, "Please explain the criminal record")]
    public string? ExplainCriminalRecord { get; set; }

    [Display(Name = "Has anyone applying ever had their lease agreement terminated or been evicted?")]
    [EnumDataType(typeof(YesNo))]
    [Required(ErrorMessage = "Please indicate if anyone applying has been evicted")]
    public YesNo? HasBeenEvicted { get; set; }

    [Display(Name = "Please explain")]
    [DataType(DataType.MultilineText)]
    [RequireIfEnum(nameof(HasBeenEvicted), YesNo.Yes, "Please explain the eviction")]
    public string? ExplainBeenEvicted { get; set; }

    [Display(Name = "Do any applicants have or anticipate obtaining a Medical Marijuana Caregiver or Patient card designation?")]
    [EnumDataType(typeof(YesNo))]
    [Required(ErrorMessage = "Please indicate if any applicants have or anticipate obtaining a Medical Marijuana card")]
    public YesNo? MarijuanaCard { get; set; }

    [Display(Name = "Do any of the people that will be residing in this unit smoke anything?")]
    [EnumDataType(typeof(YesNo))]
    [Required(ErrorMessage = "Please indicate if any residents smoke")]
    public YesNo? Smokers { get; set; }

    [Display(Name = "How many individuals smoke?")]
    [RangeIfEnum("1", 0, "5", nameof(Smokers), YesNo.Yes, "Please enter the number of smokers")]
    public int? SmokersCount { get; set; }

    [Display(Name = "Do any of the people that will be residing in this unit drink alcohol?")]
    [EnumDataType(typeof(YesNo))]
    [Required(ErrorMessage = "Please indicate if any residents drink alcohol")]
    public YesNo? Drinkers { get; set; }

    [Display(Name = "How Often")]
    [RangeIfEnum("1", 0, "3", nameof(Drinkers), YesNo.Yes, "Please indicate how often residents drink")]
    public HowOften? HowOftenDrink { get; set; }

    [Display(Name = "Will there be any pets living on or in the premesis?")]
    [EnumDataType(typeof(YesNo))]
    [Required(ErrorMessage = "Please indicate if there will be any pets")]
    public YesNo? AnyPets { get; set; }

    [Display(Name = "List and describe each pet")]
    [DataType(DataType.MultilineText)]
    [RequireIfEnum(nameof(AnyPets), YesNo.Yes, "Please provide details about your pets")]
    public string? DescribePets { get; set; }

    [Display(Name = "Will there be any animals, birds, reptiles, insects, fish, or other non-human life forms living on or in the premises?")]
    [EnumDataType(typeof(YesNo))]
    [Required(ErrorMessage = "Please indicate if there will be any non-human life forms")]
    public YesNo? AnyNonHuman { get; set; }

    [Display(Name = "List and describe each of them")]
    [DataType(DataType.MultilineText)]
    [RequireIfEnum(nameof(AnyNonHuman), YesNo.Yes, "Please provide details about the non-human life forms")]
    public string? DescribeNonHuman { get; set; }

    [Display(Name = "Do you currently attend college?")]
    [EnumDataType(typeof(YesNo))]
    [Required(ErrorMessage = "Please indicate if you attend college")]
    public YesNo? AttendCollege { get; set; }

    [Display(Name = "How many years have you attended?")]
    [RangeIfEnum("0.1", 1, "20.0", nameof(AttendCollege), YesNo.Yes, "Please enter how many years you've attended college")]
    public int? CollegeYearsAttended { get; set; }

    [Display(Name = "When do you plan to graduate?")]
    [DataType(DataType.MultilineText)]
    [RequireIfEnum(nameof(AttendCollege), YesNo.Yes, "Please indicate when you plan to graduate")]
    public string? PlanToGraduate { get; set; }

    [Display(Name = "Is there a request for reasonable accommodation?")]
    [EnumDataType(typeof(YesNo))]
    [Required(ErrorMessage = "Please indicate if you need reasonable accommodation")]
    public YesNo? NeedReasonableAccommodation { get; set; }

    [Display(Name = "Please explain")]
    [DataType(DataType.MultilineText)]
    [RequireIfEnum(nameof(NeedReasonableAccommodation), YesNo.Yes, "Please explain your accommodation needs")]
    public string? DescribeReasonableAccommodation { get; set; }

    [Display(Name = "Do you agree to the following?")]
    [EnumDataType(typeof(Yes))]
    [Required(ErrorMessage = "You must agree to the terms and conditions")]
    public Yes? CertificationAndAuthorization { get; set; }

    [Display(Name = "Additional comments for property manager")]
    [DataType(DataType.MultilineText)]
    public string? AdditionalComments { get; set; }

    public EmailRequest BuildEmailRequest()
    {
        return new EmailRequest()
        {
            Subject = $"Application for {RentalAddress} from {PersonalInfo.FirstName} {PersonalInfo.LastName}; Co-Applicants: {OtherApplicants}",
            Body = $"Attached is the application for {RentalAddress} from {PersonalInfo.FirstName} {PersonalInfo.LastName}",
            AttachmentName = $"{PersonalInfo.FirstName} {PersonalInfo.LastName} Application.pdf",
            PreferredReplyTo = PersonalInfo.Email
        };
    }
}
