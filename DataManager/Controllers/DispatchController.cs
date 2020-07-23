using Analogueweb.Mvc.Utilities;
using DataManager.Models;
using DataManager.Models.CarraraSQL;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace DataManager.Controllers
{
    [Authorize(Roles = "Admin,Precast Manager,Precast Production,Project Manager,Ready Mix Manager,Dispatch Manager,Shop")]
    public class DispatchController : DataManagerController
    {
        #region Methods
        public ActionResult AddLoad()
        {
            return PartialView("_Load", new Load
            {
                DeliveryDate =  DateTime.Today,
                DepartmentID = 4
            });
        }

        [HttpPost]
        public ActionResult AddLoad(Load model)
        {
            try
            {
                CarraraSQL.Loads.Add(model);
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

        [HttpPost]
        public ActionResult DeleteLoad(int guid)
        {
            try
            {
                Load load = CarraraSQL.Loads.Find(guid);
                CarraraSQL.Loads.Remove(load);
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

        [HttpPost]
        public ActionResult EditLoad(Load model)
        {
            try
            {
                Load entity = CarraraSQL.Loads.Find(model.LoadID);
                entity.DeliveryDate = model.DeliveryDate;
                entity.DeliveryTime = model.DeliveryTime;
                entity.SiteDepartureTime = model.SiteDepartureTime;
                entity.DriverID = model.DriverID;
                entity.IndependentDriverID = model.IndependentDriverID;
                entity.Notes = model.Notes;
                entity.HasDriverBeenContacted = model.HasDriverBeenContacted;
                entity.LoadNumber = model.LoadNumber;
                entity.VehicleID = model.VehicleID;
                entity.JobID = model.JobID;
                entity.IsLoaded = model.IsLoaded;
                entity.LoadStatusID = model.LoadStatusID;
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

        public ActionResult Index(DateTime? begin, DateTime? end, int? job, string order, string range, string sort, int? status)
        {
            Expression<Func<Load, bool>> predicate = PredicateBuilder.New<Load>(true);
            if (job.HasValue)
            {
                Job entity = CarraraSQL.Jobs.Find(job);
                if (entity == null)
                {
                    TempData["Page.Title"] = "Unable to Locate Job with Id " + job.Value.ToString() + "!";
                    return RedirectToAction("Index", Request.QueryString.ToRouteValues());
                }
                predicate = x => x.JobID == job;
                Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Loads for " + entity.JobName;
            }
            else
            {
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
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;

                    case "last-week":
                        startDate = today.AddDays(-((int)today.DayOfWeek)).AddDays(-7);
                        endDate = startDate.AddDays(7).AddSeconds(-1);
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;

                    case "last-month":
                        startDate = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, 1);
                        endDate = startDate.AddMonths(1).AddSeconds(-1);
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;

                    case "last-quarter":
                        startDate = firstDayOfQuarter.AddMonths(-3);
                        endDate = firstDayOfQuarter.AddSeconds(-1);
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;

                    case "last-year":
                        startDate = new DateTime(today.AddYears(-1).Year, 1, 1);
                        endDate = startDate.AddYears(1).AddSeconds(-1);
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;

                    case "today":
                        startDate = today.Date;
                        endDate = today.AddDays(1);
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;

                    case "week":
                        startDate = today.AddDays(-((int)today.DayOfWeek));
                        endDate = startDate.AddDays(7).AddSeconds(-1);
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;

                    case "month":
                        startDate = new DateTime(today.Year, today.Month, 1);
                        endDate = startDate.AddMonths(1).AddSeconds(-1);
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;

                    case "quarter":
                        startDate = firstDayOfQuarter;
                        endDate = startDate.AddMonths(3).AddSeconds(-1);
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;

                    case "year":
                        startDate = new DateTime(today.Year, 1, 1);
                        endDate = startDate.AddYears(1).AddSeconds(-1);
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;

                    case "custom":
                        startDate = begin ?? today;
                        endDate = (end ?? today).AddDays(1).AddSeconds(-1);
                        predicate = x => (x.DeliveryDate.HasValue && x.DeliveryDate.Value >= startDate && x.DeliveryDate.Value < endDate);
                        break;
                }
                Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Dispatch";
            }

            if (status.HasValue)
            {
                predicate = predicate.And(x => x.LoadStatusID == status);
            }

            // Run the query
            IEnumerable<Load> model = CarraraSQL.Loads.AsNoTracking().AsExpandable().Where(predicate).ToList();

            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "delivery-date" : sort;
            bool asc = order == "asc";
            // TODO: Sorting

            // Return Results
            return View(model);
        }

        [Authorize(Roles = "Admin,Precast Manager,Precast Production,Project Manager,Ready Mix Manager,Dispatch Manager")]
        public ActionResult ShippingSchedule(DateTime? begin)
        {
            Expression<Func<Load, bool>> predicate = PredicateBuilder.New<Load>(true);

            DateTime startDate = begin ?? DateTime.Today.StartOfWeek(DayOfWeek.Monday);
            DateTime endDate = startDate.AddDays(7).AddSeconds(-1);
            predicate = x => (x.DeliveryDate >= startDate && x.DeliveryDate < endDate);

            // Run the query
            IEnumerable<Load> model = CarraraSQL.Loads.AsNoTracking().AsExpandable().Where(predicate).ToList();

            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Shipping Schedule";
            return View(model);
        }
        #endregion
    }
}