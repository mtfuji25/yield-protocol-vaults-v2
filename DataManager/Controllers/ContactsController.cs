using Analogueweb.Mvc.Utilities;
using DataManager.Models.CarraraSQL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace DataManager.Controllers
{
    [Authorize(Roles = "Admin,Precast Manager,Precast Production,Project Manager,Ready Mix Manager,Dispatch Manager")]
    public class ContactsController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add a New Contact";
            return View("Manage", new Contact());
        }

        [HttpPost]
        public ActionResult Add(Contact model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                CarraraSQL.Contacts.Add(model);
                CarraraSQL.SaveChanges();
                HttpContext.Cache.Remove("IndependantDriversSelectList");
                TempData["Page.Title"] = "The Contact Was Added Successfully!";
                return RedirectToAction("Index", parameters);
            }
            // Failure is always an option...
            Page.Title = "The Contact Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Edit(int guid)
        {
            Contact model = CarraraSQL.Contacts.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null)
            {
                TempData["Page.Title"] = "Unable to Locate Contact with Id " + guid.ToString() + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.FullName;
            return View("Manage", model);
        }

        [HttpPost]
        public ActionResult Edit(Contact model)
        {
            Contact entity = CarraraSQL.Contacts.Find(model.ContactID);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Delete")
            {
                if (entity.Jobs.Any() || entity.Loads.Any())
                {
                    Page.Title = "The Contact Is In Use And Was NOT Deleted!";
                    Page.Message = "Set the Contact Type to Inactive if you do not intend to use this Contact again.";
                    return View("Manage", model);
                }
                try
                {
                    CarraraSQL.Contacts.Attach(model);
                    CarraraSQL.Contacts.Remove(model);
                    CarraraSQL.SaveChanges();
                    HttpContext.Cache.Remove("IndependantDriversSelectList");
                    TempData["Page.Title"] = "The Contact Was Deleted Successfully";
                    return RedirectToAction("Index", parameters);
                }
                catch (Exception ex)
                {
                    Page.Title = "The Contact Was NOT Deleted! " + ex.Message;
                    return View("Manage", model);
                }
            }

            if (ModelState.IsValid)
            {
                entity.StreetAddress = model.StreetAddress;
                entity.City = model.City;
                entity.Company = model.Company;
                entity.ContactTypeID = model.ContactTypeID;
                entity.DisplayName = model.DisplayName;
                entity.EMail = model.EMail;
                entity.FaxNumber = model.FaxNumber;
                entity.FirstName = model.FirstName;
                entity.HomePhone = model.HomePhone;
                entity.LastName = model.LastName;
                entity.MobilePhone = model.MobilePhone;
                entity.Notes = model.Notes;
                entity.State = model.State;
                entity.WorkPhone = model.WorkPhone;
                entity.Zip = model.Zip;
                CarraraSQL.SaveChanges();
                Models.Search.Index.ContactsAsync();
                HttpContext.Cache.Remove("IndependantDriversSelectList");
                TempData["Page.Title"] = "The Contact Was Updated Successfully";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = "The Contact Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ActionResult Index(int? cid, string order, int? page, string query, int? show, string sort, int? type)
        {
            IEnumerable<Contact> model = CarraraSQL.Contacts;
            if (string.IsNullOrEmpty(query))
            {
                // Filter
                if (type.HasValue)
                {
                    model = model.Where(x => x.ContactTypeID == type.Value);
                }
                else
                {
                    model = model.Where(x => x.ContactTypeID != 14);
                }
                // Sort
                order = string.IsNullOrEmpty(order) ? "asc" : order;
                sort = string.IsNullOrEmpty(sort) ? "display-name" : sort;
                bool asc = order == "asc";
                switch (sort)
                {
                    case "company":
                    case "first-name":
                        model = asc ? model.OrderBy(x => x.FirstName).ThenBy(x => x.LastName) : model.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName);
                        break;
                    case "last-name":
                        model = asc ? model.OrderBy(x => x.LastName).ThenBy(x => x.FirstName) : model.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName);
                        break;
                    case "home-phone":
                        model = asc ? model.OrderBy(x => x.HomePhone) : model.OrderByDescending(x => x.HomePhone);
                        break;
                    case "work-phone":
                        model = asc ? model.OrderBy(x => x.WorkPhone) : model.OrderByDescending(x => x.WorkPhone);
                        break;
                    case "mobile-phone":
                        model = asc ? model.OrderBy(x => x.MobilePhone) : model.OrderByDescending(x => x.MobilePhone);
                        break;
                    case "fax":
                        model = asc ? model.OrderBy(x => x.FaxNumber) : model.OrderByDescending(x => x.FaxNumber);
                        break;
                    case "email":
                        model = asc ? model.OrderBy(x => x.EMail) : model.OrderByDescending(x => x.EMail);
                        break;
                    case "address":
                        model = asc ? model.OrderBy(x => x.Address) : model.OrderByDescending(x => x.Address);
                        break;
                    case "city":
                        model = asc ? model.OrderBy(x => x.City) : model.OrderByDescending(x => x.City);
                        break;
                    case "state":
                        model = asc ? model.OrderBy(x => x.State) : model.OrderByDescending(x => x.State);
                        break;
                    case "zip":
                        model = asc ? model.OrderBy(x => x.Zip) : model.OrderByDescending(x => x.Zip);
                        break;
                    case "contact-type":
                        model = asc ?
                            model.OrderBy(x => !x.ContactTypeID.HasValue).ThenBy(x => x.ContactTypeID.HasValue ? x.ContactType.ContactTypeName : "") :
                            model.OrderByDescending(x => x.ContactTypeID.HasValue && x.ContactType.ContactTypeName != null).ThenByDescending(x => x.ContactTypeID.HasValue ? x.ContactType.ContactTypeName : null);
                        break;
                    default:
                        model = asc ? model.OrderBy(x => x.DisplayName) : model.OrderByDescending(x => x.DisplayName);
                        break;
                }
            }
            else
            {
                if (cid.HasValue)
                {
                    Contact contact = CarraraSQL.Contacts.Find(cid);
                    if (contact != null)
                    {
                        RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
                        return RedirectToAction("Edit", parameters.CopyAndAdd("guid", cid));
                    }
                }
                // Search by query
                Models.Search.Results<Contact> search = Models.Search.Contacts(null, CarraraSQL, query);
                model = search.List;
                Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : string.IsNullOrEmpty(search.DidYouMean) ? "Found " + search.TotalHits + " result" + (search.TotalHits == 1 ? "" : "s") + " for ‘" + query + "’" : "Did You Mean ‘" + search.DidYouMean + "’?";
                ViewBag.Results = search.TotalHits;
            }
            // Return Results
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Contacts";
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

            ViewBag.Columns = GetColumns<Contact>();
        }
        #endregion
    }
}