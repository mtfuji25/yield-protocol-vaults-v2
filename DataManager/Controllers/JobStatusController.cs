using Analogueweb.Mvc.Utilities;
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
    [Authorize(Roles = "Admin")]
    public class JobStatusController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Job Status";
            return View("Manage", new JobStatu());
        }

        [HttpPost]
        public ActionResult Add(JobStatu model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.JobStatus.Add(model);
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Job Status Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Job Status Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            JobStatu model = CarraraSQL.JobStatus.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Job Status with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.JobStatusName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(JobStatu model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                JobStatu entity = CarraraSQL.JobStatus.Find(model.JobStatusID);
                if (entity.Jobs.Any())
                {
                    Page.Title = "The Job Status Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Job Status to Inactive if you do not intend to use this Job Status again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.JobStatus.Attach(model);
                    CarraraSQL.JobStatus.Remove(model);
                    CarraraSQL.SaveChanges();
                    TempData["Page.Title"] = "The Job Status Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Job Status Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                TempData["Page.Title"] = "The Job Status Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Job Status Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(bool? active, string order, int? page, int? show, string sort)
        {
            IEnumerable<JobStatu> model = CarraraSQL.JobStatus;
            // Filter by active/inactive
            if (active.HasValue)
            {
                model = model.Where(x => x.IsActive == active.Value);
            }
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "active":
                    model = asc ? model.OrderBy(x => x.IsActive) : model.OrderByDescending(x => x.IsActive);
                    break;
                case "name":
                    model = asc ? model.OrderBy(x => x.JobStatusName) : model.OrderByDescending(x => x.JobStatusName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.JobStatusID) : model.OrderByDescending(x => x.JobStatusID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Job Status";
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
        #endregion
    }
}