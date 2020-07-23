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
    public class VehicleTypesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Vehicle Type";
            return View("Manage", new VehicleType());
        }

        [HttpPost]
        public ActionResult Add(VehicleType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.VehicleTypes.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("VehicleTypesSelectList");
                TempData["Page.Title"] = "The Vehicle Type Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Vehicle Type Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            VehicleType model = CarraraSQL.VehicleTypes.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Vehicle Type with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.VehicleTypeName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(VehicleType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                VehicleType entity = CarraraSQL.VehicleTypes.Find(model.VehicleTypeID);
                if (entity.Vehicles.Any())
                {
                    Page.Title = "The Vehicle Type Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Vehicle Type to Inactive if you do not intend to use this Vehicle Type again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.VehicleTypes.Attach(model);
                    CarraraSQL.VehicleTypes.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("VehicleTypesSelectList");
                    TempData["Page.Title"] = "The Vehicle Type Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Vehicle Type Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("VehicleTypesSelectList");
                TempData["Page.Title"] = "The Vehicle Type Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Vehicle Type Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<VehicleType> model = CarraraSQL.VehicleTypes;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "name":
                    model = asc ? model.OrderBy(x => x.VehicleTypeName) : model.OrderByDescending(x => x.VehicleTypeName);
                    break;
                case "include-in-dispatch":
                    model = asc ? model.OrderBy(x => x.IncludeInDispatch) : model.OrderByDescending(x => x.IncludeInDispatch);
                    break;
                case "meter-type":
                    model = asc ?
                        model.OrderBy(x => !x.MeterTypeID.HasValue).ThenBy(x => x.MeterTypeID.HasValue ? x.MeterType.MeterTypeName : "") :
                        model.OrderByDescending(x => x.MeterTypeID.HasValue && x.MeterType.MeterTypeName != null).ThenByDescending(x => x.MeterTypeID.HasValue ? x.MeterType.MeterTypeName : null);
                    break;
                case "grease-interval":
                    model = asc ? model.OrderBy(x => x.GreaseInterval) : model.OrderByDescending(x => x.GreaseInterval);
                    break;
                case "oil-interval":
                    model = asc ? model.OrderBy(x => x.OilInterval) : model.OrderByDescending(x => x.OilInterval);
                    break;
                case "service-interval--in-days-":
                    model = asc ? model.OrderBy(x => x.ServiceInterval) : model.OrderByDescending(x => x.ServiceInterval);
                    break;
                case "needs-annual-service":
                    model = asc ? model.OrderBy(x => x.NeedsAnnualService) : model.OrderByDescending(x => x.NeedsAnnualService);
                    break;
                case "needs-grease":
                    model = asc ? model.OrderBy(x => x.NeedsGrease) : model.OrderByDescending(x => x.NeedsGrease);
                    break;
                case "needs-oil":
                    model = asc ? model.OrderBy(x => x.NeedsOil) : model.OrderByDescending(x => x.NeedsOil);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.VehicleTypeID) : model.OrderByDescending(x => x.VehicleTypeID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Vehicle Types";
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