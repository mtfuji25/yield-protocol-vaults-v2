namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class MarkSizeType
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MarkSizeType()
        {
            MarkTypes = new HashSet<MarkType>();
        }
        #endregion

        #region Properties
        [HiddenInput]
        public int MarkSizeTypeID { get; set; }

        [DisplayName("Mark Size Type Name"), Required, StringLength(50)]
        public string MarkSizeTypeName { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MarkType> MarkTypes { get; set; }
        #endregion
    }
}
