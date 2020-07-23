using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("States")]
    public class State
    {
        #region Methods
        public State()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int StateID { get; set; }

        [DisplayName("State Name"), Required, StringLength(50)]
        public string StateName { get; set; }

        [DisplayName("Abbreviation"), StringLength(2)]
        public string StateAbbreviation { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<RouteMileage> RouteMileages { get; set; }
        #endregion
    }
}
