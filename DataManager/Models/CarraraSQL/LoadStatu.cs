namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class LoadStatu
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoadStatu()
        {
            Loads = new HashSet<Load>();
        }
#endregion

        #region Properties
        [Key]
        public int LoadStatusID { get; set; }

        [DisplayName("Load Status Name"), Required, StringLength(50)]
        public string LoadStatusName { get; set; }
#endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Load> Loads { get; set; }
        #endregion
    }
}
