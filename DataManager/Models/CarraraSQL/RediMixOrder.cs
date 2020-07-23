namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RediMixOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RediMixOrder()
        {
            RediMixLoads = new HashSet<RediMixLoad>();
        }

        public int RediMixOrderID { get; set; }

        [StringLength(50)]
        public string Customer { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        public int? RediMixDestinationID { get; set; }

        public int? LocationID { get; set; }

        public int? OrderStatusID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? StartTime { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Duration { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? YardsConcrete { get; set; }

        public int? MixID { get; set; }

        public int? NumberOfLoads { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Column(TypeName = "text")]
        public string Notes { get; set; }

        public bool IsComplete { get; set; }

        public virtual Location Location1 { get; set; }

        public virtual Mix Mix { get; set; }

        public virtual RediMixDestination RediMixDestination { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RediMixLoad> RediMixLoads { get; set; }

        public virtual RediMixOrderStatu RediMixOrderStatu { get; set; }
    }
}
