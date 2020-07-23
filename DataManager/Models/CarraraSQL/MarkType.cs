namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class MarkType
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MarkType()
        {
            Marks = new HashSet<Mark>();
            Pours = new HashSet<Pour>();
        }
        #endregion

        #region Properties
        [HiddenInput]
        public int MarkTypeID { get; set; }

        [DisplayName("Mark Type Name"), Required, StringLength(50)]
        public string MarkTypeName { get; set; }

        [DisplayName("Weight Formula")]
        public int? WeightFormulaTypeID { get; set; }

        [DisplayName("Size Type")]
        public int? MarkSizeTypeID { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Foreign Keys
        public virtual MarkSizeType MarkSizeType { get; set; }

        public virtual WeightFormulaType WeightFormulaType { get; set; }
        #endregion

        #region Collections
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mark> Marks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pour> Pours { get; set; }
        #endregion
    }
}
