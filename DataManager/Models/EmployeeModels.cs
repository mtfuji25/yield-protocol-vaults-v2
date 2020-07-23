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

    public class UserProfile : IdentityUser
    {
        #region Methods
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserProfile> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string ToCsv()
        {
            List<string> row = new List<string>
            {

            };
            return string.Join(",", row.ToArray());
        }
        #endregion

        #region Properties
        [DisplayName("Active?")]
        public bool Active { get; set; }

        [DataType(DataType.Text), DisplayName("Address"), MaxLength(128), StringLength(128)]
        public string Address1 { get; set; }

        [DataType(DataType.Text), DisplayName("Address Continued"), MaxLength(128), StringLength(128)]
        public string Address2 { get; set; }

        [DataType(DataType.Date), DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Text), MaxLength(128), StringLength(128)]
        public string City { get; set; }

        [DisplayName("Default Assignment Type")]
        public int? DefaultAssignmentTypeId { get; set; }

        [DisplayName("Default Cost Center")]
        public int? DefaultCostCenterId { get; set; }

        [DisplayName("Department")]
        public int? DepartmentId { get; set; }

        public string DriverKey { get; set; }

        [DataType(DataType.Text), DisplayName("Emergency Contact"), MaxLength(256), StringLength(256)]
        public string EmergencyContactName { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Emergency Contact Phone"), MaxLength(32), StringLength(32)]
        public string EmergencyContactPhone { get; set; }

        [DisplayName("Employee Number")]
        public int? EmployeeNumber { get; set; }

        [DisplayName("Employee Status")]
        public int? EmployeeStatusId { get; set; }

        [DataType(DataType.Date), DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Text), DisplayName("Name"), MaxLength(128), Required, StringLength(128)]
        public string FirstName { get; set; }

        [DisplayName("Administrator")]
        public bool IsAdministrator { get; set; }

        [DisplayName("Driver")]
        public bool IsDriver { get; set; }

        [DisplayName("Hourly")]
        public bool IsHourly { get; set; }

        [DisplayName("Project Manager")]
        public bool IsProjectManager { get; set; }

        [DisplayName("Redi Mix Visible")]
        public bool IsRediMixVisible { get; set; }

        [DataType(DataType.Text), DisplayName("Last Name"), MaxLength(128), Required, StringLength(128)]
        public string LastName { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public DateTime LastUpdated { get; set; }

        [DisplayName("Location")]
        public int? LocationId { get; set; }

        public int LoginCount { get; set; }

        [DataType(DataType.PhoneNumber), MaxLength(32), StringLength(32)]
        public string Mobile { get; set; }

        public int? EmployeeId { get; set; }

        [DataType(DataType.PhoneNumber), MaxLength(32), StringLength(32)]
        public string Phone { get; set; }

        public decimal? Rate { get; set; }

        [DataType(DataType.DateTime), DisplayName("Shift End Time")]
        public DateTime? ShiftEndTime { get; set; }

        [DataType(DataType.DateTime), DisplayName("Shift Start Time")]
        public DateTime? ShiftStartTime { get; set; }

        [DataType(DataType.Date), DisplayName("End Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Text), MaxLength(16), StringLength(16)]
        public string SSN { get; set; }

        [DataType(DataType.Text), MaxLength(2), StringLength(2)]
        public string State { get; set; }

        [DisplayName("Vacation Hours")]
        public decimal? VacationHours { get; set; }

        [DisplayName("Vehicle")]
        public int? VehicleId { get; set; }

        [DisplayName("Years of Service")]
        public decimal? YearsOfService { get; set; }

        [DataType(DataType.Text), DisplayName("Zip Code"), MaxLength(16), StringLength(16)]
        public string ZipCode { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }

        [NotMapped]
        public string Address
        {
            get
            {
                return string.Concat(FirstName, " ", LastName, Environment.NewLine, Address1, string.IsNullOrEmpty(Address2) ? "" : Environment.NewLine + Address2, Environment.NewLine, City, ", ", State, " ", ZipCode);
            }
        }

        [DisplayName("Restore Point"), NotMapped]
        public DateTime? ChangeDate { get; set; }

        [NotMapped]
        public List<SelectListItem> ChangeDates { get; set; }

        [DataType(DataType.Password), DisplayName("Confirm Password"), NotMapped]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress), DisplayName("Email"), NotMapped, StringLength(128)]
        public string EmailAddress { get; set; }

        [NotMapped]
        public string FullAddress
        {
            get
            {
                return string.Concat(FirstName, " ", LastName, Environment.NewLine, Address1, string.IsNullOrEmpty(Address2) ? "" : Environment.NewLine + Address2, Environment.NewLine, City, ", ", State, " ", ZipCode);
            }
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Concat(FirstName, " ", LastName);
            }
        }

        [DataType(DataType.Password), DisplayName("Password"), NotMapped]
        public string NewPassword { get; set; }

        [NotMapped]
        public bool NewUser { get; set; }

        [NotMapped]
        public string Role { get; set; }

        [NotMapped]
        public string Street
        {
            get
            {
                return Address1;
            }
        }

        [NotMapped]
        public string Zip
        {
            get
            {
                return ZipCode;
            }
        }
        #endregion

        #region One-to-Many Relationships

        #endregion

        #region One-to-One Relationships
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        [ForeignKey("EmployeeStatusId")]
        public EmployeeStatus EmployeeStatus { get; set; }

        [ForeignKey("LocationId")]
        public Location Location { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }
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
}