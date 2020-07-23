namespace DataManager.Models.CarraraSQL
{
    using System.Data.Entity;

    public partial class CarraraSQL : DbContext
    {
        public CarraraSQL()
            : base("name=CarraraSQL")
        {
        }

        public static CarraraSQL Create()
        {
            return new CarraraSQL();
        }

        public virtual DbSet<ApplicationError> ApplicationErrors { get; set; }
        public virtual DbSet<Bed> Beds { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactType> ContactTypes { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeStatus> EmployeeStatuses { get; set; }
        public virtual DbSet<HoursAssignment> HoursAssignments { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobSiteLocation> JobSiteLocations { get; set; }
        public virtual DbSet<JobStatu> JobStatus { get; set; }
        public virtual DbSet<LoadMark> LoadMarks { get; set; }
        public virtual DbSet<Load> Loads { get; set; }
        public virtual DbSet<LoadStatu> LoadStatus { get; set; }
        public virtual DbSet<LoadType> LoadTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }
        public virtual DbSet<MarkSizeType> MarkSizeTypes { get; set; }
        public virtual DbSet<MarkType> MarkTypes { get; set; }
        public virtual DbSet<MeterType> MeterTypes { get; set; }
        public virtual DbSet<Mix> Mixes { get; set; }
        public virtual DbSet<ModulePermission> ModulePermissions { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<NonConformingReportAction> NonConformingReportActions { get; set; }
        public virtual DbSet<NonConformingReport> NonConformingReports { get; set; }
        public virtual DbSet<PourDetail> PourDetails { get; set; }
        public virtual DbSet<Pour> Pours { get; set; }
        public virtual DbSet<PourStatu> PourStatus { get; set; }
        public virtual DbSet<RediMixDestination> RediMixDestinations { get; set; }
        public virtual DbSet<RediMixDriverAssignment> RediMixDriverAssignments { get; set; }
        public virtual DbSet<RediMixLoad> RediMixLoads { get; set; }
        public virtual DbSet<RediMixLoadType> RediMixLoadTypes { get; set; }
        public virtual DbSet<RediMixOrder> RediMixOrders { get; set; }
        public virtual DbSet<RediMixOrderStatu> RediMixOrderStatus { get; set; }
        public virtual DbSet<RouteMileage> RouteMileages { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TimeCard> TimeCards { get; set; }
        public virtual DbSet<TimeClockEntry> TimeClockEntries { get; set; }
        public virtual DbSet<TimeClockPermission> TimeClockPermissions { get; set; }
        public virtual DbSet<TouchScreen> TouchScreens { get; set; }
        public virtual DbSet<TrailerType> TrailerTypes { get; set; }
        public virtual DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        public virtual DbSet<WeightFormulaType> WeightFormulaTypes { get; set; }
        public virtual DbSet<ContactsView> ContactsViews { get; set; }
        public virtual DbSet<PourDetailsView> PourDetailsViews { get; set; }
        public virtual DbSet<ProductionAnalysi> ProductionAnalysis { get; set; }
        public virtual DbSet<rptHoursAssignment> rptHoursAssignments { get; set; }
        public virtual DbSet<vVehicle> vVehicles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationError>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationError>()
                .Property(e => e.StackTrace)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationError>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationError>()
                .Property(e => e.Source)
                .IsUnicode(false);

            modelBuilder.Entity<Bed>()
                .Property(e => e.BedName)
                .IsUnicode(false);

            modelBuilder.Entity<Bed>()
                .Property(e => e.Width)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Bed>()
                .Property(e => e.Length)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Bed>()
                .Property(e => e.StrandLength)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Bed>()
                .Property(e => e.DefaultJackNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Bed>()
                .Property(e => e.DefaultSlump)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Bed>()
                .Property(e => e.DefaultReleaseSpec)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Bed>()
                .Property(e => e.Default28DaySpec)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Bed>()
                .HasMany(e => e.Pours)
                .WithRequired(e => e.Bed)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.HomePhone)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.WorkPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.MobilePhone)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.EMail)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Company)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.StreetAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Zip)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.FaxNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .HasMany(e => e.Jobs)
                .WithOptional(e => e.Contact)
                .HasForeignKey(e => e.ErectorID);

            modelBuilder.Entity<Contact>()
                .HasMany(e => e.Loads)
                .WithOptional(e => e.Contact)
                .HasForeignKey(e => e.IndependentDriverID);

            modelBuilder.Entity<ContactType>()
                .Property(e => e.ContactTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.DepartmentCode)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.DepartmentName)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.ViewPointOffsetAccount)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Rate)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Employee>()
                .Property(e => e.SSN)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Street)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.VacationHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Employee>()
                .Property(e => e.YearsOfService)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Zip)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.DriverKey)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmergencyContactName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmergencyContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.HoursAssignments)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Jobs)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.ProjectManagerID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Loads)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.DriverID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.NonConformingReports)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.CompletedByID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.RediMixDriverAssignments)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.DriverID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.RediMixLoads)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.DriverID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.TimeCards)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.TimeClockEntries)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmployeeStatus>()
                .Property(e => e.EmployeeStatusName)
                .IsUnicode(false);

            modelBuilder.Entity<HoursAssignment>()
                .Property(e => e.OvertimeHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<HoursAssignment>()
                .Property(e => e.RegularHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Job>()
                .Property(e => e.JobName)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.JobNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.GeneralContractor)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.Directions)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.TravelTime)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Job>()
                .Property(e => e.Street)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.ZIP)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.JobSiteLocations)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.NonConformingReports)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.Pours)
                .WithOptional(e => e.Job)
                .HasForeignKey(e => e.DefaultJobID);

            modelBuilder.Entity<JobSiteLocation>()
                .Property(e => e.JobSiteLocationName)
                .IsUnicode(false);

            modelBuilder.Entity<JobStatu>()
                .Property(e => e.JobStatusName)
                .IsUnicode(false);

            modelBuilder.Entity<Load>()
                .Property(e => e.MarkList)
                .IsUnicode(false);

            modelBuilder.Entity<Load>()
                .Property(e => e.TotalMarkWeight)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Load>()
                .Property(e => e.TotalWeight)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Load>()
                .Property(e => e.DriverNotes)
                .IsUnicode(false);

            modelBuilder.Entity<Load>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<Load>()
                .Property(e => e.MarkLayout)
                .IsUnicode(false);

            modelBuilder.Entity<Load>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<LoadStatu>()
                .Property(e => e.LoadStatusName)
                .IsUnicode(false);

            modelBuilder.Entity<LoadType>()
                .Property(e => e.LoadTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.LocationName)
                .IsUnicode(false);

            modelBuilder.Entity<Mark>()
                .Property(e => e.MarkNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Mark>()
                .Property(e => e.MarkDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Mark>()
                .Property(e => e.Length)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Mark>()
                .Property(e => e.Thickness)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Mark>()
                .Property(e => e.Weight)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Mark>()
                .Property(e => e.Width)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Mark>()
                .Property(e => e.SquareFeet)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Mark>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<Mark>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Mark>()
                .HasMany(e => e.LoadMarks)
                .WithRequired(e => e.Mark)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mark>()
                .HasMany(e => e.PourDetails)
                .WithRequired(e => e.Mark)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MarkSizeType>()
                .Property(e => e.MarkSizeTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<MarkType>()
                .Property(e => e.MarkTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<MarkType>()
                .HasMany(e => e.Pours)
                .WithOptional(e => e.MarkType)
                .HasForeignKey(e => e.DefaultMarkTypeID);

            modelBuilder.Entity<MeterType>()
                .Property(e => e.MeterTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<Mix>()
                .Property(e => e.MixName)
                .IsUnicode(false);

            modelBuilder.Entity<Mix>()
                .Property(e => e.MixDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Mix>()
                .HasMany(e => e.Beds)
                .WithOptional(e => e.Mix)
                .HasForeignKey(e => e.DefaultMixID);

            modelBuilder.Entity<Module>()
                .Property(e => e.ModuleName)
                .IsUnicode(false);

            modelBuilder.Entity<NonConformingReportAction>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<NonConformingReport>()
                .Property(e => e.MarkNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NonConformingReport>()
                .Property(e => e.Finding)
                .IsUnicode(false);

            modelBuilder.Entity<NonConformingReport>()
                .Property(e => e.Action)
                .IsUnicode(false);

            modelBuilder.Entity<NonConformingReport>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<PourDetail>()
                .Property(e => e.Camber)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PourDetail>()
                .Property(e => e.MarkRange)
                .IsUnicode(false);

            modelBuilder.Entity<PourDetail>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Pour>()
                .Property(e => e.SlipNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Pour>()
                .Property(e => e.JackNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Pour>()
                .Property(e => e.ReleaseTest1)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.ReleaseTest2)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.ReleaseTestAverage)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.C28DayTest1)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.C28DayTest2)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.C28DayTestAverage)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.OtherTest1)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.OtherTest2)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.OtherTestAverage)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.Workability)
                .IsUnicode(false);

            modelBuilder.Entity<Pour>()
                .Property(e => e.Thickness)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.YardsConcrete)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.YardsGrout)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<Pour>()
                .Property(e => e.UnitWeight)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.ConcreteTemperature)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.AmbientTemperature)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.Weather)
                .IsUnicode(false);

            modelBuilder.Entity<Pour>()
                .Property(e => e.Yield)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.Slump)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.VSI)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.Air)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.SpecReleaseStrength)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.Spec28DayStrength)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.SpecMaxSlump)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.SpecAir)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.SpecAirError)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Pour>()
                .Property(e => e.SpecNotes)
                .IsUnicode(false);

            modelBuilder.Entity<Pour>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Pour>()
                .Property(e => e.PourScheduleNotes)
                .IsUnicode(false);

            modelBuilder.Entity<Pour>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<PourStatu>()
                .Property(e => e.PourStatusName)
                .IsUnicode(false);

            modelBuilder.Entity<PourStatu>()
                .HasMany(e => e.Pours)
                .WithRequired(e => e.PourStatu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RediMixDestination>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<RediMixDestination>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<RediMixDestination>()
                .Property(e => e.ZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<RediMixLoad>()
                .Property(e => e.YardsConcrete)
                .HasPrecision(18, 4);

            modelBuilder.Entity<RediMixLoad>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<RediMixLoadType>()
                .Property(e => e.RediMixLoadTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<RediMixOrder>()
                .Property(e => e.Customer)
                .IsUnicode(false);

            modelBuilder.Entity<RediMixOrder>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<RediMixOrder>()
                .Property(e => e.Duration)
                .HasPrecision(18, 4);

            modelBuilder.Entity<RediMixOrder>()
                .Property(e => e.YardsConcrete)
                .HasPrecision(18, 4);

            modelBuilder.Entity<RediMixOrder>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<RediMixOrder>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<RediMixOrder>()
                .HasMany(e => e.RediMixLoads)
                .WithOptional(e => e.RediMixOrder)
                .WillCascadeOnDelete();

            modelBuilder.Entity<RediMixOrderStatu>()
                .Property(e => e.OrderStatusName)
                .IsUnicode(false);

            modelBuilder.Entity<Route>()
                .Property(e => e.RouteName)
                .IsUnicode(false);

            modelBuilder.Entity<Route>()
                .HasMany(e => e.RouteMileages)
                .WithRequired(e => e.Route)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<State>()
                .Property(e => e.StateName)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .Property(e => e.StateAbbreviation)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .HasMany(e => e.RouteMileages)
                .WithRequired(e => e.State)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TimeCard>()
                .Property(e => e.RegularHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<TimeCard>()
                .Property(e => e.OvertimeHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<TimeCard>()
                .Property(e => e.VacationHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<TimeCard>()
                .Property(e => e.HolidayHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<TimeCard>()
                .Property(e => e.TotalHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<TimeClockEntry>()
                .Property(e => e.HoursWorked)
                .HasPrecision(18, 4);

            modelBuilder.Entity<TimeClockEntry>()
                .Property(e => e.ApprovedBy)
                .IsUnicode(false);

            modelBuilder.Entity<TimeClockEntry>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<TouchScreen>()
                .Property(e => e.TouchScreenName)
                .IsUnicode(false);

            modelBuilder.Entity<TrailerType>()
                .Property(e => e.TrailerTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<TrailerType>()
                .HasMany(e => e.Loads)
                .WithOptional(e => e.TrailerType)
                .HasForeignKey(e => e.RequiredTrailerTypeID);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.VehicleCode)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.VehicleName)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.VehicleNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.EZPass)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Make)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Registration)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.VIN)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.HoursAssignments)
                .WithOptional(e => e.Vehicle)
                .HasForeignKey(e => e.DrivenVehicleID);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.HoursAssignments1)
                .WithOptional(e => e.Vehicle1)
                .HasForeignKey(e => e.MaintainedVehicleID);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.Loads)
                .WithOptional(e => e.Vehicle)
                .HasForeignKey(e => e.VehicleID);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.Loads1)
                .WithOptional(e => e.Vehicle1)
                .HasForeignKey(e => e.TrailerID);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.RediMixDriverAssignments)
                .WithRequired(e => e.Vehicle)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vehicle>()
                .HasMany(e => e.TimeClockEntries)
                .WithOptional(e => e.Vehicle)
                .HasForeignKey(e => e.DrivenVehicleID);

            modelBuilder.Entity<VehicleType>()
                .Property(e => e.VehicleTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<VehicleType>()
                .HasMany(e => e.Vehicles)
                .WithRequired(e => e.VehicleType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WeightFormulaType>()
                .Property(e => e.WeightFormulaTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.HomePhone)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.WorkPhone)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.MobilePhone)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.EMail)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.Company)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.StreetAddress)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.Zip)
                .IsUnicode(false);

            modelBuilder.Entity<ContactsView>()
                .Property(e => e.ContactTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.MarkNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.MarkDescription)
                .IsUnicode(false);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.JobNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.MarkTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.Length)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.Thickness)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.Weight)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.Width)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.SquareFeet)
                .HasPrecision(18, 4);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.TotalSquareFeet)
                .HasPrecision(29, 4);

            modelBuilder.Entity<PourDetailsView>()
                .Property(e => e.TotalWeight)
                .HasPrecision(29, 4);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.MarkNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.MarkDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.JobNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.MarkTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.Length)
                .HasPrecision(18, 4);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.Thickness)
                .HasPrecision(18, 4);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.Weight)
                .HasPrecision(18, 4);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.Width)
                .HasPrecision(18, 4);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.SquareFeet)
                .HasPrecision(18, 4);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.BedName)
                .IsUnicode(false);

            modelBuilder.Entity<ProductionAnalysi>()
                .Property(e => e.JobName)
                .IsUnicode(false);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.EmployeeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.EmployeeDepartmentCode)
                .IsUnicode(false);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.CostCenter)
                .IsUnicode(false);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.DrivenVehicleCode)
                .IsUnicode(false);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.JobNumber)
                .IsUnicode(false);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.CalcRegularHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.CalcOvertimeHours)
                .HasPrecision(18, 4);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.VacationHours)
                .HasPrecision(19, 4);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.HolidayHours)
                .HasPrecision(19, 4);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.ExcusedHours)
                .HasPrecision(19, 4);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.UnExcusedHours)
                .HasPrecision(19, 4);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.TotalHours)
                .HasPrecision(19, 4);

            modelBuilder.Entity<rptHoursAssignment>()
                .Property(e => e.ViewPointOffsetAccount)
                .IsUnicode(false);

            modelBuilder.Entity<vVehicle>()
                .Property(e => e.VehicleTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<vVehicle>()
                .Property(e => e.VehicleCode)
                .IsUnicode(false);

            modelBuilder.Entity<vVehicle>()
                .Property(e => e.VehicleName)
                .IsUnicode(false);

            modelBuilder.Entity<vVehicle>()
                .Property(e => e.VehicleNumber)
                .IsUnicode(false);

            modelBuilder.Entity<vVehicle>()
                .Property(e => e.EZPass)
                .IsUnicode(false);

            modelBuilder.Entity<vVehicle>()
                .Property(e => e.Make)
                .IsUnicode(false);

            modelBuilder.Entity<vVehicle>()
                .Property(e => e.Registration)
                .IsUnicode(false);

            modelBuilder.Entity<vVehicle>()
                .Property(e => e.VIN)
                .IsUnicode(false);

            modelBuilder.Entity<vVehicle>()
                .Property(e => e.MeterTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<vVehicle>()
                .Property(e => e.LocationName)
                .IsUnicode(false);
        }
    }
}
