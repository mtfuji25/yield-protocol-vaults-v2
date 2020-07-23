using Analogueweb.Mvc.Utilities;
using DataManager.Models;
using DataManager.Models.CarraraSQL;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace DataManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : DataManagerController
    {
        #region Methods
        public ActionResult Add()
        {
            Page.Title = "Add A New User";
            return View("Manage", new AspNetUser
            {
                Active = true,
                NewUser = true
            });
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Add(AspNetUser model)
        {
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (ModelState.IsValid)
            {
                Employee employee = CarraraSQL.Employees.FirstOrDefault(x => x.FirstName == model.FirstName && x.LastName == model.LastName && x.EmployeeStatusID == 1);
                if (employee != null)
                {
                    model.EmployeeID = employee.EmployeeID;
                }
                model.Active = true;
                model.Email = model.EmailAddress;
                model.EmailConfirmed = true;
                model.LastUpdated = DateTime.Now;
                model.UserName = model.EmailAddress;
                IdentityResult result = await UserManager.CreateAsync(model, Membership.GeneratePassword(128, 18));
                if (result.Succeeded)
                {
                    UserManager.AddToRole(model.Id, model.Role);
                    //string token = await UserManager.GeneratePasswordResetTokenAsync(model.Id);
                    //string url = Url.Action("Reset", "Account", new { code = token, id = model.Id }, protocol: Request.Url.Scheme);
                    //StringBuilder builder = new StringBuilder();
                    //builder.AppendLine("<p>" + model.FullName + "</p>");
                    //builder.AppendLine("<p>A new account has been created for you on JP Carrara Data Manager. Please click the link below to confirm your account and set your password:</p>");
                    //builder.AppendLine("<p><a href=\"" + url + "\">Set Your Password</a>");
                    //await SendMail(builder.ToString(), "Confirm Your JP Carrara Data Manager Account", model.Email, string.Empty);
                    TempData["Page.Title"] = "The User Was Added Successfully!";
                    TempData["Page.Message"] = string.Concat("We have just sent an e-mail to <strong>", model.Email, "</strong> with instructions on how to confirm their JP Carrara Data Manager account.");
                    return RedirectToAction("Edit", parameters.CopyAndAdd("guid", model.Id));
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            Page.Title = "The User Was NOT Added!";
            Page.Message = "Please correct the following errors:";
            model.NewUser = true;
            return View("Manage", model);
        }

        public ActionResult Edit(string guid)
        {
            AspNetUser model = DataContext.Users.Find(guid);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model == null /*|| model.EmployeeID == -1*/)
            {
                TempData["Page.Title"] = "Unable to Locate User with Id " + guid + "!";
                return RedirectToAction("Index", parameters);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Edit " + model.FullName;
            return View("Manage", model);
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Edit(AspNetUser model)
        {
            AspNetUser entity = DataContext.Users.Find(model.Id);
            RouteValueDictionary parameters = Request.QueryString.ToRouteValues();
            if (model.Action == "Masquerade" && User.IsInRole("Admin"))
            {
                string adminId = User.Identity.GetUserId();
                string returnUrl = Request.UrlReferrer.ToString();
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                ClaimsIdentity identity = await UserManager.CreateIdentityAsync(entity, DefaultAuthenticationTypes.ApplicationCookie);
                identity.AddClaim(new Claim("AdminId", adminId));
                identity.AddClaim(new Claim("IsMasquerading", "true"));
                identity.AddClaim(new Claim("ReturnUrl", returnUrl));
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                return RedirectToAction("Index", "Account");
            }
            // Duplicate check
            AspNetUser duplicate = DataContext.Users.FirstOrDefault(x => x.Email == model.EmailAddress && x.Id != model.Id);
            if (duplicate != null)
            {
                ModelState.AddModelError("", "The email you entered already exists within our system, please enter a different email!");
            }
            // Password check
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword == model.ConfirmPassword)
                {
                    string token = UserManager.GeneratePasswordResetToken(entity.Id);
                    IdentityResult result = UserManager.ResetPassword(entity.Id, token, model.NewPassword);
                    if (result != IdentityResult.Success)
                    {
                        AddErrors(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The password and confirmation password do not match.");
                }
            }
            // Revalidate model
            model.Email = model.EmailAddress;
            if (TryValidateModel(model))
            {
                if (!UserManager.IsInRole(entity.Id, model.Role))
                {
                    IList<string> roles = UserManager.GetRoles(entity.Id);
                    if (roles.Any())
                    {
                        UserManager.RemoveFromRoles(entity.Id, UserManager.GetRoles(entity.Id).ToArray());
                    }
                    UserManager.AddToRole(entity.Id, model.Role);
                }
                entity.Active = model.Active;
                entity.FirstName = model.FirstName;
                entity.EmployeeID = model.EmployeeID;
                entity.LastName = model.LastName;
                entity.LastUpdated = DateTime.Now;
                entity.UserName = model.Email;
                DataContext.SaveChanges();
                TempData["Page.Title"] = "The User Was Updated Successfully!";
                return RedirectToAction("Edit", parameters.CopyAndAdd("guid", model.Id));
            }
            Page.Title = "The User Was NOT Updated!";
            Page.Message = "Please correct the following errors:";
            return View("Manage", model);
        }

        public ContentResult GetPassword()
        {
            return Content(Membership.GeneratePassword(12, 1), "text/plain");
        }

        public ActionResult Index(bool? active, string order, int? page, string role, int? show, string sort, string uid)
        {
            IEnumerable<AspNetUser> model = DataContext.Users.Where(x => x.EmployeeID != -1);

            // Filter
            if (active.HasValue)
            {
                model = model.Where(x => x.Active == active.Value);
            }
            if (!string.IsNullOrEmpty(role))
            {
                string roleId = DataContext.Roles.FirstOrDefault(x => x.Name == role).Id;
                model = model.Where(x => x.Roles.Any(r => r.RoleId.Contains(roleId)));
            }
            // Sort
            order = string.IsNullOrEmpty(order) ? "asc" : order;
            sort = string.IsNullOrEmpty(sort) ? "last" : sort;
            bool asc = order == "asc";
            switch (sort)
            {
                case "last":
                    model = asc ? model.OrderBy(x => x.LastName).ThenBy(x => x.FirstName) : model.OrderByDescending(x => x.LastName).ThenBy(x => x.FirstName);
                    break;
                case "first":
                    model = asc ? model.OrderBy(x => x.FirstName).ThenBy(x => x.LastName) : model.OrderByDescending(x => x.FirstName).ThenBy(x => x.LastName);
                    break;
                case "email":
                    model = asc ? model.OrderBy(x => x.Email) : model.OrderByDescending(x => x.Email);
                    break;
                case "logins":
                    model = asc ? model.OrderBy(x => x.LoginCount) : model.OrderByDescending(x => x.LoginCount);
                    break;
                case "last-login":
                    model = asc ? model.OrderBy(x => x.LastLoginDate) : model.OrderByDescending(x => x.LastLoginDate);
                    break;
                case "active":
                    model = asc ? model.OrderBy(x => x.Active) : model.OrderByDescending(x => x.Active);
                    break;
                default:
                    model = asc ? model.OrderBy(x => x.LastName).ThenBy(x => x.FirstName) : model.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName);
                    break;
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Users";
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