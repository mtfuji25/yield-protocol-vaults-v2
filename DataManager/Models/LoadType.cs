using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("LoadTypes")]
    public class LoadType
    {
        #region Methods
        public LoadType()
        {
            Loads = new HashSet<Load>();
        }
        #endregion

        #region Properties
        [HiddenInput, Key]
        public int LoadTypeID { get; set; }

        [DisplayName("Load Type Name"), Required, StringLength(50)]
        public string LoadTypeName { get; set; }

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
