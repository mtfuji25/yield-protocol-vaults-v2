namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class Mix
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mix()
        {
            Beds = new HashSet<Bed>();
            Pours = new HashSet<Pour>();
            RediMixOrders = new HashSet<RediMixOrder>();
        }
        #endregion

        #region Properties
        [HiddenInput]
        public int MixID { get; set; }

        [DisplayName("Mix Name"), Required, StringLength(50)]
        public string MixName { get; set; }

        [DisplayName("Mix Description"), StringLength(150)]
        public string MixDescription { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bed> Beds { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pour> Pours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RediMixOrder> RediMixOrders { get; set; }
        #endregion
    }
}
