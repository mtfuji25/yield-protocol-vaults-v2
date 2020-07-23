namespace DataManager.Models.CarraraSQL
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class Bed
    {
        #region Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bed()
        {
            Pours = new HashSet<Pour>();
        }
        #endregion

        #region Properties
        [HiddenInput]
        public int BedID { get; set; }

        [DisplayName("Bed Name"), Required, StringLength(50)]
        public string BedName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Width { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Length { get; set; }

        [DisplayName("Strand Length"), Column(TypeName = "numeric")]
        public decimal? StrandLength { get; set; }

        [DisplayName("Max # Of Pours Per Day")]
        public int? MaxNumOfPoursPerDay { get; set; }

        [DisplayName("Mix")]
        public int? DefaultMixID { get; set; }

        [DisplayName("Jack #"), StringLength(100)]
        public string DefaultJackNumber { get; set; }

        [DisplayName("# Of Cylinders")]
        public int? DefaultNumOfCylinders { get; set; }

        [DisplayName("Slump"), Column(TypeName = "numeric")]
        public decimal? DefaultSlump { get; set; }

        [DisplayName("Release Spec"), Column(TypeName = "numeric")]
        public decimal? DefaultReleaseSpec { get; set; }

        [DisplayName("28 Day Spec"), Column(TypeName = "numeric")]
        public decimal? Default28DaySpec { get; set; }
        #endregion

        #region Foreign Keys
        public virtual Mix Mix { get; set; }
        #endregion

        #region Collections
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pour> Pours { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion
    }
}
