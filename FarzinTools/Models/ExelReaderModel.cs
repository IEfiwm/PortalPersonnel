using System;
using System.ComponentModel.DataAnnotations;

namespace FarzinTools.Models
{
    public class ExelReaderViewModel
    {
        public System.Guid Id { get; set; }
        public string PersonaliCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public Nullable<int> LocationJobCode { get; set; }
        public string PostCategori { get; set; }
        public Nullable<int> PostCode { get; set; }
        public Nullable<int> AreaCode { get; set; }
        public string JobCategori { get; set; }
        public Nullable<int> ChildCount { get; set; }
        public string workDays { get; set; }
        public string OverTimeHour { get; set; }
        public string NightWorkDays { get; set; }
        public string NightWorkHour { get; set; }
        public string CloseWorkDays { get; set; }
        public string LeaveWork { get; set; }
        public string AmountBenefits { get; set; }
        public string AmountYears { get; set; }
        public string Absence { get; set; }
        public string SummaryValidation { get; set; }
        public Nullable<bool> IsValid { get; set; }
        public DateTime DateTime { get; set; }
    }
}