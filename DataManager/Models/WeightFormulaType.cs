using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("WeightFormulaTypes")]
    public class WeightFormulaType
    {
        #region Methods
        public WeightFormulaType()
        {
            MarkTypes = new HashSet<MarkType>();
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int WeightFormulaTypeID { get; set; }

        [DisplayName("Weight Formula Type Name"), Required, StringLength(50)]
        public string WeightFormulaTypeName { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<MarkType> MarkTypes { get; set; }
        #endregion
    }
}
