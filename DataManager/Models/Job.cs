using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace DataManager.Models
{
    [Table("Jobs")]
    public class Job
    {
        #region Methods
        public Job()
        {
        }
        #endregion

        #region Properties
        [DisplayName("Job ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), HiddenInput, Key]
        public int JobID { get; set; }

        [DisplayName("Job Number"), Required, StringLength(50)]
        public string JobNumber { get; set; }

        [DisplayName("Job Name"), Required, StringLength(50)]
        public string JobName { get; set; }

        [DisplayName("Erector")]
        public int? ErectorID { get; set; }

        [DisplayName("Project Manager")]
        public int? ProjectManagerID { get; set; }

        [DisplayName("Street"), StringLength(100)]
        public string Street { get; set; }

        [DisplayName("City"), StringLength(50)]
        public string City { get; set; }

        [DisplayName("State"), StringLength(50)]
        public string State { get; set; }

        [DisplayName("Zip Code"), StringLength(50)]
        public string ZIP { get; set; }

        [DisplayName("General Contractor"), StringLength(100)]
        public string GeneralContractor { get; set; }

        [DisplayName("Status")]
        public int? JobStatusID { get; set; }

        public string Directions { get; set; }

        [DisplayName("Travel Time")]
        public decimal? TravelTime { get; set; }

        [DisplayName("Route")]
        public int? RouteID { get; set; }
        #endregion

        #region Not Mapped
        [NotMapped]
        public string Action { get; set; }

        [NotMapped]
        public string FullAddress
        {
            get
            {
                return string.Concat(Environment.NewLine, Street, Environment.NewLine, City, ", ", State, " ", ZIP);
            }
        }

        [DisplayName("Total Mileage (One Way)"), NotMapped]
        public string TotalMileage
        {
            get
            {
                if (RouteID.HasValue && Route.RouteMileages.Any())
                {
                    return Route.RouteMileages.Select(x => x.Mileage).Sum().ToString("F2");

                }
                return string.Empty;
            }
        }
        #endregion

        #region Foreign Keys
        [ForeignKey("ErectorID")]
        public virtual Contact Contact { get; set; }

        [ForeignKey("ProjectManagerID")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("JobStatusID")]
        public virtual JobStatu JobStatu { get; set; }

        [ForeignKey("RouteID")]
        public virtual Route Route { get; set; }
        #endregion

        #region Collections
        public virtual ICollection<HoursAssignment> HoursAssignments { get; set; }

        public virtual ICollection<JobSiteLocation> JobSiteLocations { get; set; }

        public virtual ICollection<Load> Loads { get; set; }

        public virtual ICollection<Mark> Marks { get; set; }

        public virtual ICollection<NonConformingReport> NonConformingReports { get; set; }

        public virtual ICollection<Pour> Pours { get; set; }
        #endregion
    }
}
