using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("JobStatus")]
    public class JobStatu
    {
        #region Methods
        public JobStatu()
        {
            IsActive = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int JobStatusID { get; set; }

        [DisplayName("Job Status Name"), Required, StringLength(50)]
        public string JobStatusName { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Job> Jobs { get; set; }
        #endregion
    }
}
