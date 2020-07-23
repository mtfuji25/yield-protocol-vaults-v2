namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class State
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public State()
        {
            RouteMileages = new HashSet<RouteMileage>();
        }
        #endregion

        #region Properties
        [HiddenInput]
        public int StateID { get; set; }

        [DisplayName("State Name"), Required, StringLength(50)]
        public string StateName { get; set; }

        [DisplayName("Abbreviation"), StringLength(2)]
        public string StateAbbreviation { get; set; }
        #endregion

        #region Collections
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RouteMileage> RouteMileages { get; set; }
        #endregion

        #region NotMapped
        [NotMapped]
        public string Action { get; set; }
        #endregion
    }
}
