namespace DataManager.Models.CarraraSQL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class rptHoursAssignment
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployeeID { get; set; }

        [StringLength(50)]
        public string EmployeeNumber { get; set; }

        [StringLength(525)]
        public string FullName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string EmployeeDepartmentCode { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool IsHourly { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AssignmentTypeID { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime DateWorked { get; set; }

        [StringLength(50)]
        public string CostCenter { get; set; }

        [StringLength(50)]
        public string DrivenVehicleCode { get; set; }

        [StringLength(50)]
        public string JobNumber { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "numeric")]
        public decimal CalcRegularHours { get; set; }

        [Key]
        [Column(Order = 6, TypeName = "numeric")]
        public decimal CalcOvertimeHours { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VacationHours { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HolidayHours { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ExcusedHours { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? UnExcusedHours { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TotalHours { get; set; }

        public bool? ShouldExportUsage { get; set; }

        [StringLength(50)]
        public string ViewPointOffsetAccount { get; set; }
    }
}
