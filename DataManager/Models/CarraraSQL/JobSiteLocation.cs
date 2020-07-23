namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class JobSiteLocation
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobSiteLocation()
        {
            Loads = new HashSet<Load>();
        }
        #endregion

        #region Properties
        [HiddenInput]
        public int JobSiteLocationID { get; set; }

        [DisplayName("Job")]
        public int JobID { get; set; }

        [DisplayName("Name"), Required, StringLength(100)]
        public string JobSiteLocationName { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Foreign Keys
        public virtual Job Job { get; set; }
        #endregion

        #region Collections
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Load> Loads { get; set; }
        #endregion
    }
}
