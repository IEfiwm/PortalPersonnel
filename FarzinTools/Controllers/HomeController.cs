using FarzinTools.Models;
using FarzinTools.Service;
using FarzinTools.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ngra.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FarzinTools.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static Dictionary<string, List<PersonsPerformViewModel>> _model = new Dictionary<string, List<PersonsPerformViewModel>>();
        public static int _count;
        public static DateTime? _importDate;
        static readonly eOrganizationEntities1 _context = new eOrganizationEntities1();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadWorkDay()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, byte? date, DateTime? performDate)
        {
            try
            {
                var userId = HttpContext.Request.Cookies["user_id"]?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    userId = Guid.NewGuid().ToString();

                    HttpContext.Response.AppendCookie(new HttpCookie("user_id", userId));
                }

                if (!User.IsInRole("admin"))
                    performDate = null;

                var excelModel = ExelReader.ReadXLS(file, date, performDate);

                Session["attendances"] = CommonHelper.Serialize(excelModel);
                
                _model.Remove(userId);

                _model.Add(userId, excelModel);

                if (performDate == null)
                {
                    _importDate = date == 0 ? DateTime.Now :
                                  date == 1 ? DateTime.Now.AddDays(-1) :
                                  date == 2 ? DateTime.Now.AddDays(-2) : (DateTime?)null;
                }
                else
                {
                    _importDate = performDate.Value;
                }

                return RedirectToAction("DisplayExcel");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult DisplayExcel()
        {
            //HttpContext.Response.AppendCookie(new HttpCookie("step", CommonHelper.Deserialize<List<PersonsPerformViewModel>>(Session["attendances"]?.ToString())?.FirstOrDefault()?.SerializeJson()));
           
            //return Json("Ok", JsonRequestBehavior.AllowGet);

            var userId = HttpContext.Request.Cookies["user_id"]?.Value;

            var model = _model?.Where(f => f.Key == userId).Select(f => f.Value).FirstOrDefault();

            if (model == null || !(model != null && model.Any()))
            {
                //HttpContext.Response.AppendCookie(new HttpCookie("step", "if true : " + Session["attendances"].ToString()));

                //return Json("Ok", JsonRequestBehavior.AllowGet);

                model = new List<PersonsPerformViewModel>();

                model = CommonHelper.Deserialize<List<PersonsPerformViewModel>>(Session["attendances"].ToString());

                //HttpContext.Response.AppendCookie(new HttpCookie("step", "Model:" + model.FirstOrDefault()?.SerializeJson()));

                //return Json("Ok", JsonRequestBehavior.AllowGet);

                if (model == null || !(model != null && model.Any()))

                    return RedirectToAction("UploadWorkDay");
            }

            ViewBag.ImportDate = _importDate.ToPersianDateTimeString();

            return View(model);
        }

        [HttpPost]
        public ActionResult ReadFile([DataSourceRequest] DataSourceRequest request)
        {
            var userId = HttpContext.Request.Cookies["user_id"]?.Value;

            var model = _model.Where(f => f.Key == userId).Select(f => f.Value).FirstOrDefault();

            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, PersonsPerformViewModel model)
        {
            var userId = HttpContext.Request.Cookies["user_id"]?.Value;

            var personPerform = _model.Where(f => f.Key == userId).Select(f => f.Value).FirstOrDefault();

            if (personPerform != null && personPerform.Any())
            {
                var item = personPerform.FirstOrDefault(f => f.Id == model.Id);

                if (item == null)
                {
                    return Json(_model.ToDataSourceResult(request, ModelState));
                }

                item.IsValid = true;

                item.ClassName = "";

                var personalCode = ExelReader.PersonalCodeValidation(model.PersonalCode);

                if (ExelReader.IsPersonalCodeExist(personalCode, model.PerformDate.Value))
                {
                    item.IsValid = false;

                    item.ClassName = "warning";

                    item.SummaryValidation = " رکورد تکراری";
                }

                var result = ExelReader.PersenaliIsValid(personalCode, item);

                if (!result)
                {
                    item.IsValid = false;

                    item.ClassName = "danger";

                    item.SummaryValidation = "کد پرسنلی نامعتبر میباشد";
                }

                item.PersonalCode = result ? personalCode : string.Empty;

                item.Name = model.Name;

                item.LastName = model.LastName;

                item.FatherName = model.FatherName;

                if (!_context.Entity_PersonelsInfo.Any(f => f.NationalID == model.NationalCode))
                {
                    item.IsValid = false;

                    item.ClassName = "danger";

                    item.SummaryValidation += "کد ملی در سیستم موجود نیست";

                    if (model.NationalCode.Length != 10)
                    {
                        item.SummaryValidation += "، فرمت کد ملی معتبر نیست";
                    }
                }

                item.NationalCode = model.NationalCode;

                item.ProjectCode = model.ProjectCode;

                var project = _context.Entity_BaseInfo.FirstOrDefault(f => f.TitleID == model.ProjectCode);

                if (project == null)
                {
                    item.IsValid = false;

                    model.ClassName = "danger";

                    model.SummaryValidation += "پروژه در سیستم موجود نیست";

                    item.ProjectText = string.Empty;
                }
                else
                {
                    item.ProjectText = project.Title ?? string.Empty;
                }

                item.PostText = model.PostText;

                item.PostCode = model.PostCode;

                item.AreaCode = model.AreaCode;

                item.JobGroup = model.JobGroup;

                item.ChildNo = model.ChildNo;

                item.Perform = model.Perform;

                item.Overtime = model.Overtime;

                item.NightWorkDay = model.NightWorkDay;

                item.NightWorkHour = model.NightWorkHour;

                item.HolidayWork = model.HolidayWork;

                item.Leave = model.Leave;

                item.OtherAdvantage = model.OtherAdvantage;

                item.Deductions = model.Deductions;

                item.Absence = model.Absence;
            }

            return Json(_model.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, PersonsPerformViewModel model)
        {
            var userId = HttpContext.Request.Cookies["user_id"]?.Value;

            var personPerforms = _model.Where(f => f.Key == userId).Select(f => f.Value).FirstOrDefault();

            var personPerform = personPerforms.FirstOrDefault(f => f.Id == model.Id);

            if (personPerform != null)
            {
                personPerforms.Remove(personPerform);
            }

            return Json(personPerforms.ToDataSourceResult(request, ModelState));
        }

        [HttpGet]
        public ActionResult Export()
        {
            foreach (var item in ExelReader._personsPerformList.Where(f => f.IsValid))
            {
                if (ExelReader.IsPersonalCodeExist(item.PersonalCode, item.PerformDate.Value)) continue;

                _context.Entity_PersonsPerform.Add(new Entity_PersonsPerform
                {
                    CategoryCode = -1,

                    RaitingHits = 0,

                    Grade = 1,

                    IsPreNote = false,

                    Locked = false,

                    LocalLock = false,

                    LocalLockUserCode = -1,

                    LocalLockRoleID = -1,

                    LockUserCode = -1,

                    LockRoleID = -1,

                    FieldsStatus = "<Fields></Fields>",

                    IsPrivateSearch = true,

                    PrivateSearchUserCode = 349,

                    PrivateSearchRoleID = 421,

                    EntityNumber = "1",

                    NumberOfCopies = 1,

                    Version = "0.1",

                    OriginalVersionCode = -1,

                    IsSigned = false,

                    SecurityLevelCode = 1,

                    IsConfirm = true,

                    IsActive = true,

                    FirstEntityCode = 1,

                    ImportEntityNumber = string.Empty,

                    ExportEntityNumber = string.Empty,

                    CreatorID = 349,

                    CreatorRoleID = 421,

                    CreationDate = DateTime.Now,

                    LastEditDate = DateTime.Now,

                    ImportDate = DateTime.Now,

                    PerformDate = item.PerformDate,

                    Perform = item.Perform,

                    Leave = item.Leave,

                    Absence = item.Absence,

                    overtime = item.Overtime,

                    HolidayWork = item.HolidayWork,

                    NightWorkDay = item.NightWorkDay,

                    ProjectText = item.ProjectText,

                    PersonalCode = item.PersonalCode,

                    Name = item.Name,

                    LastName = item.LastName,

                    FatherName = item.FatherName,

                    JobGroup = item.JobGroup,

                    ChildNo = item.ChildNo,

                    NightWorkHour = item.NightWorkHour,

                    OtherAdvantage = item.OtherAdvantage,

                    Deductions = item.Deductions,

                    ProjectCode = item.ProjectCode,

                    PostText = item.PostText,

                    PostCode = item.PostCode,

                    AreaCode = item.AreaCode
                });
            }

            _context.SaveChanges();

            var userId = HttpContext.Request.Cookies["user_id"]?.Value;

            var personPerform = _model.Where(f => f.Key == userId).Select(f => f.Value).FirstOrDefault();

            var valids = personPerform.Where(f => f.IsValid).ToList();

            valids.ForEach(model => personPerform.Remove(model));

            if (personPerform.Any())
            {
                return RedirectToAction("DisplayExcel");
            }
            else
            {
                return View("UploadWorkDay");
            }
        }

        //public ActionResult Test()
        //{
        //    return Json(_model.ToList(), JsonRequestBehavior.AllowGet);
        //}
    }
}