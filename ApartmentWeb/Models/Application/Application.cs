using ApartmentWeb.Validation;
using Domain.Enums;
using Domain.Models;
using System.ComponentModel.DataAnnotations;
using rm = Resources.Domain.Application;
using vrm = Resources.Domain.ApplicationValidation;

namespace ApartmentWeb.Models.Application
{
    public class Application : IEmailRequestBuilder
    {
        public Application()
        {

        }

        [Display(Name = nameof(rm.APP_RENTAL_ADDRESS), ResourceType = typeof(rm))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(vrm.APP_RENTAL_ADDRESS), ErrorMessageResourceType = typeof(vrm))]
        public string RentalAddress { get; set; }

        [Display(Name = nameof(rm.APP_OTHER_APPLICANTS), ResourceType = typeof(rm))]
        public string OtherApplicants { get; set; }

        [Display(Name = nameof(rm.APP_PERSONAL_INFO), ResourceType = typeof(rm))]
        public PersonalInfo PersonalInfo { get; set; } = new PersonalInfo();

        public EmploymentInfo PrimaryEmployment { get; set; } = new EmploymentInfo()
        {
            DisplayName = rm.APP_PRIMARY_EMPLOYMENT,
            AllowElectiveRequire = true,
            ElectiveRequireDisplay = rm.APP_HAS_JOB
        };

        public EmploymentInfo SecondaryEmployment { get; set; } = new EmploymentInfo()
        {
            DisplayName = rm.APP_SECOND_EMPLOYMENT,
            AllowElectiveRequire = true,
            ElectiveRequireDisplay = rm.APP_HAS_SECOND_JOB
        };

        public ParentInfo ParentInfo { get; set; } = new ParentInfo()
        {
            DisplayName = rm.APP_PARENT_INFO
        };

