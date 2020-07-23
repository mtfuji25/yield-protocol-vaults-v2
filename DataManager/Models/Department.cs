using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("Departments")]
    public class Department
    {
        #region Methods
        public Department()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int DepartmentID { get; set; }

        [DisplayName("Code"), Required, StringLength(50)]
        public string DepartmentCode { get; set; }

        [DisplayName("Name"), Required, StringLength(100)]
        public string DepartmentName { get; set; }

        [DisplayName("Viewpoint Offset Account"), StringLength(50)]
        public string ViewPointOffsetAccount { get; set; }

        public int? DefaultAssignmentTypeID { get; set; }

        [DisplayName("Default Cost Center")]
        public int? DefaultCostCenterID { get; set; }

        [DisplayName("Default Lunch Setting")]
        public bool DefaultLunch { get; set; }

        [DisplayName("Auto Adjust Clock In")]
        public bool AutoAdjustClockIn { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<HoursAssignment> HoursAssignments { get; set; }

        public virtual ICollection<Load> Loads { get; set; }

        public virtual ICollection<TimeClockPermission> TimeClockPermissions { get; set; }
        #endregion
    }
}
