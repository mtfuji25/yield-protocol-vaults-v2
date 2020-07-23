using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("PourStatus")]
    public class PourStatu
    {
        #region Methods
        public PourStatu()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int PourStatusID { get; set; }

        [DisplayName("Pour Status Name"), Required, StringLength(50)]
        public string PourStatusName { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Pour> Pours { get; set; }
        #endregion
    }
}
