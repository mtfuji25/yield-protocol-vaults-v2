namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    [Table("EmployeeStatuses")]
    public partial class EmployeeStatus
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployeeStatus()
        {
            Employees = new HashSet<Employee>();
        }
        #endregion

        #region Properties
        [HiddenInput]
        public int EmployeeStatusID { get; set; }

        [DisplayName("Employee Status Name"), Required, StringLength(50)]
        public string EmployeeStatusName { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }
        #endregion
    }
}
