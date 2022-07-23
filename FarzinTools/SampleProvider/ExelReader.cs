using FarzinTools.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarzinTools.ViewModels;
using System.Text.RegularExpressions;
using System.Data.Entity;
using System.Threading.Tasks;
using FarzinTools.Models.Parameters;
using FarzinTools.Models.Dtos;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace FarzinTools.Service
{
    public static class ExelReader
    {
        public static string _filePath;
        public static bool IsValid = false;
        public static bool _isExist = false;
        public static List<PersonsPerformViewModel> _personsPerformList;
        public static List<PersonelsInfoViewModel> _personelsInfoList = new List<PersonelsInfoViewModel>();
        public static List<BaseInfoViewModel> _baseInfoList = new List<BaseInfoViewModel>();
        public static List<Personal> _personals = new List<Personal>();

        static readonly eOrganizationEntities1 context = new eOrganizationEntities1();

        public static void SetFilePath(string filePath)
        {
            _filePath = filePath;
        }

        public static List<PersonsPerformViewModel> ReadXLS(HttpPostedFileBase fileInfo, byte? date, DateTime? performDate)
        {
            ExcelPackage _package = new ExcelPackage(fileInfo.InputStream);

            ExcelWorksheet _worksheet = _package.Workbook.Worksheets.FirstOrDefault();

            int rows = _worksheet.Dimension.Rows;

            _personsPerformList = new List<PersonsPerformViewModel>();

            DateTime datePerform = DateTime.Now.Date;

            if (performDate == null)
            {
                if (date == 0)
                {
                    //today
                    datePerform = DateTime.Now.Date;
                }
                else if (date == 1)
                {
                    // yesterday
                    datePerform = DateTime.Now.Date.AddDays(-1);
                }
                else if (date == 2)
                {
                    // 2 days ago
                    datePerform = DateTime.Now.Date.AddDays(-2);
                }
            }
            else
            {
                datePerform = performDate.Value;
            }

            for (int i = 2; i <= rows; i++)
            {
                for (int j = 1; j <= 20; j++)
                {
                    var rowModel = new PersonsPerformViewModel();

                    try
                    {
                        rowModel.PerformDate = datePerform;

                        var content = _worksheet.Cells[i, j].Value;

                        if (content == null) continue;

                        if (!PersenaliIsValid(content, rowModel))
                        {
                            rowModel.IsValid = false;

                            rowModel.SummaryValidation = "کد پرسنلی نامعتبر میباشد";

                            rowModel.ClassName = "danger";
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.Name = string.Empty;
                        }
                        else
                        {
                            rowModel.Name = content.ToString();
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.LastName = string.Empty;
                        }
                        else
                        {
                            rowModel.LastName = content.ToString();
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.FatherName = string.Empty;
                        }
                        else
                        {
                            rowModel.FatherName = content.ToString();
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content != null)
                        {
                            var nationalCode = content.ToString().Trim();

                            rowModel.NationalCode = nationalCode;
                        }
                        else
                        {
                            rowModel.NationalCode = string.Empty;

                            rowModel.IsValid = false;

                            rowModel.ClassName = "danger";

                            rowModel.SummaryValidation += "کد ملی معتبر نمی باشد";
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.ProjectCode = 0;
                        }
                        else
                        {
                            int.TryParse(content.ToString(), out int result);

                            rowModel.ProjectCode = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.PostText = string.Empty;
                        }
                        else
                        {
                            rowModel.PostText = content.ToString();
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.PostCode = string.Empty;
                        }
                        else
                        {
                            rowModel.PostCode = content.ToString();
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.AreaCode = -1;
                        }
                        else
                        {
                            short.TryParse(content.ToString(), out short result);

                            rowModel.AreaCode = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.JobGroup = 0;
                        }
                        else
                        {
                            short.TryParse(content.ToString(), out short result);

                            rowModel.JobGroup = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.ChildNo = 0;
                        }
                        else
                        {
                            byte.TryParse(content.ToString(), out byte result);

                            rowModel.ChildNo = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.Perform = 0;
                        }
                        else
                        {
                            byte.TryParse(content.ToString(), out byte result);

                            rowModel.Perform = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.Overtime = 0;
                        }
                        else
                        {
                            byte.TryParse(content.ToString(), out byte result);

                            rowModel.Overtime = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.NightWorkDay = 0;
                        }
                        else
                        {
                            byte.TryParse(content.ToString(), out byte result);

                            rowModel.NightWorkDay = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.NightWorkHour = 0;
                        }
                        else
                        {
                            byte.TryParse(content.ToString(), out byte result);

                            rowModel.NightWorkHour = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.HolidayWork = 0;
                        }
                        else
                        {
                            byte.TryParse(content.ToString(), out byte result);

                            rowModel.HolidayWork = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.Leave = 0;
                        }
                        else
                        {
                            byte.TryParse(content.ToString(), out byte result);

                            rowModel.Leave = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.OtherAdvantage = 0;
                        }
                        else
                        {
                            byte.TryParse(content.ToString(), out byte result);

                            rowModel.OtherAdvantage = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.Deductions = 0;
                        }
                        else
                        {
                            byte.TryParse(content.ToString(), out byte result);

                            rowModel.Deductions = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            rowModel.Absence = 0;
                        }
                        else
                        {
                            byte.TryParse(content.ToString(), out byte result);

                            rowModel.Absence = result;
                        }

                        j++;

                        content = _worksheet.Cells[i, j].Value;

                        if (content == null)
                        {
                            //rowModel.SummaryValidation = "0";
                        }
                        else
                        {
                            //rowModel.SummaryValidation = content.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        rowModel.IsValid = false;

                        rowModel.ClassName = "exception";

                        rowModel.SummaryValidation += ex.Message;
                    }

                    _personsPerformList.Add(rowModel);

                    _personals.Add(new Personal
                    {
                        NationalCode = rowModel.NationalCode,

                        PersonalCode = rowModel.PersonalCode,

                        ProjectCode = rowModel.ProjectCode ?? 0
                    });
                }
            }

            string serializeXml = _personals.SerializeXml("Personals");

            var storeProcuderConfig = new StoreProcedureConfiguration();

            var validatingLists = storeProcuderConfig.ExecuteStoredProcedure<ExcelParameter, GetPersonalCodeDto, GetNationalCodeDto, ProjectTitleDto, GetPersonalPerformDto>("Sp_GetPersonalInfo", new ExcelParameter
            {
                Date = datePerform,

                SerializedPersonalXml = serializeXml
            }, "dbo");

            PrepareDatabaseValidation(validatingLists);

            return _personsPerformList;
        }

        private static void PrepareDatabaseValidation(ResultStoredProcedure<GetPersonalCodeDto, GetNationalCodeDto, ProjectTitleDto, GetPersonalPerformDto> validatingLists)
        {
            _personsPerformList.ForEach(model =>
            {
                if (!validatingLists.List1.Any(f => f.PersonelID == model.PersonalCode))
                {
                    model.IsValid = false;

                    model.SummaryValidation = "کد پرسنلی نامعتبر میباشد";

                    model.ClassName = "danger";
                }

                if (!validatingLists.List2.Any(f => f.NationalID == model.NationalCode))
                {
                    model.IsValid = false;

                    model.ClassName = "danger";

                    model.SummaryValidation += "کد ملی در سیستم موجود نیست";

                    if (model.NationalCode.Length != 10)
                    {
                        model.SummaryValidation += "، فرمت کد ملی معتبر نیست";
                    }
                }

                var project = validatingLists.List3.FirstOrDefault(f => f.ProjectCode == model.ProjectCode);

                if (project == null)
                {
                    model.IsValid = false;

                    model.ClassName = "danger";

                    model.SummaryValidation += "پروژه در سیستم موجود نیست";

                    model.ProjectText = string.Empty;
                }
                else
                {
                    model.ProjectText = project.Project ?? string.Empty;
                }

                if (validatingLists.List4.Any(f => f.PersonalCode == model.PersonalCode))
                {
                    model.IsValid = false;

                    model.ClassName = "warning";

                    model.SummaryValidation = " رکورد تکراری";
                }
            });
        }

        public static bool IsPersonalCodeExist(object content, DateTime date)
        {
            if (content == null)
            {
                return true;
            }

            var minDate = new DateTime(date.Year, date.Month, date.Day);

            var maxDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);

            return context.Entity_PersonsPerform.Any(s => s.PersonalCode == content.ToString() && s.PerformDate.Value <= maxDate && s.PerformDate.Value >= minDate);
        }

        public static string PersonalCodeValidation(object content)
        {
            string PersonalCodeValided;

            if (content.ToString().Length == 4)
            {
                PersonalCodeValided = content.ToString();

                return PersonalCodeValided;
            }

            if (content.ToString().Length == 3)
            {
                PersonalCodeValided = "0" + content.ToString();

                return PersonalCodeValided;
            }

            if (content.ToString().Length == 2)
            {
                PersonalCodeValided = "00" + content.ToString();

                return PersonalCodeValided;
            }

            if (content.ToString().Length == 1)
            {
                PersonalCodeValided = "000" + content.ToString();

                return PersonalCodeValided;
            }

            else
            {
                PersonalCodeValided = content.ToString();

                return PersonalCodeValided;
            }
        }

        public static bool PersenaliIsValid(object content, PersonsPerformViewModel rowModel = null)
        {
            if (content == null || content.ToString().Length > 5 || content.ToString().Length < 1)
            {
                rowModel.PersonalCode = content?.ToString() ?? string.Empty;

                IsValid = false;

                return false;
            }

            rowModel.PersonalCode = content.ToString();

            return true;
        }

        public static bool IsIranianNationalIdValid(string id)
        {
            if (id?.Length != 10)
                return false;

            id = id.PadLeft(10, '0');

            if (!Regex.IsMatch(id, @"^\d{10}$"))
                return false;

            var check = Convert.ToInt32(id.Substring(9, 1));
            var sum = Enumerable.Range(0, 9)
                .Select(x => Convert.ToInt32(id.Substring(x, 1)) * (10 - x))
                .Sum() % 11;

            return sum < 2 && check == sum || sum >= 2 && check + sum == 11;
        }

        public static string SerializeXml<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var xmlserializer = new XmlSerializer(typeof(T));

            var stringWriter = new StringWriter();

            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, value);

                return stringWriter.ToString();
            }
        }

        public static string SerializeXml<T>(this T value, string rootName)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var xmlserializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

            var stringWriter = new StringWriter();

            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, value);

                return stringWriter.ToString();
            }
        }
    }
}