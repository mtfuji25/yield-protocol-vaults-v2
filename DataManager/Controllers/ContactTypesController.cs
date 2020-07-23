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
    public class ContactTypesController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Contact Type";
            return View("Manage", new ContactType());
        }

        [HttpPost]
        public ActionResult Add(ContactType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.ContactTypes.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("ContactTypesSelectList");
                TempData["Page.Title"] = "The Contact Type Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Contact Type Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            ContactType model = CarraraSQL.ContactTypes.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Contact Type with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.ContactTypeName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(ContactType model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                ContactType entity = CarraraSQL.ContactTypes.Find(model.ContactTypeID);
                if (entity.Contacts.Any())
                {
                    Page.Title = "The Contact Type Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Contact Type to Inactive if you do not intend to use this Contact Type again.";
                    return View("Manage", model);
                }

                try
                {
                    CarraraSQL.ContactTypes.Attach(model);
                    CarraraSQL.ContactTypes.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("ContactTypesSelectList");
                    TempData["Page.Title"] = "The Contact Type Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Contact Type Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }
            if (ModelState.IsValid)
            {
                CarraraSQL.Entry(model).State = EntityState.Modified;
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("ContactTypesSelectList");
                TempData["Page.Title"] = "The Contact Type Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Contact Type Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(string order, int? page, int? show, string sort)
        {
            IEnumerable<ContactType> model = CarraraSQL.ContactTypes;
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "id" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "contact-type":
                    model = asc ? model.OrderBy(x => x.ContactTypeName) : model.OrderByDescending(x => x.ContactTypeName);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.ContactTypeID) : model.OrderByDescending(x => x.ContactTypeID);
                    break;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Contact Types";
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