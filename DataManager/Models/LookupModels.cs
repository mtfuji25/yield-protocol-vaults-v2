using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("tblDepartments")]
    public class Department
    {
        #region Properties
        [DisplayName("Active?")]
        public bool Active { get; set; } // IsActive

        [DisplayName("Auto Adjust Clock In?")]
        public bool AutoAdjustClockIn { get; set; }

        [DataType(DataType.Text), MaxLength(64), Required, StringLength(64)]
        public string Code { get; set; }

        [DisplayName("Default Assignment Type")]
        public int? DefaultAssignmentTypeId { get; set; }

        [DisplayName("Default Cost Center")]
        public int? DefaultCostCenterId { get; set; }

        [DisplayName("Default Lunch?")]
        public bool DefaultLunch { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int DepartmentId { get; set; }

        [DataType(DataType.Text), MaxLength(128), Required, StringLength(128)]
        public string Name { get; set; }

        public int? OriginalId { get; set; }

        [DisplayName("View Point Offset Account")]
        public string ViewPointOffsetAccount { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<UserProfile> Employees { get; set; }
        #endregion
    }

    [Table("tblEmployeeStatuses")]
    public class EmployeeStatus
    {
        #region Properties
        [DisplayName("Active?")]
        public bool Active { get; set; } // IsActive

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int EmployeeStatusId { get; set; }

        [DataType(DataType.Text), MaxLength(64), Required, StringLength(64)]
        public string Name { get; set; }

        public int? OriginalId { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion
    }

    [Table("tblLocations")]
    public class Location
    {
        #region Properties
        [DisplayName("Active?")]
        public bool Active { get; set; } // IsActive

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int LocationId { get; set; }

        [DataType(DataType.Text), MaxLength(64), Required, StringLength(64)]
        public string LocationName { get; set; }

        public int? OriginalId { get; set; }
        #endregion

        #region One-to-Many Relationships
        public virtual ICollection<UserProfile> Employees { get; set; }

        public virtual ICollection<RediMixOrder> RediMixOrders { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion
    }
}