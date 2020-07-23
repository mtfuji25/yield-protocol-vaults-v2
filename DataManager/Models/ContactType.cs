using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("ContactTypes")]
    public class ContactType
    {
        #region Methods
        public ContactType()
        {
            Active = true;
        }
        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int ContactTypeID { get; set; }

        [DisplayName("Contact Type"), MaxLength(64), Required, StringLength(64)]
        public string ContactTypeName { get; set; }

        [DisplayName("Active")]
        public bool Active { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<Contact> Contacts { get; set; }
        #endregion
    }
}
