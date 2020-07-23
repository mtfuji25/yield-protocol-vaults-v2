namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RediMixDestination
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RediMixDestination()
        {
            RediMixOrders = new HashSet<RediMixOrder>();
        }

        public int RediMixDestinationID { get; set; }

        [StringLength(150)]
        public string City { get; set; }

        [StringLength(2)]
        public string State { get; set; }

        [StringLength(50)]
        public string ZipCode { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NYMilesFromMiddlebury { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VTMilesFromMiddlebury { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NYMilesFromRutland { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VTMilesFromRutland { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NYMilesFromCrownPoint { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VTMilesFromCrownPoint { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RediMixOrder> RediMixOrders { get; set; }
    }
}
