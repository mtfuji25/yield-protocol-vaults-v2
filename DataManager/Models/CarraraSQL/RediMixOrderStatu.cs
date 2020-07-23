namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RediMixOrderStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RediMixOrderStatu()
        {
            RediMixOrders = new HashSet<RediMixOrder>();
        }

        [Key]
        public int OrderStatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderStatusName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RediMixOrder> RediMixOrders { get; set; }
    }
}
