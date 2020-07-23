namespace DataManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Modules")]
    public class Module
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Module()
        {
            ModulePermissions = new HashSet<ModulePermission>();
        }

        public int ModuleID { get; set; }

        [Required]
        [StringLength(50)]
        public string ModuleName { get; set; }

        public int ParentID { get; set; }

        public int? Sort { get; set; }

        public int? ImageIndex { get; set; }

        public bool IsDefaultModule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModulePermission> ModulePermissions { get; set; }
    }
}
