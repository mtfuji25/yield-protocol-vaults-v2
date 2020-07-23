using Analogueweb.Mvc.Utilities;
using DataManager.Models;
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
    public class ProductionController : DataManagerController
    {
        #region Methods
        [HttpPost]
        public ActionResult AddNonConformingReport(NonConformingReport model)
        {
            try
            {
                CarraraSQL.NonConformingReports.Add(model);
                CarraraSQL.SaveChanges();
                Pour pour = CarraraSQL.Pours.Find(model.PourID);
                return PartialView("_NonConformingReport", new NonConformingReport {
                    JobID = pour.DefaultJobID.Value,
                    PourID = pour.PourID
                });
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

        public ActionResult AddPour()
        {
            Page.Title = "Add a New Pour";
            return View("Manage", new Pour());
        }

        [HttpPost]
        public ActionResult AddPour(Pour model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.Pours.Add(model);
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Pour Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Pour Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult AddPourDetail(PourDetailView model)
        {
            try
            {
                Mark mark = new Mark
                {
                    JobID = model.JobID,
                    MarkNumber = model.MarkNumber,
                    Width = model.Width,
                    Length = model.Length,
                    Thickness = model.Thickness,
                    MarkTypeID = model.MarkTypeID,
                    Weight = model.Weight,
                    SquareFeet = model.SquareFeet
                };
                CarraraSQL.Marks.Add(mark);
                PourDetail pourDetail = new PourDetail
                {
                    Quantity = model.Quantity,
                    MarkRange = model.MarkRange,
                    Camber = model.Camber,
                    MarkID = mark.MarkID,
                    PourID = model.PourID
                };
                CarraraSQL.PourDetails.Add(pourDetail);
                CarraraSQL.SaveChanges();
                Pour pour = CarraraSQL.Pours.Find(model.PourID);
                return PartialView("_PourDetail", new PourDetail
                {
                    PourID = model.PourID,
                    Pour = pour,
                    DefaultJobID = pour.DefaultJobID,
                    DefaultMarkTypeID = pour.DefaultMarkTypeID
                });
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

        public ActionResult Analysis()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteNonConformingReport(int guid)
        {
            try
            {
                NonConformingReport nonConformingReport = CarraraSQL.NonConformingReports.Find(guid);
                CarraraSQL.NonConformingReports.Remove(nonConformingReport);
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
        public ActionResult DeletePourDetail(int guid)
        {
            try
            {
                PourDetail pourDetail = CarraraSQL.PourDetails.Find(guid);
                Mark mark = CarraraSQL.Marks.Find(pourDetail.MarkID);
                CarraraSQL.Marks.Remove(mark);
                CarraraSQL.PourDetails.Remove(pourDetail);
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
        public ActionResult EditNonConformingReport(NonConformingReport model)
        {
            try
            {
                NonConformingReport entity = CarraraSQL.NonConformingReports.Find(model.NonConformingReportID);
                entity.JobID = model.JobID;
                entity.DateChecked = model.DateChecked;
                entity.MarkNumber = model.MarkNumber;
                entity.Finding = model.Finding;
                entity.Action = model.Action;
                entity.DueDate = model.DueDate;
                entity.IsCompleted = model.IsCompleted;
                entity.CompletedByID = model.CompletedByID;
                entity.CompletedOn = model.CompletedOn;
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

        public ActionResult EditPour(int guid)
        {
            Pour model = CarraraSQL.Pours.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Pour with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit Pour ID " + model.PourID;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult EditPour(Pour model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();

            Pour entity = CarraraSQL.Pours.Find(model.PourID);
            if (model.Action == "Delete")
            {
                try
                {
                    CarraraSQL.Pours.Attach(model);
                    CarraraSQL.Pours.Remove(model);
                    CarraraSQL.SaveChanges();
                    TempData["Page.Title"] = "The Pour Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Pour Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                entity.BedID = model.BedID;
                entity.MixID = model.MixID;
                entity.Thickness = model.Thickness;
                entity.ThreeInchStrands = model.ThreeInchStrands;
                entity.PourDate = model.PourDate;
                entity.PourTime = model.PourTime;
                entity.YardsConcrete = model.YardsConcrete;
                entity.Location = model.Location;
                entity.FiveInchStrands = model.FiveInchStrands;
                entity.PourStatusID = model.PourStatusID;
                entity.YardsGrout = model.YardsGrout;
                entity.SlipNumber = model.SlipNumber;
                entity.SixInchStrands = model.SixInchStrands;
                entity.JackNumber = model.JackNumber;
                entity.PourScheduleNotes = model.PourScheduleNotes;
                entity.Notes = model.Notes;

                entity.DefaultJobID = model.DefaultJobID;
                entity.DefaultMarkTypeID = model.DefaultMarkTypeID;

                entity.Air = model.Air;
                entity.Slump = model.Slump;
                entity.VSI = model.VSI;
                entity.UnitWeight = model.UnitWeight;
                entity.Yield = model.Yield;
                entity.AmbientTemperature = model.AmbientTemperature;
                entity.ConcreteTemperature = model.ConcreteTemperature;
                entity.NumberOfCylinders = model.NumberOfCylinders;
                entity.Workability = model.Workability;
                entity.Weather = model.Weather;


                entity.ReleaseTest1 = model.ReleaseTest1;
                entity.ReleaseTest2 = model.ReleaseTest2;
                entity.ReleaseTestAverage = ((model.ReleaseTest1.HasValue ? model.ReleaseTest1.Value : 0) + (model.ReleaseTest2.HasValue ? model.ReleaseTest2.Value : 0)) / 2;
                entity.ReleaseTestDate = model.ReleaseTestDate;

                entity.C28DayTest1 = model.C28DayTest1;
                entity.C28DayTest2 = model.C28DayTest2;
                entity.C28DayTestAverage = ((model.C28DayTest1.HasValue ? model.C28DayTest1.Value : 0) + (model.C28DayTest2.HasValue ? model.C28DayTest2.Value : 0)) / 2;
                entity.C28DayTestDate = model.C28DayTestDate;

                entity.OtherTest1 = model.OtherTest1;
                entity.OtherTest2 = model.OtherTest2;
                entity.OtherTestAverage = ((model.OtherTest1.HasValue ? model.OtherTest1.Value : 0) + (model.OtherTest2.HasValue ? model.OtherTest2.Value : 0)) / 2;
                entity.OtherTestDate = model.OtherTestDate;

                entity.SpecAir = model.SpecAir;
                entity.SpecAirError = model.SpecAirError;
                entity.SpecMaxSlump = model.SpecMaxSlump;
                entity.SpecReleaseStrength = model.SpecReleaseStrength;
                entity.Spec28DayStrength = model.Spec28DayStrength;
                entity.SpecNotes = model.SpecNotes;
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Pour Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Pour Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult EditPourDetail(PourDetailView model)
        {
            try
            {
                Mark mark = CarraraSQL.Marks.Find(model.MarkID);
                mark.JobID = model.JobID;
                mark.MarkNumber = model.MarkNumber;
                mark.Width = model.Width;
                mark.Length = model.Length;
                mark.Thickness = model.Thickness;
                mark.MarkTypeID = model.MarkTypeID;
                mark.Weight = model.Weight;
                mark.SquareFeet = model.SquareFeet;
                PourDetail pourDetail = CarraraSQL.PourDetails.Find(model.PourDetailID);
                pourDetail.Quantity = model.Quantity;
                pourDetail.MarkRange = model.MarkRange;
                pourDetail.Camber = model.Camber;
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

        public ActionResult Index(int? bed, DateTime? begin, DateTime? end, string order, string range, string sort)
        {

            /*TODO
             * Sorting
             */

            Expression<Func<Pour, bool>> predicate = PredicateBuilder.New<Pour>(true);

            DateTime today = DateTime.Today;
            DateTime startDate = today.AddDays(-((int)today.DayOfWeek)).AddDays(-7);
            DateTime endDate = startDate.AddDays(7).AddSeconds(-1);
            int quarterNumber = (today.Month - 1) / 3 + 1;
            DateTime firstDayOfQuarter = new DateTime(today.Year, 3 * quarterNumber - 2, 1);

            if (bed.HasValue)
            {
                predicate = predicate.And(x => x.BedID == bed);
            }


            if (string.IsNullOrEmpty(range))
            {
                range = "week";
            }
            switch (range)
            {
                case "yesterday":
                    startDate = today.Date.AddDays(-1);
                    endDate = today.AddSeconds(-1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;

                case "last-week":
                    startDate = today.AddDays(-((int)today.DayOfWeek)).AddDays(-7);
                    endDate = startDate.AddDays(7).AddSeconds(-1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;

                case "last-month":
                    startDate = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;

                case "last-quarter":
                    startDate = firstDayOfQuarter.AddMonths(-3);
                    endDate = firstDayOfQuarter.AddSeconds(-1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;

                case "last-year":
                    startDate = new DateTime(today.AddYears(-1).Year, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;

                case "today":
                    startDate = today.Date;
                    endDate = today.AddDays(1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;

                case "week":
                    startDate = today.AddDays(-((int)today.DayOfWeek));
                    endDate = startDate.AddDays(7).AddSeconds(-1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;

                case "month":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;

                case "quarter":
                    startDate = firstDayOfQuarter;
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;

                case "year":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;

                case "custom":
                    startDate = begin ?? today;
                    endDate = (end ?? today).AddDays(1).AddSeconds(-1);
                    predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);
                    break;
            }

            // Run the query
            IEnumerable<Pour> model = CarraraSQL.Pours.AsNoTracking().AsExpandable().Where(predicate).ToList();

            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "pour-date" : sort;
            bool asc = order == "asc";


            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Pours";
            ViewBag.Columns = GetColumns<Pour>();
            return View(model);
        }

        public ActionResult NonConformingReports()
        {
            return View();
        }

        public ActionResult StrandUsage()
        {
            return View();
        }

        public ActionResult Pours()
        {
            return RedirectToAction("Index", Request.QueryString.ToRouteValues());
        }

        public ActionResult PourSchedule(int? bed, DateTime? begin, int? job)
        {
            Expression<Func<Pour, bool>> predicate = PredicateBuilder.New<Pour>(true);

            DateTime startDate = begin ?? DateTime.Today.StartOfWeek(DayOfWeek.Monday);
            DateTime endDate = startDate.AddDays(7).AddSeconds(-1);
            predicate = x => (x.PourDate >= startDate && x.PourDate < endDate);

            if (bed.HasValue)
            {
                predicate = predicate.And(x => x.BedID == bed);
            }

            if (job.HasValue)
            {
                predicate = predicate.And(x => x.DefaultJobID.HasValue && x.DefaultJobID.Value == job.Value);
            }

            // Run the query
            IEnumerable<Pour> model = CarraraSQL.Pours.AsNoTracking().AsExpandable().Where(predicate).ToList();

            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Pour Schedule";
            return View(model);
        }
        #endregion
    }
}