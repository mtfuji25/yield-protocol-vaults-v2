using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("MarkSizeTypes")]
    public class MarkSizeType
    {
        #region Methods
        public MarkSizeType()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int MarkSizeTypeID { get; set; }

        [DisplayName("Mark Size Type Name"), Required, StringLength(50)]
        public string MarkSizeTypeName { get; set; }

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
