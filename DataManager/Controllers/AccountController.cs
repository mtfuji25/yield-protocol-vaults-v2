using DataManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataManager.Controllers
{
    [Authorize]
    public class AccountController : DataManagerController
    {
        #region Methods
        public ActionResult Clear()
        {
            IEnumerable<ViewSetting> settings = DataContext.ViewSettings.Where(x => x.AspNetUserId == AspNetUser.Id).ToList();
            foreach (ViewSetting setting in settings)
            {
                DataContext.ViewSettings.Remove(setting);
            }
            DataContext.SaveChanges();
            return RedirectToAction("Index", "Account");
        }

        [AllowAnonymous]
        public ActionResult Forgot()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Forgot Your Password?";
            Page.Message = TempData["Page.Message"] != null ? TempData["Page.Message"].ToString() : "Please enter your email address, and we will send you a link to change your password.";
            return View();
        }

        [AllowAnonymous, HttpPost]
        public async Task<ActionResult> Forgot(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                TempData["Page.Title"] = "Check Your Email";
                AspNetUser user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("Forgot");
                }
                // Send an email with this link
                string token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                string url = Url.Action("Reset", "Account", new { code = token, id = user.Id }, protocol: Request.Url.Scheme);
                ViewBag.Url = url;
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("We've received a request that you'd like to reset your password. If you made this request, please follow the instructions below." + Environment.NewLine);
                builder.AppendLine("Click the following link to reset your password: " + url + Environment.NewLine);
                builder.AppendLine("Please note: For security purposes, this link will expire 24 hours from the time it was sent. If clicking the link does not work, you can copy and paste the link into your browser's address window, or retype it there.");
                builder.AppendLine("If you did not request to have your password reset, you do not need to take any action.");
                //await SendMail(builder.ToString(), "Reset Your Data Manager Login", user.Email, string.Empty);
                TempData["Page.Message"] = string.Concat( "We've sent you an email with instructions on how to reset your password.", HttpContext.IsDebuggingEnabled ? url : string.Empty);
                return RedirectToAction("Login");
            }
            // If we got this far, something failed, redisplay form
            Page.Title = "We're Sorry";
            Page.Message = "We were not able to send you a link to change your password, please try again.";
            return View(model);
        }

        public ActionResult Index()
        {
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Welcome to Data Manager";
            Page.Message = TempData["Page.Message"] != null ? TempData["Page.Message"].ToString() : "What would you like to do?";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Welcome to Data Mangager";
            Page.Message = TempData["Page.Message"] != null ? TempData["Page.Message"].ToString() : "Please login to continue.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous, HttpPost]
        public async Task<ActionResult> Login(Login model, string returnUrl)
        {
            Page.Title = "Welcome to Data Mangager";
            Page.Message = "We're sorry, some of your login information isn't correct. Please try again.";
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            AspNetUser user = DataContext.Users.FirstOrDefault(x => x.Email == model.Email);
            if (user != null && !user.Active)
            {
                Page.Message = "The account you are trying to access is no longer active.";
                return View(model);
            }
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            SignInStatus result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    DateTime now = DateTime.Now;
                    user.LoginCount += 1;
                    user.LastLoginDate = now;
                    DataContext.SaveChanges();
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index");
                case SignInStatus.LockedOut:
                    Page.Message = "For security reasons your account is temporarily locked, please try to login again later.";
                    return View(model);
                case SignInStatus.Failure:
                default:
                    return View(model);
            }
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Manage()
        {
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Manage Your Account";
            Page.Message = TempData["Page.Message"] != null ? TempData["Page.Message"].ToString() : "Update your account information and login credentials.";
            AspNetUser model = UserManager.FindById(User.Identity.GetUserId());
            return View(model);
        }

        [HttpPost]
        public ActionResult Manage(AspNetUser model)
        {
            AspNetUser entity = DataContext.Users.Find(model.Id);
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
                if (!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.ConfirmPassword)
                {
                    // A new password has been set...
                    string token = UserManager.GeneratePasswordResetToken(entity.Id);
                    UserManager.ResetPassword(entity.Id, token, model.NewPassword);
                }
                entity.Email = model.EmailAddress;
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.LastUpdated = DateTime.Now;
                entity.UserName = model.Email;
                DataContext.SaveChanges();
                TempData["Page.Title"] = "Your Account Information Was Updated Successfully!";
                return RedirectToAction("Manage");
            }
            Page.Title = "We Were Not Able to Update Your Account Information!";
            Page.Message = "Please correct the following errors:";
            return View(model);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (Request.IsAuthenticated)
            {
                Page.Id = "account";
            }
        }

        [AllowAnonymous]
        public ActionResult Reset(string code, string id)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("Login");
            }
            if (UserManager.VerifyUserToken(id, "ResetPassword", code))
            {
                Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Reset Your Password";
                Page.Message = TempData["Page.Message"] != null ? TempData["Page.Message"].ToString() : "Use the form below to change your password:";
                return View(new ResetPassword
                {
                    Code = code,
                    Id = id
                });
            }
            AspNetUser user = DataContext.Users.Find(id);
            if (user != null)
            {
                return View("Expired", user);
            }
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "Your Reset Link Has Expired!";
            Page.Message = TempData["Page.Message"] != null ? TempData["Page.Message"].ToString() : "Please enter your email address, and we will send you a new link to change your password.";
            return View("Forgot");
        }

        [AllowAnonymous, HttpPost]
        public async Task<ActionResult> Reset(ResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                Page.Title = "We're Sorry";
                Page.Message = "The password you entered was either too weak or didn't match the confirmation password, please try again.";
                return View(model);
            }
            try
            {
                IdentityResult result = await UserManager.ResetPasswordAsync(model.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    TempData["Page.Title"] = "Your Password Was Changed Successfully";
                    return RedirectToAction("Login");
                }
                AddErrors(result);
            }
            catch { }
            Page.Title = "We're Sorry";
            Page.Message = "We were not able to update your password, please try again.";
            return View();
        }

        [HttpPost]
        public ActionResult SaveViewSetting(ViewSetting model)
        {
            ViewSetting entity = AspNetUser.ViewSettings.FirstOrDefault(x => x.Path == model.Path);
            string columns = Request.Form["Columns"].ToString();
            if (entity != null)
            {
                entity.Columns = columns;
                DataContext.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                DataContext.ViewSettings.Add(new ViewSetting
                {
                    AspNetUserId = AspNetUser.Id,
                    Columns = columns,
                    Path = model.Path
                });
            }
            DataContext.SaveChanges();
            return Content("OK");
        }

        public async Task<ActionResult> StopMasquerade()
        {
            string adminId = User.GetMasqueradeData("AdminId");
            string returnUrl = User.GetMasqueradeData("ReturnUrl");
            AspNetUser originalUser = await UserManager.FindByIdAsync(adminId);
            ClaimsIdentity identity = await UserManager.CreateIdentityAsync(originalUser, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
            return Redirect(returnUrl);
        }

        public ActionResult TimeClock()
        {
            return View();
        }
        #endregion
    }
}