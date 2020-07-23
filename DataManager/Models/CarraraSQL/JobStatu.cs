namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class JobStatu
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobStatu()
        {
            Jobs = new HashSet<Job>();
        }
        #endregion

        #region Properties
        [HiddenInput, Key]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }
        #endregion
    }
}
