namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class LoadType
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoadType()
        {
            Loads = new HashSet<Load>();
        }
        #endregion

        #region Properties
        [HiddenInput, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LoadTypeID { get; set; }

        [DisplayName("Load Type Name"), Required, StringLength(50)]
        public string LoadTypeName { get; set; }
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
