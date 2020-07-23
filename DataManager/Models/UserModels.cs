using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataManager.Models
{

    public class ForgotPassword
    {
        #region Properties
        [DataType(DataType.EmailAddress), Required, StringLength(256)]
        public string Email { get; set; }
        #endregion
    }

    public class Login
    {
        #region Properties
        [DataType(DataType.EmailAddress), Required, StringLength(256)]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, StringLength(128)]
        public string Password { get; set; }

        [DisplayName("Keep me logged in")]
        public bool RememberMe { get; set; }
        #endregion
    }

    public class AspNetUser : IdentityUser
    {
        #region Methods
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AspNetUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        #endregion

        #region Properties
        [DisplayName("Active")]
        public bool Active { get; set; }

        [DisplayName("Employee")]
        public int? EmployeeID { get; set; }

        [DisplayName("Name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public int LoginCount { get; set; }

        public DateTime LastUpdated { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }

        [DataType(DataType.Password), DisplayName("Confirm Password"), NotMapped]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress), DisplayName("Email Address"), NotMapped, StringLength(256)]
        public string EmailAddress { get; set; }

        public string FullName
        {
            get
            {
                return string.Concat(FirstName, " ", LastName);
            }
        }

        [DataType(DataType.Password), DisplayName("New Password"), NotMapped]
        public string NewPassword { get; set; }

        [NotMapped]
        public bool NewUser { get; set; }

        [NotMapped]
        public string Role { get; set; }
        #endregion

        #region Collections 
        public virtual ICollection<ViewSetting> ViewSettings { get; set; }
        #endregion
    }

    public class ResetPassword
    {
        #region Properties
        [DataType(DataType.Password), Required, StringLength(128, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match."), DataType(DataType.Password), DisplayName("Confirm password"), Required]
        public string ConfirmPassword { get; set; }

        [HiddenInput]
        public string Code { get; set; }

        [HiddenInput]
        public string Id { get; set; }
        #endregion
    }

    [Table("ViewSettings")]
    public class ViewSetting
    {
        public string AspNetUserId { get; set; }

        public string Columns { get; set; }

        public string Path { get; set; }

        public string PathAndQuery { get; set; }

        public int ViewSettingId { get; set; }
    }
}