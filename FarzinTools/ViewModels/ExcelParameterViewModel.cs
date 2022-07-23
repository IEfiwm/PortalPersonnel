using System.Collections.Generic;

namespace FarzinTools.ViewModels
{
    public class PersonalsListViewModel
    {
        public List<Personal> Personals { get; set; } = new List<Personal>();
    }

    public class Personal
    {
        public string PersonalCode { get; set; }

        public string NationalCode { get; set; }

        public int ProjectCode { get; set; }
    }
}