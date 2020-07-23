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
    public class WeightFormulaTypesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Weight Formula Type";
            return PartialView("Manage", new WeightFormulaType());
        }

        [HttpPost]
        public ActionResult Add(WeightFormulaType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.WeightFormulaTypes.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("WeightFormulaTypesSelectList");
                return PartialView("Row", model);
            }
            return ReturnError();
        }

        public ActionResult Edit(int guid)
        {
            WeightFormulaType model = CarraraSQL.WeightFormulaTypes.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                return Content(string.Concat("Whoops…Unable to Locate Weight Formula Type with Id ", guid.ToString(), "!"), "text/plain");
            }
            return PartialView("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(WeightFormulaType model)
        {
            if (model.Action == "Delete")
            {
                WeightFormulaType entity = CarraraSQL.WeightFormulaTypes.Find(model.WeightFormulaTypeID);
                if (entity.MarkTypes.Any())
                {
                    return Content("Whoops…The Weight Formula Type Is In Use And Was NOT Deleted!", "text/plain");
                }
                try
                {
                    CarraraSQL.WeightFormulaTypes.Remove(entity);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("WeightFormulaTypesSelectList");
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
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("WeightFormulaTypesSelectList");
                return PartialView("Row", model);
            }
            return ReturnError();
        }

        public ActionResult Index(bool? active, string order, int? page, int? show, string sort)
        {
            IEnumerable<WeightFormulaType> model = CarraraSQL.WeightFormulaTypes;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "name":
                    model = asc ? model.OrderBy(x => x.WeightFormulaTypeName) : model.OrderByDescending(x => x.WeightFormulaTypeName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.WeightFormulaTypeID) : model.OrderByDescending(x => x.WeightFormulaTypeID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Weight Formula Types";
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