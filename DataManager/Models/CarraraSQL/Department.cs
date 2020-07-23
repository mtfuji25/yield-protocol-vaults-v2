namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class Department
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            Employees = new HashSet<Employee>();
            HoursAssignments = new HashSet<HoursAssignment>();
            Loads = new HashSet<Load>();
            TimeClockPermissions = new HashSet<TimeClockPermission>();
        }
        #endregion

        #region Properties
        [HiddenInput]
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
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoursAssignment> HoursAssignments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Load> Loads { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeClockPermission> TimeClockPermissions { get; set; }
        #endregion
    }
}
