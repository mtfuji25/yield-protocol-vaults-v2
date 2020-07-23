using Analogueweb.Mvc.Utilities;
using DataManager.Models.CarraraSQL;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataManager.Controllers
{
    [Authorize(Roles = "Admin,Precast Manager,Ready Mix Manager,Dispatch Manager")]
    public class TimeController : DataManagerController
    {
        #region Methods
        public ActionResult Attendance()
        {
            return View();
        }

        public ActionResult AddAbsence()
        {
            return PartialView("_Absence", new TimeClockEntry());
        }

        public ActionResult AddTimeClockEntry()
        {
            return PartialView("_TimeClockEntry", new TimeClockEntry());
        }

        [HttpPost]
        public ActionResult AddTimeClockEntry(TimeClockEntry model)
        {
            try
            {
                CarraraSQL.TimeClockEntries.Add(model);
                CarraraSQL.SaveChanges();
                return Content("OK", "text/plain");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                }
                return Content(string.Concat("Whoops…", message), "text/plain");
            }
        }

        public ActionResult EditTimeCard(int guid)
        {
            TimeCard model = CarraraSQL.TimeCards.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Time Card with Id " + guid.ToString() + "!";
                return RedirectToAction("TimeCards", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit Time Card ID " + model.TimeCardID;
            return View(model);
        }

        public ActionResult EditTimeClockEntry(int guid)
        {
            TimeClockEntry model = CarraraSQL.TimeClockEntries.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                return Content(string.Concat("Whoops…Unable to Locate Time Clock Entry with Id ", guid.ToString(), "!"), "text/plain");
            }
            return PartialView("_TimeClockEntry", model);
        }

        [HttpPost]
        public ActionResult EditTimeClockEntry(TimeClockEntry model)
        {
            if (ModelState.IsValid)
            {
                TimeClockEntry entity = CarraraSQL.TimeClockEntries.Find(model.TimeClockEntryID);
                if (!entity.Approved && model.Approved)
                {
                    entity.ApprovedBy = AspNetUser.FullName;
                    entity.ApprovedDateTime = DateTime.Now;
                }
                else if (!model.Approved)
                {
                    entity.ApprovedBy = string.Empty;
                    entity.ApprovedDateTime = null;
                }
                try
                {
                    string converted = model.ClockInDate.Value.ToString("MM/dd/yy") + " " + model.ClockInTime.Value.ToString("h:mm:ss tt");
                    DateTime convertedDate = Convert.ToDateTime(converted);
                    entity.ClockIn = convertedDate;
                }
                catch { }
                try
                {
                    string converted = model.ClockOutDate.Value.ToString("MM/dd/yy") + " " + model.ClockOutDate.Value.ToString("h:mm:ss tt");
                    DateTime convertedDate = Convert.ToDateTime(converted);
                    entity.ClockOut = convertedDate;
                }
                catch { }
                entity.HoursWorked = model.HoursWorked;
                entity.CostCenterID = model.CostCenterID;
                entity.DrivenVehicleID = model.DrivenVehicleID;
                entity.Lunch = model.Lunch;
                entity.Notes = model.Notes;
                CarraraSQL.SaveChanges();
                return Content(string.Concat(
                    "<td>", entity.Employee.LastName, ", ", entity.Employee.FirstName, "</td>",
                    "<td class=\"numeric\">", entity.StartTime.HasValue ? entity.StartTime.Value.ToShortTimeString() : string.Empty, "</td>",
                    "<td class=\"numeric\">", entity.DayOfWeek, "</td>",
                    "<td class=\"numeric\">", entity.ClockIn, "</td>",
                    "<td class=\"numeric\">", entity.OriginalClockIn, "</td>",
                    "<td class=\"numeric\">",
                    entity.Lunch ?
                        string.Concat("<input checked=\"checked\" id=\"lunch_", entity.TimeClockEntryID, "\" name=\"lunch_", entity.TimeClockEntryID, "\" readonly=\"readonly\" type=\"checkbox\" />") :
                        string.Concat("<input id=\"lunch_", entity.TimeClockEntryID, "\" name=\"lunch_", entity.TimeClockEntryID, "\" readonly=\"readonly\" type=\"checkbox\" />"),
                    "</td>",
                    "<td class=\"numeric\">", entity.ClockOut, "</td>",
                    "<td class=\"numeric\">", entity.OriginalClockOut, "</td>",
                    "<td class=\"numeric\">", entity.HoursWorked.HasValue ? entity.HoursWorked.Value.ToString("F2") : string.Empty, "</td>",
                    "<td>", entity.Employee.DepartmentID.HasValue ? entity.Employee.Department.DepartmentName : string.Empty, " - ", entity.CostCenterID, "</td>",
                    "<td>", entity.DrivenVehicleID.HasValue ? entity.Vehicle.VehicleCode : string.Empty, "</td>",
                    "<td class=\"numeric\">",
                    entity.ApprovedDateTime.HasValue ?
                        string.Concat("<input checked=\"checked\" id=\"approved_", entity.TimeClockEntryID, "\" name=\"approved_", entity.TimeClockEntryID, "\" readonly=\"readonly\" type=\"checkbox\" />") :
                        string.Concat("<input id=\"approved_", entity.TimeClockEntryID, "\" name=\"approved_", entity.TimeClockEntryID, "\" readonly=\"readonly\" type=\"checkbox\" />"),
                    "</td>",
                    "<td class=\"numeric\">", entity.ApprovedDateTime, "</td>",
                    "<td>", entity.ApprovedBy, "</td>",
                    "<td class=\"numeric\">", entity.Notes, "</td>",
                    "<td class=\"numeric error\">", entity.Status, "</td>"
                ), "text/plain");
            }
            return ReturnError();
        }

        public ActionResult EmployeeList()
        {
            return View();
        }

        public ActionResult Index(DateTime? begin, int? department, DateTime? end, int? employee, bool? errors, bool? incomplete, string order, string range, string sort)
        {
            /*ISSUES 
             * Filter by role via department, need map o department
             * Cost Cetner ID to deprtment mapping
             * Sorting
             */


            Expression<Func<TimeClockEntry, bool>> predicate = PredicateBuilder.New<TimeClockEntry>(true);

            DateTime today = DateTime.Today;
            DateTime startDate = today.AddDays(-((int)today.DayOfWeek)).AddDays(-7);
            DateTime endDate = startDate.AddDays(7).AddSeconds(-1);
            int quarterNumber = (today.Month - 1) / 3 + 1;
            DateTime firstDayOfQuarter = new DateTime(today.Year, 3 * quarterNumber - 2, 1);

            if (string.IsNullOrEmpty(range))
            {
                range = "week";
            }
            switch (range)
            {
                case "yesterday":
                    startDate = today.Date.AddDays(-1);
                    endDate = today.AddSeconds(-1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;

                case "last-week":
                    startDate = today.AddDays(-((int)today.DayOfWeek)).AddDays(-7);
                    endDate = startDate.AddDays(7).AddSeconds(-1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;

                case "last-month":
                    startDate = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;

                case "last-quarter":
                    startDate = firstDayOfQuarter.AddMonths(-3);
                    endDate = firstDayOfQuarter.AddSeconds(-1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;

                case "last-year":
                    startDate = new DateTime(today.AddYears(-1).Year, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;

                case "today":
                    startDate = today.Date;
                    endDate = today.AddDays(1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;

                case "week":
                    startDate = today.AddDays(-((int)today.DayOfWeek));
                    endDate = startDate.AddDays(7).AddSeconds(-1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;

                case "month":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;

                case "quarter":
                    startDate = firstDayOfQuarter;
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;

                case "year":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;

                case "custom":
                    startDate = begin ?? today;
                    endDate = (end ?? today).AddDays(1).AddSeconds(-1);
                    predicate = x => (x.ClockIn.HasValue && x.ClockIn.Value >= startDate && x.ClockIn.Value < endDate);
                    break;
            }

            if (department.HasValue)
            {
                predicate = predicate.And(x => x.Employee.DepartmentID.HasValue && x.Employee.DepartmentID.Value == department.Value);
            }

            if (employee.HasValue)
            {
                predicate = predicate.And(x => x.EmployeeID == employee.Value);
            }

            if (errors.HasValue && errors.Value)
            {
                // No cost center or clocked in before start time
                predicate = predicate.And(x => !string.IsNullOrEmpty(x.Status));
            }

            if (incomplete.HasValue && incomplete.Value)
            {
                // Missing clock in or clock out value
                predicate = predicate.And(x => !x.ClockOut.HasValue || !x.ClockIn.HasValue);
            }

            // Run the query
            IEnumerable<TimeClockEntry> model = CarraraSQL.TimeClockEntries.AsNoTracking().AsExpandable().Where(predicate).ToList();

            IEnumerable<TimeClockEntryView> view = from entry in model
                                                   group entry by entry.DepartmentId into grouping
                                                   let name = grouping.First().Department
                                                   let hours = grouping.Select(y => y.HoursWorked).Sum()

                                                   select new TimeClockEntryView()
                                                   {
                                                       Department = name,
                                                       HoursWorked = hours,
                                                       TimeClockEntries = grouping
                                                   };
            // Sort
            /*  var query = from tuple in tuples
                orderby tuple.P1
                group tuple.P2 by tuple.P1 into g
                select new { Group = g.Key, Elements = g.OrderByDescending(p2 => p2) };*/

            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Time Clock Manager";
            ViewBag.Columns = GetColumns<TimeClockEntry>();
            return View(view.OrderBy(x => x.Department));
        }

        public ActionResult TimeCards(bool? hourly, string order, string sort, DateTime? week)
        {
            DateTime StartOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            DateTime EndOfWeek = week ?? StartOfWeek.AddDays(-1);

            IEnumerable<TimeCard> model = CarraraSQL.TimeCards.AsNoTracking().Where(x => x.WeekEndDate.HasValue && x.WeekEndDate.Value == EndOfWeek);

            // Filter
            if (hourly.HasValue && hourly.Value)
            {
                model = model.Where(x => x.Employee.IsHourly);
            }

            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "employee" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "dept-code":
                    model = asc ? model.OrderBy(x => x.DeptCode) : model.OrderByDescending(x => x.DeptCode);
                    break;
                case "department":
                    model = asc ? model.OrderBy(x => x.Department) : model.OrderByDescending(x => x.Department);
                    break;
                case "employee":
                    model = asc ? model.OrderBy(x => x.Employee.LastName).ThenBy(x => x.Employee.FirstName) : model.OrderByDescending(x => x.Employee.LastName).ThenByDescending(x => x.Employee.FirstName);
                    break;
                case "status":
                    model = asc ? model.OrderBy(x => x.Status) : model.OrderByDescending(x => x.Status);
                    break;
                case "hourly":
                    model = asc ? model.OrderBy(x => x.Hourly) : model.OrderByDescending(x => x.Hourly);
                    break;
                case "regular":
                    model = asc ?
                        model.OrderByDescending(x => x.RegularHours.HasValue).ThenByDescending(x => x.RegularHours) :
                        model.OrderByDescending(x => x.RegularHours.HasValue).ThenBy(x => x.RegularHours);
                    break;
                case "overtime":
                    model = asc ?
                        model.OrderByDescending(x => x.OvertimeHours.HasValue).ThenByDescending(x => x.OvertimeHours) :
                        model.OrderByDescending(x => x.OvertimeHours.HasValue).ThenBy(x => x.OvertimeHours);
                    break;
                case "vacation":
                    model = asc ?
                        model.OrderByDescending(x => x.VacationHours.HasValue).ThenByDescending(x => x.VacationHours) :
                        model.OrderByDescending(x => x.VacationHours.HasValue).ThenBy(x => x.VacationHours);
                    break;
                case "holiday":
                    model = asc ?
                        model.OrderByDescending(x => x.HolidayHours.HasValue).ThenByDescending(x => x.HolidayHours) :
                        model.OrderByDescending(x => x.HolidayHours.HasValue).ThenBy(x => x.HolidayHours);
                    break;
                case "total":
                    model = asc ?
                        model.OrderByDescending(x => x.TotalHours.HasValue).ThenByDescending(x => x.TotalHours) :
                        model.OrderByDescending(x => x.TotalHours.HasValue).ThenBy(x => x.TotalHours);
                    break;
                case "total-unapproved":
                    model = asc ?
                        model.OrderByDescending(x => x.TotalUnaapproved.HasValue).ThenByDescending(x => x.TotalUnaapproved) :
                        model.OrderByDescending(x => x.TotalUnaapproved.HasValue).ThenBy(x => x.TotalUnaapproved);
                    break;
            }

            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Time Cards";
            ViewBag.Columns = GetColumns<TimeCard>();
            return View(model.ToList());
        }

        public ActionResult VacationSummary()
        {
            return View();
        }
        #endregion
    }
}
