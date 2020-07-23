using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("JobSiteLocations")]
    public class JobSiteLocation
    {
        #region Methods
        public JobSiteLocation()
        {
            Loads = new HashSet<Load>();
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
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
        [ForeignKey("JobID")]
        public virtual Job Job { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Load> Loads { get; set; }
        #endregion
    }
}
