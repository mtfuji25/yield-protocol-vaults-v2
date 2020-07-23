using Analogueweb.Mvc.Utilities;
using DataManager.Models;
using DataManager.Models.CarraraSQL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataManager.Controllers
{
    [Authorize(Roles = "Admin,Precast Manager,Precast Production,Project Manager,Ready Mix Manager,Dispatch Manager")]
    public class JobsController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Job";
            return View("Manage", new Job());
        }

        [HttpPost]
        public ActionResult Add(Job model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.Jobs.Add(model);
                CarraraSQL.SaveChanges();
                Search.Index.JobsAsync();
                HttpContext.Cache.Remove("JobsSelectList");
                TempData["Page.Title"] = "The Job Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Job Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            Job model = CarraraSQL.Jobs.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Job with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.JobName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(Job model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                Job entity = CarraraSQL.Jobs.Find(model.JobID);
                if (entity.HoursAssignments.Any() ||
                    entity.JobSiteLocations.Any() ||
                    entity.Loads.Any() ||
                    entity.Marks.Any() ||
                    entity.NonConformingReports.Any() ||
                    entity.Pours.Any())
                {
                    Page.Title = "The Job Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Job Type to Completed, On Hold or Cancelled if you do not intend to use this Job again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.Jobs.Attach(model);
                    CarraraSQL.Jobs.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("JobsSelectList");
                    TempData["Page.Title"] = "The Job Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Job Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                Search.Index.JobsAsync();
                HttpContext.Cache.Remove("JobsSelectList");
                TempData["Page.Title"] = "The Job Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Job Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(int? jid, string order, int? page, string query, int? show, int? status, string sort)
        {
            IEnumerable<Job> model = CarraraSQL.Jobs;
            if (string.IsNullOrEmpty(query))
            {
                // Filter
                if (status.HasValue)
                {
                    if (status.Value != 0)
                    {
                        model = model.Where(x => x.JobStatusID == status.Value);
                    }
                }
                else
                {
                    model = model.Where(x => x.JobStatusID == 1);
                }
                // Sort
                order = string.IsNullOrEmpty(order) ? "asc" : order;
                sort = string.IsNullOrEmpty(sort) ? "job-id" : sort;
                bool asc = order == "asc";
                switch (sort)
                {
                    case "job-number":
                        model = asc ? model.OrderBy(x => x.JobNumber) : model.OrderByDescending(x => x.JobNumber);
                        break;
                    case "job-name":
                        model = asc ? model.OrderBy(x => x.JobName) : model.OrderByDescending(x => x.JobName);
                        break;
                    case "erector":
                        model = asc ?
                           model.OrderBy(x => !x.ErectorID.HasValue).ThenBy(x => x.ErectorID.HasValue ? x.Contact.DisplayName : "") :
                           model.OrderByDescending(x => x.ErectorID.HasValue && x.Contact.DisplayName != null).ThenByDescending(x => x.ErectorID.HasValue ? x.Contact.DisplayName : null);
                        break;
                    case "project-manager":
                        model = asc ?
                            model.OrderBy(x => !x.ProjectManagerID.HasValue).ThenBy(x => x.ProjectManagerID.HasValue ? x.Employee.LastName : "").ThenBy(x => x.ProjectManagerID.HasValue ? x.Employee.FirstName : "") :
                            model.OrderByDescending(x => x.ProjectManagerID.HasValue && x.Employee.LastName != null).ThenByDescending(x => x.ProjectManagerID.HasValue ? x.Employee.LastName : null).ThenByDescending(x => x.ProjectManagerID.HasValue ? x.Employee.FirstName : null);
                        break;
                    case "street":
                        model = asc ? model.OrderBy(x => x.Street) : model.OrderByDescending(x => x.Street);
                        break;
                    case "city":
                        model = asc ? model.OrderBy(x => x.City) : model.OrderByDescending(x => x.City);
                        break;
                    case "state":
                        model = asc ? model.OrderBy(x => x.State) : model.OrderByDescending(x => x.State);
                        break;
                    case "zip-code":
                        model = asc ? model.OrderBy(x => x.ZIP) : model.OrderByDescending(x => x.ZIP);
                        break;
                    case "general-contractor":
                        model = asc ? model.OrderBy(x => x.GeneralContractor) : model.OrderByDescending(x => x.GeneralContractor);
                        break;
                    case "status":
                        model = asc ?
                           model.OrderBy(x => !x.JobStatusID.HasValue).ThenBy(x => x.JobStatusID.HasValue ? x.JobStatu.JobStatusName : "") :
                           model.OrderByDescending(x => x.JobStatusID.HasValue && x.JobStatu.JobStatusName != null).ThenByDescending(x => x.JobStatusID.HasValue ? x.JobStatu.JobStatusName : null);
                        break;
                    case "travel-time":
                        model = asc ? model.OrderBy(x => x.TravelTime) : model.OrderByDescending(x => x.TravelTime);
                        break;
                    case "route":
                        model = asc ? model.OrderBy(x => x.State) : model.OrderByDescending(x => x.State);
                        break;
                    default:
                        model = asc ? model.OrderBy(x => x.JobID) : model.OrderByDescending(x => x.JobID);
                        break;
                }
            }
            else
            {
                if (jid.HasValue)
                {
                    Job job = CarraraSQL.Jobs.Find(jid);
                    if (job != null)
                    {
                        RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
                        return RedirectToAction("Edit", parameters.CopyAndAdd("guid", jid));
                    }
                }
                // Search by query
                Search.Results<Job> search = Search.Jobs(null, CarraraSQL, query);
                model = search.List;
                Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : string.IsNullOrEmpty(search.DidYouMean) ? "Found " + search.TotalHits + " result" + (search.TotalHits == 1 ? "" : "s") + " for ‘" + query + "’" : "Did You Mean ‘" + search.DidYouMean + "’?";
                ViewBag.Results = search.TotalHits;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Jobs";
            page = page ?? 1;
            show = show ?? 40;
            int count = model.Count() == 0 ? 1 : model.Count();
            if (show.Value > count || show == 0)
            {
                show = count;
                page = 1;
            }
            int? max = (count / show) + 1;
            if (page > max)
            {
                page = max;
            }
            return View(model.ToPagedList(page.Value, show.Value));
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewBag.Columns = GetColumns<Job>();
        }
        #endregion
    }
}