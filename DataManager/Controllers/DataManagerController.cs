using Analogueweb.Mvc;
using DataManager.Models;
using DataManager.Models.CarraraSQL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataManager.Controllers
{
    public class DataManagerController : PageController
    {
        #region Public Fields
        public AspNetUser AspNetUser;
        public string AssemblyVersion;
        public DataContext DataContext = DataContext.Create();
        public CarraraSQL CarraraSQL = CarraraSQL.Create();
        public string IndexDirectory = HostingEnvironment.MapPath("~/App_Data/Lucene/");
        public Stopwatch stopWatch = new Stopwatch();
        public ViewSetting ViewSetting;
        #endregion

        #region Private Fields
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        #endregion

        #region Methods
        public DataManagerController() { }

        public DataManagerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public void AddErrors(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public List<SelectListItem> GetColumns<T>()
        {
            List<string> selected = new List<string>();
            bool hasColumnSettings = ViewSetting != null && !string.IsNullOrEmpty(ViewSetting.Columns);
            if (hasColumnSettings)
            {
                selected = ViewSetting.Columns.Split(',').ToList();
            }
            return (from t in typeof(T).GetProperties()
                    where Attribute.IsDefined(t, typeof(System.ComponentModel.DisplayNameAttribute))
                    let d = (t.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), true)[0] as System.ComponentModel.DisplayNameAttribute)
                    select new SelectListItem
                    {
                        Selected = hasColumnSettings ? selected.Contains(t.Name) : true,
                        Text = d.DisplayName,
                        Value = t.Name
                    }).ToList();
        }

        public string GetIP()
        {
            string forwardedFor = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                string[] addresses = forwardedFor.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses.Last().Trim();
                }
            }
            return Request.ServerVariables["REMOTE_ADDR"];
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            stopWatch.Start();
            AssemblyVersion = Assembly.LoadFrom(HostingEnvironment.MapPath("~/bin/DataManager.dll")).GetName().Version.ToString();
            if (Request.IsAuthenticated)
            {
                AspNetUser = UserManager.FindById(User.Identity.GetUserId());
                Uri uri = Request.Url;

                if (!Request.IsAjaxRequest() &&
                    !uri.PathAndQuery.StartsWith("/Account") &&
                    !uri.PathAndQuery.StartsWith("/Error") &&
                    !uri.PathAndQuery.StartsWith("/Home") &&
                    !uri.PathAndQuery.StartsWith("/Search") &&
                    !uri.PathAndQuery.StartsWith("/Suggest") &&
                    !uri.PathAndQuery.Contains("export=") &&
                    !uri.PathAndQuery.Contains("save=false") &&
                    !uri.PathAndQuery.Contains("Masquerade") && uri.PathAndQuery != "/")
                {
                    string path = uri.AbsolutePath;
                    ViewSetting = AspNetUser.ViewSettings.FirstOrDefault(x => x.Path.ToLower() == path.ToLower());

                    // Redirect if this is a queryless path and we have a prior setting
                    if (string.IsNullOrEmpty(uri.Query) && ViewSetting != null && !string.IsNullOrEmpty(ViewSetting.PathAndQuery) && ViewSetting.Path.ToLower() != ViewSetting.PathAndQuery.ToLower())
                    {
                        // Only redirect if we have a matching setting
                        requestContext.HttpContext.Response.Clear();
                        requestContext.HttpContext.Response.Redirect(ViewSetting.PathAndQuery);
                        requestContext.HttpContext.Response.End();
                    }
                    else
                    {
                        // Store the current page setting
                        if (ViewSetting == null)
                        {
                            ViewSetting = new ViewSetting
                            {
                                AspNetUserId = AspNetUser.Id,
                                Path = path,
                                PathAndQuery = uri.PathAndQuery,
                            };
                            DataContext.ViewSettings.Add(ViewSetting);
                        }
                        else
                        {
                            ViewSetting.PathAndQuery = uri.PathAndQuery;
                            DataContext.Entry(ViewSetting).State = EntityState.Modified;
                        }
                        DataContext.SaveChanges();
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            CarraraSQL.Dispose();
            DataContext.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewBag.AspNetUser = AspNetUser;
            ViewBag.AssemblyVersion = AssemblyVersion;

            ViewBag.DateRanges = new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "- Date Range -" },
                new SelectListItem { Text = "Yesterday", Value = "yesterday" },
                new SelectListItem { Text = "Last Week", Value = "last-week" },
                new SelectListItem { Text = "Last Month", Value = "last-month" },
                new SelectListItem { Text = "Last Quarter", Value = "last-quarter" },
                new SelectListItem { Text = "Last Year", Value = "last-year" },
                new SelectListItem { Text = "Today", Value = "today" },
                new SelectListItem { Text = "This Week", Value = "week" },
                new SelectListItem { Text = "This Month", Value = "month" },
                new SelectListItem { Text = "This Quarter", Value = "quarter" },
                new SelectListItem { Text = "This Year", Value = "year" },
                new SelectListItem { Text = "Custom", Value = "custom" }
            };

            List<SelectListItem> Beds = HttpContext.Cache["BedsSelectList"] as List<SelectListItem>;
            if (Beds == null)
            {
                Beds = CarraraSQL.Beds.AsNoTracking().OrderBy(x => x.BedName).Select(x => new SelectListItem { Value = x.BedID.ToString(), Text = x.BedName }).ToList();
                Beds.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Bed -" });
                HttpContext.Cache["BedsSelectList"] = Beds;
            }
            ViewBag.Beds = Beds;

            List<SelectListItem> ContactTypes = HttpContext.Cache["ContactTypesSelectList"] as List<SelectListItem>;
            if (ContactTypes == null)
            {
                ContactTypes = CarraraSQL.ContactTypes.AsNoTracking().OrderBy(x => x.ContactTypeName).Select(x => new SelectListItem { Value = x.ContactTypeID.ToString(), Text = x.ContactTypeName }).ToList();
                ContactTypes.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Contact Type -" });
                HttpContext.Cache["ContactTypesSelectList"] = ContactTypes;
            }
            ViewBag.ContactTypes = ContactTypes;

            List<SelectListItem> Departments = HttpContext.Cache["DepartmentsSelectList"] as List<SelectListItem>;
            if (Departments == null)
            {
                Departments = CarraraSQL.Departments.AsNoTracking().OrderBy(x => x.DepartmentName).Select(x => new SelectListItem { Value = x.DepartmentID.ToString(), Text = x.DepartmentName }).ToList();
                Departments.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Department -" });
                HttpContext.Cache["DepartmentsSelectList"] = Departments;
            }
            ViewBag.Departments = Departments;

            List<SelectListItem> Drivers = HttpContext.Cache["DriversSelectList"] as List<SelectListItem>;
            if (Drivers == null)
            {
                Drivers = CarraraSQL.Employees.AsNoTracking().Where(x => x.EmployeeStatusID.HasValue && x.EmployeeStatusID == 1 && x.IsDriver).Select(x => new SelectListItem { Value = x.EmployeeID.ToString(), Text = x.LastName + ", " + x.FirstName }).ToList();
                Drivers.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Driver -" });
                HttpContext.Cache["DriversSelectList"] = Drivers;
            }
            ViewBag.Drivers = Drivers;

            List<SelectListItem> Employees = HttpContext.Cache["EmployeesSelectList"] as List<SelectListItem>;
            if (Employees == null)
            {
                Employees = CarraraSQL.Employees.AsNoTracking().Where(x => x.EmployeeStatusID.HasValue && x.EmployeeStatusID == 1).Select(x => new SelectListItem { Value = x.EmployeeID.ToString(), Text = x.LastName + ", " + x.FirstName }).ToList();
                Employees.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Employee -" });
                HttpContext.Cache["EmployeesSelectList"] = Employees;
            }
            ViewBag.Employees = Employees;

            List<SelectListItem> EmployeeStatuses = HttpContext.Cache["EmployeeStatusSelectList"] as List<SelectListItem>;
            if (EmployeeStatuses == null)
            {
                EmployeeStatuses = CarraraSQL.EmployeeStatuses.AsNoTracking().OrderBy(x => x.EmployeeStatusName).Select(x => new SelectListItem { Value = x.EmployeeStatusID.ToString(), Text = x.EmployeeStatusName }).ToList();
                EmployeeStatuses.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Employee Status -" });
                HttpContext.Cache["EmployeeStatusSelectList"] = EmployeeStatuses;
            }
            ViewBag.EmployeeStatuses = EmployeeStatuses;

            List<SelectListItem> IndependantDrivers = HttpContext.Cache["IndependantDriversSelectList"] as List<SelectListItem>;
            if (IndependantDrivers == null)
            {
                IndependantDrivers = CarraraSQL.Contacts.AsNoTracking().Where(x => x.ContactTypeID == 7).OrderBy(x => x.DisplayName).Select(x => new SelectListItem { Value = x.ContactID.ToString(), Text = x.DisplayName }).ToList();
                IndependantDrivers.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Driver -" });
                HttpContext.Cache["IndependantDriversSelectList"] = IndependantDrivers;
            }
            ViewBag.IndependantDrivers = IndependantDrivers;

            List<SelectListItem> Jobs = HttpContext.Cache["JobsSelectList"] as List<SelectListItem>;
            if (Jobs == null)
            {
                Jobs = CarraraSQL.Jobs.AsNoTracking().Where(x => x.JobStatusID == 1).OrderBy(x => x.JobName).Select(x => new SelectListItem { Value = x.JobID.ToString(), Text = x.JobName }).ToList();
                Jobs.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Job -" });
                HttpContext.Cache["JobsSelectList"] = Jobs;
            }
            ViewBag.Jobs = Jobs;

            List<SelectListItem> JobStatuses = HttpContext.Cache["JobStatusSelectList"] as List<SelectListItem>;
            if (JobStatuses == null)
            {
                JobStatuses = CarraraSQL.JobStatus.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.JobStatusName).Select(x => new SelectListItem { Value = x.JobStatusID.ToString(), Text = x.JobStatusName }).ToList();
                JobStatuses.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Job Status -" });
                HttpContext.Cache["JobStatusSelectList"] = JobStatuses;
            }
            ViewBag.JobStatuses = JobStatuses;

            List<SelectListItem> LoadStatuses = HttpContext.Cache["LoadStatusSelectList"] as List<SelectListItem>;
            if (LoadStatuses == null)
            {
                LoadStatuses = CarraraSQL.LoadStatus.AsNoTracking().OrderBy(x => x.LoadStatusName).Select(x => new SelectListItem { Value = x.LoadStatusID.ToString(), Text = x.LoadStatusName }).ToList();
                LoadStatuses.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Load Status -" });
                HttpContext.Cache["LoadStatusSelectList"] = LoadStatuses;
            }
            ViewBag.LoadStatuses = LoadStatuses;

            List<SelectListItem> Locations = HttpContext.Cache["LocationsSelectList"] as List<SelectListItem>;
            if (Locations == null)
            {
                Locations = CarraraSQL.Locations.AsNoTracking().OrderBy(x => x.LocationName).Select(x => new SelectListItem { Value = x.LocationID.ToString(), Text = x.LocationName }).ToList();
                Locations.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Location -" });
                HttpContext.Cache["LocationsSelectList"] = Locations;
            }
            ViewBag.Locations = Locations;

            List<SelectListItem> MarkSizeTypes = HttpContext.Cache["MarkSizeTypesSelectList"] as List<SelectListItem>;
            if (MarkSizeTypes == null)
            {
                MarkSizeTypes = CarraraSQL.MarkSizeTypes.AsNoTracking().OrderBy(x => x.MarkSizeTypeName).Select(x => new SelectListItem { Value = x.MarkSizeTypeID.ToString(), Text = x.MarkSizeTypeName }).ToList();
                MarkSizeTypes.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Mark Size Type -" });
                HttpContext.Cache["MarkSizeTypesSelectList"] = MarkSizeTypes;
            }
            ViewBag.MarkSizeTypes = MarkSizeTypes;

            List<SelectListItem> MarkTypes = HttpContext.Cache["MarkTypesSelectList"] as List<SelectListItem>;
            if (MarkTypes == null)
            {
                MarkTypes = CarraraSQL.MarkTypes.AsNoTracking().OrderBy(x => x.MarkTypeName).Select(x => new SelectListItem { Value = x.MarkTypeID.ToString(), Text = x.MarkTypeName }).ToList();
                MarkTypes.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Mark Type -" });
                HttpContext.Cache["MarkTypesSelectList"] = MarkTypes;
            }
            ViewBag.MarkTypes = MarkTypes;

            List<SelectListItem> MeterTypes = HttpContext.Cache["MeterTypesSelectList"] as List<SelectListItem>;
            if (MeterTypes == null)
            {
                MeterTypes = CarraraSQL.MeterTypes.AsNoTracking().OrderBy(x => x.MeterTypeName).Select(x => new SelectListItem { Value = x.MeterTypeID.ToString(), Text = x.MeterTypeName }).ToList();
                MeterTypes.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Meter Type -" });
                HttpContext.Cache["MeterTypesSelectList"] = MeterTypes;
            }
            ViewBag.MeterTypes = MeterTypes;

            List<SelectListItem> Mixes = HttpContext.Cache["MixesSelectList"] as List<SelectListItem>;
            if (Mixes == null)
            {
                Mixes = CarraraSQL.Mixes.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.MixName).Select(x => new SelectListItem { Value = x.MixID.ToString(), Text = x.MixName }).ToList();
                Mixes.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Mix -" });
                HttpContext.Cache["MixesSelectList"] = Mixes;
            }
            ViewBag.Mixes = Mixes;

            List<SelectListItem> PourStatuses = HttpContext.Cache["PourStatusSelectList"] as List<SelectListItem>;
            if (PourStatuses == null)
            {
                PourStatuses = CarraraSQL.PourStatus.AsNoTracking().OrderBy(x => x.PourStatusName).Select(x => new SelectListItem { Value = x.PourStatusID.ToString(), Text = x.PourStatusName }).ToList();
                PourStatuses.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Pour Status -" });
                HttpContext.Cache["PourStatusSelectList"] = PourStatuses;
            }
            ViewBag.PourStatuses = PourStatuses;

            List<IdentityRole> Roles = DataContext.Roles.ToList();
            ViewBag.Roles = Roles;

            List<SelectListItem> RolesList = HttpContext.Cache["RolesSelectList"] as List<SelectListItem>;
            if (RolesList == null)
            {
                RolesList = Roles.OrderBy(r => r.Name).Select(x => new SelectListItem { Value = x.Name, Text = x.Name }).ToList();
                RolesList.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Role -" });
                HttpContext.Cache["RolesSelectList"] = RolesList;
            }
            ViewBag.RolesList = RolesList;

            List<SelectListItem> Routes = HttpContext.Cache["RoutesSelectList"] as List<SelectListItem>;
            if (Routes == null)
            {
                Routes = CarraraSQL.Routes.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.RouteName).Select(x => new SelectListItem { Value = x.RouteID.ToString(), Text = x.RouteName }).ToList();
                Routes.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Route -" });
                HttpContext.Cache["RoutesSelectList"] = Routes;
            }
            ViewBag.Routes = Routes;
            /*
            List<SelectListItem> States = HttpContext.Cache["StatesSelectList"] as List<SelectListItem>;
            if (States == null)
            {
                States = CarraraSQL.States.AsNoTracking().OrderBy(x => x.StateName).Select(x => new SelectListItem { Value = x.StateID.ToString(), Text = x.StateName }).ToList();
                States.Insert(0, new SelectListItem { Value = string.Empty, Text = "- State -" });
                HttpContext.Cache["StatesSelectList"] = States;
            }
            ViewBag.States = States;

            List<SelectListItem> StatesAbbr = HttpContext.Cache["StatesAbbrSelectList"] as List<SelectListItem>;
            if (StatesAbbr == null)
            {
                StatesAbbr = CarraraSQL.States.AsNoTracking().OrderBy(x => x.StateAbbreviation).Select(x => new SelectListItem { Value = x.StateAbbreviation.Trim(), Text = x.StateAbbreviation.Trim() }).ToList();
                StatesAbbr.Insert(0, new SelectListItem { Value = string.Empty, Text = "- State -" });
                HttpContext.Cache["StatesAbbrSelectList"] = StatesAbbr;
            }
            ViewBag.StatesAbbr = StatesAbbr;
            */
            List<SelectListItem> Tractors = HttpContext.Cache["TractorsSelectList"] as List<SelectListItem>;
            if (Tractors == null)
            {
                int[] tractorTypes = new int[] { 4, 11, 49, 55 };
                Tractors = CarraraSQL.Vehicles.AsNoTracking().Where(x => x.IsActive && tractorTypes.Contains(x.VehicleTypeID)).OrderBy(x => x.VehicleCode).Select(x => new SelectListItem { Value = x.VehicleID.ToString(), Text = x.VehicleCode }).ToList();
                Tractors.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Tractors -" });
                HttpContext.Cache["TractorsSelectList"] = Tractors;
            }
            ViewBag.Tractors = Tractors;

            List<SelectListItem> Trailers = HttpContext.Cache["TrailersSelectList"] as List<SelectListItem>;
            if (Trailers == null)
            {
                int[] trailerTypes = new int[] { 12 };
                Trailers = CarraraSQL.Vehicles.AsNoTracking().Where(x => x.IsActive && trailerTypes.Contains(x.VehicleTypeID)).OrderBy(x => x.VehicleCode).Select(x => new SelectListItem { Value = x.VehicleID.ToString(), Text = x.VehicleCode }).ToList();
                Trailers.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Trailer -" });
                HttpContext.Cache["TrailersSelectList"] = Trailers;
            }
            ViewBag.Trailers = Trailers;

            List<SelectListItem> TrailerTypes = HttpContext.Cache["TrailerTypesSelectList"] as List<SelectListItem>;
            if (TrailerTypes == null)
            {
                TrailerTypes = CarraraSQL.TrailerTypes.AsNoTracking().OrderBy(x => x.TrailerTypeName).Select(x => new SelectListItem { Value = x.TrailerTypeID.ToString(), Text = x.TrailerTypeName }).ToList();
                TrailerTypes.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Trailer Type -" });
                HttpContext.Cache["TrailerTypesSelectList"] = TrailerTypes;
            }
            ViewBag.TrailerTypes = TrailerTypes;

            List<SelectListItem> Vehicles = HttpContext.Cache["VehiclesSelectList"] as List<SelectListItem>;
            if (Vehicles == null)
            {
                Vehicles = CarraraSQL.Vehicles.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.VehicleCode).Select(x => new SelectListItem { Value = x.VehicleID.ToString(), Text = x.VehicleCode }).ToList();
                Vehicles.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Vehicle -" });
                HttpContext.Cache["VehiclesSelectList"] = Vehicles;
            }
            ViewBag.Vehicles = Vehicles;

            List<SelectListItem> VehicleTypes = HttpContext.Cache["VehicleTypesSelectList"] as List<SelectListItem>;
            if (VehicleTypes == null)
            {
                VehicleTypes = CarraraSQL.VehicleTypes.AsNoTracking().OrderBy(x => x.VehicleTypeName).Select(x => new SelectListItem { Value = x.VehicleTypeID.ToString(), Text = x.VehicleTypeName }).ToList();
                VehicleTypes.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Vehicle Type -" });
                HttpContext.Cache["VehicleTypesSelectList"] = VehicleTypes;
            }
            ViewBag.VehicleTypes = VehicleTypes;

            List<SelectListItem> WeightFormulaTypes = HttpContext.Cache["WeightFormulaTypesSelectList"] as List<SelectListItem>;
            if (WeightFormulaTypes == null)
            {
                WeightFormulaTypes = CarraraSQL.WeightFormulaTypes.AsNoTracking().OrderBy(x => x.WeightFormulaTypeName).Select(x => new SelectListItem { Value = x.WeightFormulaTypeID.ToString(), Text = x.WeightFormulaTypeName }).ToList();
                WeightFormulaTypes.Insert(0, new SelectListItem { Value = string.Empty, Text = "- Weight Formula Type -" });
                HttpContext.Cache["WeightFormulaTypesSelectList"] = WeightFormulaTypes;
            }
            ViewBag.WeightFormulaTypes = WeightFormulaTypes;

            stopWatch.Stop();
            ViewBag.Elapsed = stopWatch.Elapsed;
        }

        public ActionResult ReturnError()
        {
            StringBuilder builder = new StringBuilder("Whoops…");
            foreach (KeyValuePair<string, ModelState> pair in ModelState)
            {
                if (pair.Value.Errors.Count > 0)
                {
                    builder.Append(pair.Value.Errors[0].ErrorMessage + "<br />");
                }
            }
            return Content(builder.ToString(), "text/plain");
        }

        public async Task SendMail(string body, string subject, string to, string alt)
        {
            MailMessage mail = new MailMessage(new MailAddress("info@jpcarrara.com"), new MailAddress(to))
            {
                Subject = subject,
                Body = body + Signature(),
                IsBodyHtml = true
            };
            if (!string.IsNullOrEmpty(alt))
            {
                mail.CC.Add(new MailAddress(alt));
            }
            using (var client = new SmtpClient())
            {
                await client.SendMailAsync(mail);
            }
        }

        public string Signature()
        {
            Uri url = Request.Url;
            return string.Concat("<p>Sincerely,<br />", Site.Name, "<br />", Site.Phone, "<br />", "<a href=\"" + url.GetLeftPart(UriPartial.Authority) + "\">" + url.Host + "</a>", "</p>");
        }
        #endregion

        #region Properties
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion
    }
}