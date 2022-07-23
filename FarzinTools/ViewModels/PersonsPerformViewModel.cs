using System;
using System.Collections.Generic;

namespace FarzinTools.ViewModels
{
    public class PersonsPerformViewModel
    {
        public PersonsPerformViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public int CategoryCode { get; set; }

        public int RaitingHits { get; set; }

        public int? Grade { get; set; }

        public bool? IsPreNote { get; set; }

        public bool Locked { get; set; }

        public bool LocalLock { get; set; }

        public int LocalLockUserCode { get; set; }

        public int LocalLockRoleID { get; set; }

        public int LockUserCode { get; set; }

        public int LockRoleID { get; set; }

        public string FieldsStatus { get; set; }

        public bool IsPrivateSearch { get; set; }

        public int PrivateSearchUserCode { get; set; }

        public int PrivateSearchRoleID { get; set; }

        public DateTime? LastSignatureDate { get; set; }

        public string EntityNumber { get; set; }

        public int NumberOfCopies { get; set; }

        public string Version { get; set; }

        public int OriginalVersionCode { get; set; }

        public bool IsSigned { get; set; }

        public int SecurityLevelCode { get; set; }

        public bool IsConfirm { get; set; }

        public bool IsActive { get; set; }

        public int FirstEntityCode { get; set; }

        public string From { get; set; }

        public string ImportEntityNumber { get; set; }

        public DateTime? ImportDate { get; set; }

        public string ImportMethod { get; set; }

        public string ImportDesc { get; set; }

        public string ImportOriginNO { get; set; }

        public DateTime? ImportOriginDate { get; set; }

        public string NOPgAttached { get; set; }

        public string To { get; set; }

        public string ExportEntityNumber { get; set; }

        public DateTime? ExportDate { get; set; }

        public string ExportMethod { get; set; }

        public string ExportDesc { get; set; }

        public int? ImportIndicatorID { get; set; }

        public int? ExportIndicatorID { get; set; }

        public int? ImportRegisterID { get; set; }

        public int? ImportRegisterRoleID { get; set; }

        public int? ExportRegisterID { get; set; }

        public int? ExportRegisterRoleID { get; set; }

        public string DocType { get; set; }

        public string DocSubject { get; set; }

        public string DocKeywords { get; set; }

        public int? Priority { get; set; }

        public int? ImportPriority { get; set; }

        public int? ExportPriority { get; set; }

        public int CreatorID { get; set; }

        public int CreatorRoleID { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastEditDate { get; set; }

        public DateTime? PerformDate { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string FatherName { get; set; }

        public string PersonalCode { get; set; }

        public byte? ChildNo { get; set; }

        public string ProjectText { get; set; }

        public int? ProjectCode { get; set; }

        public string PostText { get; set; }

        public short? AreaCode { get; set; }

        public string PostCode { get; set; }

        public byte? Perform { get; set; }

        public byte? Leave { get; set; }

        public byte? Absence { get; set; }

        public byte? Overtime { get; set; }

        public byte? HolidayWork { get; set; }

        public byte? NightWorkDay { get; set; }

        public short? JobGroup { get; set; }

        public byte? NightWorkHour { get; set; }

        public long? OtherAdvantage { get; set; }

        public long? Deductions { get; set; }

        public bool IsValid { get; set; } = true;

        public string SummaryValidation { get; set; }

        public string NationalCode { get; set; }

        public string ClassName { get; set; }
    }
}