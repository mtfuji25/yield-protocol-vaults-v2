using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("TrailerTypes")]
    public class TrailerType
    {
        #region Methods
        public TrailerType()
        {
            Active = true;

        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int TrailerTypeID { get; set; }

        [DisplayName("Trailer Type Name"), Required, StringLength(255)]
        public string TrailerTypeName { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Load> Loads { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
        #endregion
    }
}