        [Display(Name = nameof(rm.APP_CONSIDER_OTHER_INCOME), ResourceType = typeof(rm))]
        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_CONSIDER_OTHER_INCOME), ErrorMessageResourceType = typeof(vrm))]
        public YesNo ConsiderOtherIncome { get; set; }

        [Display(Name = nameof(rm.APP_OTHER_INCOME), ResourceType = typeof(rm))]
        [DataType(DataType.MultilineText)]
        [RequireIfEnum(nameof(ConsiderOtherIncome), YesNo.Yes, nameof(vrm.APP_OTHER_INCOME), typeof(vrm))]
        public string OtherIncomeExplain { get; set; }

        public Automobile Automobile { get; set; } = new Automobile()
        {
            DisplayName = rm.APP_AUTOMOBILE,
            AllowElectiveRequire = true
        };

        public RentalReference CurrentRental { get; set; } = new RentalReference()
        {
            DisplayName = rm.APP_CURRENT_RESIDENCE,
            ElectiveRequireValue = YesNo.Yes
        };

        public RentalReference PriorRentRef1 { get; set; } = new RentalReference()
        {
            DisplayName = rm.APP_RENT_REF_1,
            AllowElectiveRequire = true,
            ElectiveRequireValue = YesNo.No,
            ElectiveRequireDisplay = rm.APP_ADD_RENT_REF
        };

        public RentalReference PriorRentRef2 { get; set; } = new RentalReference()
        {
            DisplayName = rm.APP_RENT_REF_2,
            AllowElectiveRequire = true,
            ElectiveRequireValue = YesNo.No,
            ElectiveRequireDisplay = rm.APP_ADD_RENT_REF
        };

        public PersonalReference PersonalReference1 = new PersonalReference()
        {
            DisplayName = rm.APP_PERSONAL_REF_1,
            AllowElectiveRequire = true,
            ElectiveRequireValue = YesNo.No,
            ElectiveRequireDisplay = rm.APP_ADD_PERSONAL_REF
        };

        public PersonalReference PersonalReference2 = new PersonalReference()
        {
            DisplayName = rm.APP_PERSONAL_REF_2,
            AllowElectiveRequire = true,
            ElectiveRequireValue = YesNo.No,
            ElectiveRequireDisplay = rm.APP_ADD_PERSONAL_REF
        };

        [Display(Name = nameof(rm.APP_ANTICIPATED_DURATION), ResourceType = typeof(rm))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(vrm.APP_ANTICIPATED_DURATION), ErrorMessageResourceType = typeof(vrm))]
        public string AnticipatedDuration { get; set; }

        [Display(Name = nameof(rm.APP_HAS_CRIMINAL_RECORD), ResourceType = typeof(rm))]
        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_HAS_CRIMINAL_RECORD), ErrorMessageResourceType = typeof(vrm))]
        public YesNo HasCriminalRecord { get; set; }

        [Display(Name = nameof(rm.APP_CRIMINAL_RECORD), ResourceType = typeof(rm))]
        [DataType(DataType.MultilineText)]
        [RequireIfEnum(nameof(HasCriminalRecord), YesNo.Yes, nameof(vrm.APP_CRIMINAL_RECORD), typeof(vrm))]
        public string ExplainCriminalRecord { get; set; }

        [Display(Name = nameof(rm.APP_HAS_BEEN_EVICTED), ResourceType = typeof(rm))]
        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_HAS_BEEN_EVICTED), ErrorMessageResourceType = typeof(vrm))]
        public YesNo HasBeenEvicted { get; set; }

        [Display(Name = nameof(rm.APP_EVICTED_EXPLAIN), ResourceType = typeof(rm))]
        [DataType(DataType.MultilineText)]
        [RequireIfEnum(nameof(HasBeenEvicted), YesNo.Yes, nameof(vrm.APP_EVICTED_EXPLAIN), typeof(vrm))]
        public string ExplainBeenEvicted { get; set; }

        [Display(Name = nameof(rm.APP_MARIJUANA), ResourceType = typeof(rm))]
        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_MARIJUANA), ErrorMessageResourceType = typeof(vrm))]
        public YesNo MarijuanaCard { get; set; }

        [Display(Name = nameof(rm.APP_SMOKERS), ResourceType = typeof(rm))]
        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_SMOKERS), ErrorMessageResourceType = typeof(vrm))]
        public YesNo Smokers { get; set; }

        [Display(Name = nameof(rm.APP_SMOKERS_COUNT), ResourceType = typeof(rm))]
        [RangeIfEnum("1", 0, "5", nameof(Smokers), YesNo.Yes, nameof(vrm.APP_SMOKERS_COUNT), typeof(vrm))]
        public int SmokersCount { get; set; }

        [Display(Name = nameof(rm.APP_DRINKERS), ResourceType = typeof(rm))]
        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_DRINKERS), ErrorMessageResourceType = typeof(vrm))]
        public YesNo Drinkers { get; set; }

        [Display(Name = nameof(rm.APP_DRINKERS_HOWOFTEN), ResourceType = typeof(rm))]
        [RangeIfEnum("1", 0, "3", nameof(Drinkers), YesNo.Yes, nameof(vrm.APP_DRINKERS_HOWOFTEN), typeof(vrm))]
        public HowOften HowOftenDrink { get; set; }

        [Display(Name = nameof(rm.APP_PETS), ResourceType = typeof(rm))]
        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_PETS), ErrorMessageResourceType = typeof(vrm))]
        public YesNo AnyPets { get; set; }

        [Display(Name = nameof(rm.APP_PETS_DESCRIBE), ResourceType = typeof(rm))]
        [DataType(DataType.MultilineText)]
        [RequireIfEnum(nameof(AnyPets), YesNo.Yes, nameof(vrm.APP_PETS_DESCRIBE), typeof(vrm))]
        public string DescribePets { get; set; }

        [Display(Name = nameof(rm.APP_NON_HUMAN), ResourceType = typeof(rm))]
        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_NON_HUMAN), ErrorMessageResourceType = typeof(vrm))]
        public YesNo AnyNonHuman { get; set; }

        [Display(Name = nameof(rm.APP_NON_HUMAN_DESCRIBE), ResourceType = typeof(rm))]
        [DataType(DataType.MultilineText)]
        [RequireIfEnum(nameof(AnyNonHuman), YesNo.Yes, nameof(vrm.APP_NON_HUMAN_DESCRIBE), typeof(vrm))]
        public string DescribeNonHuman { get; set; }

        [Display(Name = nameof(rm.APP_ATTEND_COLLEGE), ResourceType = typeof(rm))]
        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_ATTEND_COLLEGE), ErrorMessageResourceType = typeof(vrm))]
        public YesNo AttendCollege { get; set; }

        [Display(Name = nameof(rm.APP_COLLEGE_YEARS), ResourceType = typeof(rm))]
        [RangeIfEnum("0.1", 1, "20.0", nameof(AttendCollege), YesNo.Yes, nameof(vrm.APP_COLLEGE_YEARS), typeof(vrm))]
        public int CollegeYearsAttended { get; set; }

        [Display(Name = nameof(rm.APP_COLLEGE_GRADUATE), ResourceType = typeof(rm))]
        [DataType(DataType.MultilineText)]
        [RequireIfEnum(nameof(AttendCollege), YesNo.Yes, nameof(vrm.APP_COLLEGE_GRADUATE), typeof(vrm))]
        public string PlanToGraduate { get; set; }

        [Display(Name = nameof(rm.APP_ACCOMMODATION), ResourceType = typeof(rm))]
        [EnumDataType(typeof(YesNo))]
        [Range(1, 2, ErrorMessageResourceName = nameof(vrm.APP_ACCOMMODATION), ErrorMessageResourceType = typeof(vrm))]
        public YesNo NeedReasonableAccommodation { get; set; }

        [Display(Name = nameof(rm.APP_ACCOMMODATION_DESCRIBE), ResourceType = typeof(rm))]
        [DataType(DataType.MultilineText)]
        [RequireIfEnum(nameof(NeedReasonableAccommodation), YesNo.Yes, nameof(vrm.APP_ACCOMMODATION_DESCRIBE), typeof(vrm))]
        public string DescribeReasonableAccommodation { get; set; }

        [Display(Name = nameof(rm.APP_CERT_AND_AUTH), ResourceType = typeof(rm))]
        [EnumDataType(typeof(Yes))]
        [Range(1, 1, ErrorMessageResourceName = nameof(vrm.APP_CERT_AND_AUTH), ErrorMessageResourceType = typeof(vrm))]
        public Yes CertificationAndAuthorization { get; set; }

        [Display(Name = nameof(rm.APP_ADDITIONAL_COMMENTS), ResourceType = typeof(rm))]
        [DataType(DataType.MultilineText)]
        public string AdditionalComments { get; set; }

        public EmailRequest BuildEmailRequest()
        {
            return new EmailRequest()
            {
                Subject = $"Application for {RentalAddress} from {PersonalInfo.FirstName} {PersonalInfo.LastName}; Co-Applicants : {OtherApplicants}",
                Body = $"Attached is the application for {RentalAddress} from {PersonalInfo.FirstName} {PersonalInfo.LastName}",
                AttachmentName = $"{PersonalInfo.FirstName} {PersonalInfo.LastName} Application.pdf",
                PreferredReplyTo = PersonalInfo.Email
            };
        }
    }
}
