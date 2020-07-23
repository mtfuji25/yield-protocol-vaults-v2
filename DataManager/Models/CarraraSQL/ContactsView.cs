namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactsView")]
    public partial class ContactsView
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContactID { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string DisplayName { get; set; }

        [StringLength(50)]
        public string HomePhone { get; set; }

        [StringLength(50)]
        public string WorkPhone { get; set; }

        [StringLength(50)]
        public string MobilePhone { get; set; }

        [StringLength(100)]
        public string EMail { get; set; }

        [StringLength(100)]
        public string Company { get; set; }

        [StringLength(100)]
        public string StreetAddress { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string Zip { get; set; }

        public int? ContactTypeID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ContactTypeName { get; set; }

        [StringLength(4000)]
        public string FullAddress { get; set; }
    }
}
