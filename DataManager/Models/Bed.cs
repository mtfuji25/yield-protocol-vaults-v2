using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("Beds")]
    public class Bed
    {
        #region Methods
        public Bed()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int BedID { get; set; }

        [DisplayName("Bed Name"), Required, StringLength(50)]
        public string BedName { get; set; }

        [DisplayName("Width")]
        public decimal? Width { get; set; }

        [DisplayName("Length")]
        public decimal? Length { get; set; }

        [DisplayName("Strand Length")]
        public decimal? StrandLength { get; set; }

        [DisplayName("Max # Of Pours Per Day")]
        public int? MaxNumOfPoursPerDay { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }

        [DisplayName("Mix")]
        public int? DefaultMixID { get; set; }

        [DisplayName("Jack #"), StringLength(100)]
        public string DefaultJackNumber { get; set; }

        [DisplayName("# Of Cylinders")]
        public int? DefaultNumOfCylinders { get; set; }

        [DisplayName("Slump")]
        public decimal? DefaultSlump { get; set; }

        [DisplayName("Release Spec")]
        public decimal? DefaultReleaseSpec { get; set; }

        [DisplayName("28 Day Spec")]
        public decimal? Default28DaySpec { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Foreign Keys
        [ForeignKey("DefaultMixID")]
        public virtual Mix Mix { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Pour> Pours { get; set; }
        #endregion
    }
}
