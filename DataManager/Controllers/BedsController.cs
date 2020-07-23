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
    public class BedsController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Bed";
            return View("Manage", new Bed());
        }

        [HttpPost]
        public ActionResult Add(Bed model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.Beds.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("BedsSelectList");
                TempData["Page.Title"] = "The Bed Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Bed Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            Bed model = CarraraSQL.Beds.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Bed with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.BedName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(Bed model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                Bed entity = CarraraSQL.Beds.Find(model.BedID);
                if (entity.Pours.Any())
                {
                    Page.Title = "The Bed Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Bed to Inactive if you do not intend to use this Bed again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.Beds.Attach(model);
                    CarraraSQL.Beds.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("BedsSelectList");
                    TempData["Page.Title"] = "The Bed Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Bed Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("BedsSelectList");
                TempData["Page.Title"] = "The Bed Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Bed Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(bool? active, int? mix, string order, int? page, int? show, string sort)
        {
            IEnumerable<Bed> model = CarraraSQL.Beds;
            // Filter by active/inactive
            if (mix.HasValue)
            {
                model = model.Where(x => x.DefaultMixID == mix.Value);
            }
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "name" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "width":
                    model = asc ? model.OrderBy(x => x.Width) : model.OrderByDescending(x => x.Width);
                    break;
                case "length":
                    model = asc ? model.OrderBy(x => x.Length) : model.OrderByDescending(x => x.Length);
                    break;
                case "strand-length":
                    model = asc ? model.OrderBy(x => x.StrandLength) : model.OrderByDescending(x => x.StrandLength);
                    break;
                case "max---of-pours-per-day":
                    model = asc ? model.OrderBy(x => x.MaxNumOfPoursPerDay) : model.OrderByDescending(x => x.MaxNumOfPoursPerDay);
                    break;
                case "mix":
                    model = asc ?
                       model.OrderBy(x => !x.DefaultMixID.HasValue).ThenBy(x => x.DefaultMixID.HasValue ? x.Mix.MixName : "") :
                       model.OrderByDescending(x => x.DefaultMixID.HasValue && x.Mix.MixName != null).ThenByDescending(x => x.DefaultMixID.HasValue ? x.Mix.MixName : null);
                    break;
                case "jack--":
                    model = asc ? model.OrderBy(x => x.DefaultJackNumber) : model.OrderByDescending(x => x.DefaultJackNumber);
                    break;
                case "--of-cylinders":
                    model = asc ? model.OrderBy(x => x.DefaultNumOfCylinders) : model.OrderByDescending(x => x.DefaultNumOfCylinders);
                    break;
                case "slump":
                    model = asc ? model.OrderBy(x => x.DefaultSlump) : model.OrderByDescending(x => x.DefaultSlump);
                    break;
                case "release-spec":
                    model = asc ? model.OrderBy(x => x.DefaultReleaseSpec) : model.OrderByDescending(x => x.DefaultReleaseSpec);
                    break;
                case "28-day-spec":
                    model = asc ? model.OrderBy(x => x.Default28DaySpec) : model.OrderByDescending(x => x.Default28DaySpec);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.BedName) : model.OrderByDescending(x => x.BedName);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Beds";
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

            ViewBag.Columns = GetColumns<Bed>();
        }
        #endregion
    }
}