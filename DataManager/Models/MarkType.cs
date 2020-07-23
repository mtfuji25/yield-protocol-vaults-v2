using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("MarkTypes")]
    public class MarkType
    {
        #region Methods
        public MarkType()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int MarkTypeID { get; set; }

        [DisplayName("Mark Type Name"), Required, StringLength(50)]
        public string MarkTypeName { get; set; }

        [DisplayName("Weight Formula")]
        public int? WeightFormulaTypeID { get; set; }

        [DisplayName("Size Type")]
        public int? MarkSizeTypeID { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Foreign Keys
        [ForeignKey("MarkSizeTypeID")]
        public virtual MarkSizeType MarkSizeType { get; set; }

        [ForeignKey("WeightFormulaTypeID")]
        public virtual WeightFormulaType WeightFormulaType { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Mark> Marks { get; set; }

        public virtual ICollection<Pour> Pours { get; set; }
        #endregion
    }
}
