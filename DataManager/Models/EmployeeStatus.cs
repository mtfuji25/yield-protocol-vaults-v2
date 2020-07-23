using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("EmployeeStatuses")]
    public class EmployeeStatus
    {
        #region Methods
        public EmployeeStatus()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int EmployeeStatusID { get; set; }

        [DisplayName("Employee Status Name"), Required, StringLength(50)]
        public string EmployeeStatusName { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Employee> Employees { get; set; }
        #endregion
    }
}
