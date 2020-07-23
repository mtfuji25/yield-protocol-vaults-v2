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
    public class MarkTypesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Mark Type";
            return View("Manage", new MarkType());
        }

        [HttpPost]
        public ActionResult Add(MarkType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.MarkTypes.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("MarkTypesSelectList");
                TempData["Page.Title"] = "The Mark Type Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Mark Type Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            MarkType model = CarraraSQL.MarkTypes.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Mark Type with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.MarkTypeName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(MarkType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                MarkType entity = CarraraSQL.MarkTypes.Find(model.MarkTypeID);
                if (entity.Marks.Any() ||
                    entity.Pours.Any())
                {
                    Page.Title = "The Mark Type Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Mark Type to Inactive if you do not intend to use this Mark Type again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.MarkTypes.Attach(model);
                    CarraraSQL.MarkTypes.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("MarkTypesSelectList");
                    TempData["Page.Title"] = "The Mark Type Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Mark Type Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("MarkTypesSelectList");
                TempData["Page.Title"] = "The Mark Type Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Mark Type Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<MarkType> model = CarraraSQL.MarkTypes;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "id":
                    model = asc ? model.OrderBy(x => x.MarkTypeName) : model.OrderByDescending(x => x.MarkTypeName);
                    break;
                case "weight-formula-type":
                    model = asc ?
                        model.OrderBy(x => !x.WeightFormulaTypeID.HasValue).ThenBy(x => x.WeightFormulaTypeID.HasValue ? x.WeightFormulaType.WeightFormulaTypeName : "") :
                        model.OrderByDescending(x => x.WeightFormulaTypeID.HasValue && x.WeightFormulaType.WeightFormulaTypeName != null).ThenByDescending(x => x.WeightFormulaTypeID.HasValue ? x.WeightFormulaType.WeightFormulaTypeName : null);
                    break;
                case "mark-size-type":
                    model = asc ?
                        model.OrderBy(x => !x.MarkSizeTypeID.HasValue).ThenBy(x => x.MarkSizeTypeID.HasValue ? x.MarkSizeType.MarkSizeTypeName : "") :
                        model.OrderByDescending(x => x.MarkSizeTypeID.HasValue && x.MarkSizeType.MarkSizeTypeName != null).ThenByDescending(x => x.MarkSizeTypeID.HasValue ? x.MarkSizeType.MarkSizeTypeName : null);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.MarkTypeID) : model.OrderByDescending(x => x.MarkTypeID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Mark Types";
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