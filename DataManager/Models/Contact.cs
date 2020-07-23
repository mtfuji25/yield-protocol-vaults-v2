using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("Contacts")]
    public class Contact
    {
        #region Methods
        public Contact()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int ContactID { get; set; }

        [DisplayName("Display Name"), StringLength(100)]
        public string DisplayName { get; set; }

        [DisplayName("Company"), StringLength(100)]
        public string Company { get; set; }

        [DisplayName("First Name"), StringLength(50)]
        public string FirstName { get; set; }

        [DisplayName("Last Name"), StringLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Home Phone"), StringLength(50)]
        public string HomePhone { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Work Phone"), StringLength(50)]
        public string WorkPhone { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Mobile Phone"), StringLength(50)]
        public string MobilePhone { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Fax"), StringLength(50)]
        public string FaxNumber { get; set; }

        [DataType(DataType.EmailAddress), DisplayName("Email"), StringLength(100)]
        public string EMail { get; set; }

        [DisplayName("Address"), StringLength(100)]
        public string StreetAddress { get; set; }

        [DisplayName("City"), StringLength(50)]
        public string City { get; set; }

        [DisplayName("State"), StringLength(50)]
        public string State { get; set; }

        [DisplayName("Zip"), StringLength(50)]
        public string Zip { get; set; }

        [DisplayName("Contact Type")]
        public int? ContactTypeID { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }

        [NotMapped]
        public string Address
        {
            get
            {
                return string.Concat(StreetAddress, Environment.NewLine, City, ", ", State, " ", Zip);
            }
        }

        [NotMapped]
        public DateTime? ChangeDate { get; set; }

        [NotMapped]
        public List<SelectListItem> ChangeDates { get; set; }

        [NotMapped]
        public string FullAddress
        {
            get
            {
                return string.Concat(FirstName, " ", LastName, Environment.NewLine, StreetAddress, Environment.NewLine, City, ", ", State, " ", Zip);
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

        [NotMapped]
        public string LastFirst
        {
            get
            {
                return string.IsNullOrEmpty(LastName) ? FirstName : string.IsNullOrEmpty(FirstName) ? LastName : string.Concat(LastName, ", ", FirstName);
            }
        }
        #endregion

        #region Foreign Keys
        [ForeignKey("ContactTypeID")]
        public virtual ContactType ContactType { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ICollection<Load> Loads { get; set; }
        #endregion
    }
}
