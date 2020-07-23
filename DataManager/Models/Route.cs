using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("Routes")]
    public partial class Route
    {
        #region Methods
        public Route()
        {
            IsActive = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int RouteID { get; set; }

        [StringLength(150)]
        public string RouteName { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [DisplayName("Is Cement Hauling Route?")]
        public bool IsCementHaulingRoute { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ICollection<Load> Loads { get; set; }

        public virtual ICollection<RouteMileage> RouteMileages { get; set; }
        #endregion
    }
}
