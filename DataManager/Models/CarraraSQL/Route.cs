namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Route
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Route()
        {
            Jobs = new HashSet<Job>();
            Loads = new HashSet<Load>();
            RouteMileages = new HashSet<RouteMileage>();
        }
        #endregion

        #region Properties
        [HiddenInput]
        public int RouteID { get; set; }

        [DisplayName("Route Name"), StringLength(150)]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Load> Loads { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RouteMileage> RouteMileages { get; set; }
        #endregion
    }
}
