using System;
using System.ComponentModel.DataAnnotations;

namespace FarzinTools.ViewModels
{
    public class DeletePerformViewModel
    {
        [Required(ErrorMessage = "انتخاب پروژه الزامی است")]
        public string TitleId { get; set; }

        [Required(ErrorMessage = "ورود تاریخ الزامی است")]
        public DateTime? PerformDate { get; set; }
    }
}