using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("LoadStatus")]
    public class LoadStatu
    {
        #region Methods
        public LoadStatu()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int LoadStatusID { get; set; }

        [DisplayName("Load Status Name"), Required, StringLength(50)]
        public string LoadStatusName { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Load> Loads { get; set; }
        #endregion
    }
}
