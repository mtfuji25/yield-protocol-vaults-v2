using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("tblContacts")]
    public class Contact
    {
        #region Properties
        [DisplayName("Active?")]
        public bool Active { get; set; }

        [DataType(DataType.Text), DisplayName("Address"), MaxLength(128), StringLength(128)]
        public string Address1 { get; set; }

        [DataType(DataType.Text), DisplayName("Address Continued"), MaxLength(128), StringLength(128)]
        public string Address2 { get; set; }

        [DataType(DataType.Text), MaxLength(128), StringLength(128)]
        public string City { get; set; }

        [DataType(DataType.Text), MaxLength(128), StringLength(128)]
        public string Company { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int ContactId { get; set; }

        [DisplayName("Contact Type")]
        public int? ContactTypeId { get; set; }

        [DataType(DataType.Text), DisplayName("Display Name"), MaxLength(128), StringLength(128)]
        public string DisplayName { get; set; }

        [DataType(DataType.EmailAddress), MaxLength(128), StringLength(128)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Fax"), MaxLength(32), StringLength(32)]
        public string FaxNumber { get; set; }

        [DataType(DataType.Text), DisplayName("Name"), MaxLength(64), StringLength(64)]
        public string FirstName { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Home Phone"), MaxLength(32), StringLength(32)]
        public string HomePhone { get; set; }

        [DataType(DataType.Text), DisplayName("Last Name"), MaxLength(64), StringLength(64)]
        public string LastName { get; set; }

        public DateTime LastUpdated { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Mobile Phone"), MaxLength(32), StringLength(32)]
        public string MobilePhone { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public int? OriginalId { get; set; }

        [DataType(DataType.Text), MaxLength(2), StringLength(2)]
        public string State { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Work Phone"), MaxLength(32), StringLength(32)]
        public string WorkPhone { get; set; }

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
                return string.Concat(Address1, string.IsNullOrEmpty(Address2) ? "" : Environment.NewLine + Address2, Environment.NewLine, City, ", ", State, " ", ZipCode);
            }
        }

        [DisplayName("Restore Point"), NotMapped]
        public DateTime? ChangeDate { get; set; }

        [NotMapped]
        public List<SelectListItem> ChangeDates { get; set; }

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
        #endregion

        #region One-to-One Relationships
        [ForeignKey("ContactTypeId")]
        public ContactType ContactType { get; set; }
        #endregion
    }

    [Table("tblContactTypes")]
    public class ContactType
    {
        #region Properties
        [DisplayName("Active?")]
        public bool Active { get; set; } // IsActive

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int ContactTypeId { get; set; }

        [DataType(DataType.Text), MaxLength(64), Required, StringLength(64)]
        public string Name { get; set; }

        public int? OriginalId { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion
    }
}
