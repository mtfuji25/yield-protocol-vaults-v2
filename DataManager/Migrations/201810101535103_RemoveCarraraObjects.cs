namespace DataManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCarraraObjects : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Beds", "DefaultMixID", "dbo.Mixes");
            DropForeignKey("dbo.Contacts", "ContactTypeID", "dbo.ContactTypes");
            DropForeignKey("dbo.Jobs", "ErectorID", "dbo.Contacts");
            DropForeignKey("dbo.Employees", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Employees", "EmployeeStatusID", "dbo.EmployeeStatuses");
            DropForeignKey("dbo.HoursAssignments", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.HoursAssignments", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.Employees", "VehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.HoursAssignments", "DrivenVehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.HoursAssignments", "MaintainedVehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.Loads", "VehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.Loads", "TrailerID", "dbo.Vehicles");
            DropForeignKey("dbo.Employees", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.RediMixOrders", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.RediMixOrders", "MixID", "dbo.Mixes");
            DropForeignKey("dbo.RediMixOrders", "RediMixDestinationID", "dbo.RediMixDestinations");
            DropForeignKey("dbo.RediMixLoads", "RediMixLoadTypeID", "dbo.RediMixLoadTypes");
            DropForeignKey("dbo.RediMixLoads", "VehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.RediMixLoads", "RediMixOrderID", "dbo.RediMixOrders");
            DropForeignKey("dbo.RediMixOrders", "OrderStatusID", "dbo.RediMixOrderStatus");
            DropForeignKey("dbo.Vehicles", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.RediMixDriverAssignments", "VehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.TimeClockEntries", "DrivenVehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.Loads", "RequiredTrailerTypeID", "dbo.TrailerTypes");
            DropForeignKey("dbo.Vehicles", "TrailerTypeID", "dbo.TrailerTypes");
            DropForeignKey("dbo.VehicleMaintenance", "VehicleID", "dbo.Vehicles");
            DropForeignKey("dbo.VehicleTypes", "MeterTypeID", "dbo.MeterTypes");
            DropForeignKey("dbo.Vehicles", "VehicleTypeID", "dbo.VehicleTypes");
            DropForeignKey("dbo.HoursAssignments", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Jobs", "ProjectManagerID", "dbo.Employees");
            DropForeignKey("dbo.Loads", "DriverID", "dbo.Employees");
            DropForeignKey("dbo.ModulePermissions", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.ModulePermissions", "ModuleID", "dbo.Modules");
            DropForeignKey("dbo.NonConformingReports", "PourID", "dbo.Pours");
            DropForeignKey("dbo.NonConformingReports", "CompletedByID", "dbo.Employees");
            DropForeignKey("dbo.RediMixDriverAssignments", "DriverID", "dbo.Employees");
            DropForeignKey("dbo.RediMixLoads", "DriverID", "dbo.Employees");
            DropForeignKey("dbo.TimeCards", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.TimeClockEntries", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.TimeClockPermissions", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.TimeClockPermissions", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Loads", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Loads", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.Loads", "JobSiteLocationID", "dbo.JobSiteLocations");
            DropForeignKey("dbo.LoadMarks", "LoadID", "dbo.Loads");
            DropForeignKey("dbo.Marks", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.LoadMarks", "MarkID", "dbo.Marks");
            DropForeignKey("dbo.Marks", "MarkTypeID", "dbo.MarkTypes");
            DropForeignKey("dbo.MarkTypes", "MarkSizeTypeID", "dbo.MarkSizeTypes");
            DropForeignKey("dbo.Pours", "DefaultMarkTypeID", "dbo.MarkTypes");
            DropForeignKey("dbo.MarkTypes", "WeightFormulaTypeID", "dbo.WeightFormulaTypes");
            DropForeignKey("dbo.PourDetails", "PourID", "dbo.Pours");
            DropForeignKey("dbo.PourDetails", "MarkID", "dbo.Marks");
            DropForeignKey("dbo.Loads", "LoadStatusID", "dbo.LoadStatus");
            DropForeignKey("dbo.Loads", "LoadTypeID", "dbo.LoadTypes");
            DropForeignKey("dbo.Jobs", "RouteID", "dbo.Routes");
            DropForeignKey("dbo.Loads", "RouteID", "dbo.Routes");
            DropForeignKey("dbo.RouteMileage", "StateID", "dbo.States");
            DropForeignKey("dbo.RouteMileage", "RouteID", "dbo.Routes");
            DropForeignKey("dbo.Loads", "IndependentDriverID", "dbo.Contacts");
            DropForeignKey("dbo.JobSiteLocations", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "JobStatusID", "dbo.JobStatus");
            DropForeignKey("dbo.NonConformingReports", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.Pours", "DefaultJobID", "dbo.Jobs");
            DropForeignKey("dbo.Pours", "MixID", "dbo.Mixes");
            DropForeignKey("dbo.Pours", "PourStatusID", "dbo.PourStatus");
            DropForeignKey("dbo.Pours", "BedID", "dbo.Beds");
            DropForeignKey("dbo.AspNetUsers", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.Beds", new[] { "DefaultMixID" });
            DropIndex("dbo.Pours", new[] { "BedID" });
            DropIndex("dbo.Pours", new[] { "PourStatusID" });
            DropIndex("dbo.Pours", new[] { "MixID" });
            DropIndex("dbo.Pours", new[] { "DefaultJobID" });
            DropIndex("dbo.Pours", new[] { "DefaultMarkTypeID" });
            DropIndex("dbo.Jobs", new[] { "ErectorID" });
            DropIndex("dbo.Jobs", new[] { "ProjectManagerID" });
            DropIndex("dbo.Jobs", new[] { "JobStatusID" });
            DropIndex("dbo.Jobs", new[] { "RouteID" });
            DropIndex("dbo.Contacts", new[] { "ContactTypeID" });
            DropIndex("dbo.Loads", new[] { "DriverID" });
            DropIndex("dbo.Loads", new[] { "IndependentDriverID" });
            DropIndex("dbo.Loads", new[] { "JobID" });
            DropIndex("dbo.Loads", new[] { "JobSiteLocationID" });
            DropIndex("dbo.Loads", new[] { "LoadTypeID" });
            DropIndex("dbo.Loads", new[] { "RequiredTrailerTypeID" });
            DropIndex("dbo.Loads", new[] { "TrailerID" });
            DropIndex("dbo.Loads", new[] { "VehicleID" });
            DropIndex("dbo.Loads", new[] { "DepartmentID" });
            DropIndex("dbo.Loads", new[] { "LoadStatusID" });
            DropIndex("dbo.Loads", new[] { "RouteID" });
            DropIndex("dbo.Employees", new[] { "DepartmentID" });
            DropIndex("dbo.Employees", new[] { "LocationID" });
            DropIndex("dbo.Employees", new[] { "EmployeeStatusID" });
            DropIndex("dbo.Employees", new[] { "VehicleID" });
            DropIndex("dbo.HoursAssignments", new[] { "DepartmentID" });
            DropIndex("dbo.HoursAssignments", new[] { "DrivenVehicleID" });
            DropIndex("dbo.HoursAssignments", new[] { "EmployeeID" });
            DropIndex("dbo.HoursAssignments", new[] { "JobID" });
            DropIndex("dbo.HoursAssignments", new[] { "MaintainedVehicleID" });
            DropIndex("dbo.Vehicles", new[] { "VehicleTypeID" });
            DropIndex("dbo.Vehicles", new[] { "LocationID" });
            DropIndex("dbo.Vehicles", new[] { "TrailerTypeID" });
            DropIndex("dbo.RediMixOrders", new[] { "RediMixDestinationID" });
            DropIndex("dbo.RediMixOrders", new[] { "LocationID" });
            DropIndex("dbo.RediMixOrders", new[] { "OrderStatusID" });
            DropIndex("dbo.RediMixOrders", new[] { "MixID" });
            DropIndex("dbo.RediMixLoads", new[] { "RediMixOrderID" });
            DropIndex("dbo.RediMixLoads", new[] { "VehicleID" });
            DropIndex("dbo.RediMixLoads", new[] { "DriverID" });
            DropIndex("dbo.RediMixLoads", new[] { "RediMixLoadTypeID" });
            DropIndex("dbo.RediMixDriverAssignments", new[] { "VehicleID" });
            DropIndex("dbo.RediMixDriverAssignments", new[] { "DriverID" });
            DropIndex("dbo.TimeClockEntries", new[] { "EmployeeID" });
            DropIndex("dbo.TimeClockEntries", new[] { "DrivenVehicleID" });
            DropIndex("dbo.VehicleMaintenance", new[] { "VehicleID" });
            DropIndex("dbo.VehicleTypes", new[] { "MeterTypeID" });
            DropIndex("dbo.ModulePermissions", new[] { "EmployeeID" });
            DropIndex("dbo.ModulePermissions", new[] { "ModuleID" });
            DropIndex("dbo.NonConformingReports", new[] { "PourID" });
            DropIndex("dbo.NonConformingReports", new[] { "JobID" });
            DropIndex("dbo.NonConformingReports", new[] { "CompletedByID" });
            DropIndex("dbo.TimeCards", new[] { "EmployeeID" });
            DropIndex("dbo.TimeClockPermissions", new[] { "EmployeeID" });
            DropIndex("dbo.TimeClockPermissions", new[] { "DepartmentID" });
            DropIndex("dbo.JobSiteLocations", new[] { "JobID" });
            DropIndex("dbo.LoadMarks", new[] { "LoadID" });
            DropIndex("dbo.LoadMarks", new[] { "MarkID" });
            DropIndex("dbo.Marks", new[] { "MarkTypeID" });
            DropIndex("dbo.Marks", new[] { "JobID" });
            DropIndex("dbo.MarkTypes", new[] { "WeightFormulaTypeID" });
            DropIndex("dbo.MarkTypes", new[] { "MarkSizeTypeID" });
            DropIndex("dbo.PourDetails", new[] { "PourID" });
            DropIndex("dbo.PourDetails", new[] { "MarkID" });
            DropIndex("dbo.RouteMileage", new[] { "RouteID" });
            DropIndex("dbo.RouteMileage", new[] { "StateID" });
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeID" });

            DropTable("dbo.ApplicationErrors");
            DropTable("dbo.HoursAssignments");
            DropTable("dbo.TimeClockEntries");
            DropTable("dbo.VehicleMaintenance");
            DropTable("dbo.ModulePermissions");
            DropTable("dbo.Modules");
            DropTable("dbo.NonConformingReports");
            DropTable("dbo.TimeCards");
            DropTable("dbo.TimeClockPermissions");
            DropTable("dbo.LoadMarks");
            DropTable("dbo.PourDetails");
            DropTable("dbo.RouteMileage");
            DropTable("dbo.States");
            DropTable("dbo.ChangeLogs");
            DropTable("dbo.NonConformingReportActions");
            //DropTable("dbo.sysdiagram");
            DropTable("dbo.Marks");
            DropTable("dbo.RediMixDriverAssignments");
            DropTable("dbo.RediMixLoads");
            DropTable("dbo.RediMixLoadTypes");
            DropTable("dbo.RediMixOrders");
            DropTable("dbo.RediMixDestinations");
            DropTable("dbo.RediMixOrderStatus");
            DropTable("dbo.Loads");
            DropTable("dbo.Pours");
            DropTable("dbo.PourStatus");
            DropTable("dbo.Beds");
            DropTable("dbo.Mixes");
            DropTable("dbo.LoadTypes");
            DropTable("dbo.LoadStatus");
            DropTable("dbo.MarkTypes");
            DropTable("dbo.WeightFormulaTypes");
            DropTable("dbo.MarkSizeTypes");
            DropTable("dbo.JobSiteLocations");
            DropTable("dbo.Jobs");
            DropTable("dbo.JobStatus");
            DropTable("dbo.Routes");
            DropTable("dbo.Contacts");
            DropTable("dbo.ContactTypes");

            DropTable("dbo.Employees");
            DropTable("dbo.EmployeeStatuses");
            DropTable("dbo.Vehicles");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.MeterTypes");
            DropTable("dbo.TrailerTypes");
            DropTable("dbo.Departments");
            DropTable("dbo.Locations");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.sysdiagram",
            //    c => new
            //        {
            //            diagram_id = c.Int(nullable: false, identity: true),
            //            name = c.String(nullable: false, maxLength: 128),
            //            principal_id = c.Int(nullable: false),
            //            version = c.Int(),
            //            definition = c.Binary(),
            //        })
            //    .PrimaryKey(t => t.diagram_id);
            
            CreateTable(
                "dbo.NonConformingReportActions",
                c => new
                    {
                        ActionID = c.Int(nullable: false, identity: true),
                        ActionName = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.ActionID);
            
            CreateTable(
                "dbo.ChangeLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateChanged = c.DateTime(nullable: false),
                        EntityName = c.String(),
                        NewValue = c.String(),
                        OldValue = c.String(),
                        PrimaryKeyValue = c.String(),
                        PropertyName = c.String(),
                        UserName = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PourStatus",
                c => new
                    {
                        PourStatusID = c.Int(nullable: false, identity: true),
                        PourStatusName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PourStatusID);
            
            CreateTable(
                "dbo.JobStatus",
                c => new
                    {
                        JobStatusID = c.Int(nullable: false, identity: true),
                        JobStatusName = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.JobStatusID);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateID = c.Int(nullable: false, identity: true),
                        StateName = c.String(nullable: false, maxLength: 50, unicode: false),
                        StateAbbreviation = c.String(maxLength: 2, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StateID);
            
            CreateTable(
                "dbo.RouteMileage",
                c => new
                    {
                        RouteMileageID = c.Int(nullable: false, identity: true),
                        RouteID = c.Int(nullable: false),
                        StateID = c.Int(nullable: false),
                        Mileage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RouteMileageID);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        RouteID = c.Int(nullable: false, identity: true),
                        RouteName = c.String(maxLength: 150, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        IsCementHaulingRoute = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RouteID);
            
            CreateTable(
                "dbo.LoadTypes",
                c => new
                    {
                        LoadTypeID = c.Int(nullable: false, identity: true),
                        LoadTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LoadTypeID);
            
            CreateTable(
                "dbo.LoadStatus",
                c => new
                    {
                        LoadStatusID = c.Int(nullable: false, identity: true),
                        LoadStatusName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LoadStatusID);
            
            CreateTable(
                "dbo.PourDetails",
                c => new
                    {
                        PourDetailID = c.Int(nullable: false, identity: true),
                        PourID = c.Int(nullable: false),
                        MarkID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Camber = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        MarkRange = c.String(maxLength: 50, unicode: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                    })
                .PrimaryKey(t => t.PourDetailID);
            
            CreateTable(
                "dbo.WeightFormulaTypes",
                c => new
                    {
                        WeightFormulaTypeID = c.Int(nullable: false, identity: true),
                        WeightFormulaTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.WeightFormulaTypeID);
            
            CreateTable(
                "dbo.MarkSizeTypes",
                c => new
                    {
                        MarkSizeTypeID = c.Int(nullable: false, identity: true),
                        MarkSizeTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MarkSizeTypeID);
            
            CreateTable(
                "dbo.MarkTypes",
                c => new
                    {
                        MarkTypeID = c.Int(nullable: false, identity: true),
                        MarkTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        WeightFormulaTypeID = c.Int(),
                        MarkSizeTypeID = c.Int(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MarkTypeID);
            
            CreateTable(
                "dbo.Marks",
                c => new
                    {
                        MarkID = c.Int(nullable: false, identity: true),
                        MarkNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        MarkDescription = c.String(maxLength: 100, unicode: false),
                        MarkTypeID = c.Int(),
                        JobID = c.Int(nullable: false),
                        Length = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        RequiredQuantity = c.Int(),
                        Thickness = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        Weight = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        Width = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        SquareFeet = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        IsDrawn = c.Boolean(nullable: false),
                        IsReleased = c.Boolean(nullable: false),
                        DateDrawn = c.DateTime(),
                        DateReleased = c.DateTime(),
                        Location = c.String(maxLength: 150, unicode: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                    })
                .PrimaryKey(t => t.MarkID);
            
            CreateTable(
                "dbo.LoadMarks",
                c => new
                    {
                        LoadMarkID = c.Int(nullable: false, identity: true),
                        LoadID = c.Int(nullable: false),
                        MarkID = c.Int(nullable: false),
                        Top = c.Int(),
                        Left = c.Int(),
                    })
                .PrimaryKey(t => t.LoadMarkID);
            
            CreateTable(
                "dbo.JobSiteLocations",
                c => new
                    {
                        JobSiteLocationID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        JobSiteLocationName = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.JobSiteLocationID);
            
            CreateTable(
                "dbo.TimeClockPermissions",
                c => new
                    {
                        TimeClockPermissionsID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TimeClockPermissionsID);
            
            CreateTable(
                "dbo.TimeCards",
                c => new
                    {
                        TimeCardID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        WeekEndDate = c.DateTime(),
                        RegularHours = c.Decimal(precision: 18, scale: 4),
                        OvertimeHours = c.Decimal(precision: 18, scale: 4),
                        VacationHours = c.Decimal(precision: 18, scale: 4),
                        HolidayHours = c.Decimal(precision: 18, scale: 4),
                        TotalHours = c.Decimal(precision: 18, scale: 4),
                        Assigned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TimeCardID);
            
            CreateTable(
                "dbo.NonConformingReports",
                c => new
                    {
                        NonConformingReportID = c.Int(nullable: false, identity: true),
                        PourID = c.Int(nullable: false),
                        JobID = c.Int(nullable: false),
                        DateChecked = c.DateTime(),
                        MarkNumber = c.String(maxLength: 50, unicode: false),
                        Finding = c.String(maxLength: 100, unicode: false),
                        Action = c.String(maxLength: 100, unicode: false),
                        DueDate = c.DateTime(),
                        IsCompleted = c.Boolean(nullable: false),
                        CompletedByID = c.Int(),
                        CompletedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                    })
                .PrimaryKey(t => t.NonConformingReportID);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        ModuleID = c.Int(nullable: false, identity: true),
                        ModuleName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ParentID = c.Int(nullable: false),
                        Sort = c.Int(),
                        ImageIndex = c.Int(),
                        IsDefaultModule = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ModuleID);
            
            CreateTable(
                "dbo.ModulePermissions",
                c => new
                    {
                        ModulePermissionsID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        ModuleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ModulePermissionsID);
            
            CreateTable(
                "dbo.MeterTypes",
                c => new
                    {
                        MeterTypeID = c.Int(nullable: false, identity: true),
                        MeterTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MeterTypeID);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        VehicleTypeID = c.Int(nullable: false, identity: true),
                        VehicleTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        MeterTypeID = c.Int(),
                        GreaseInterval = c.Int(),
                        OilInterval = c.Int(),
                        ServiceInterval = c.Int(),
                        IncludeInReadyMixDispatch = c.Boolean(nullable: false),
                        IncludeInDispatch = c.Boolean(nullable: false),
                        NeedsAnnualService = c.Boolean(nullable: false),
                        NeedsGrease = c.Boolean(nullable: false),
                        NeedsOil = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.VehicleTypeID);
            
            CreateTable(
                "dbo.VehicleMaintenance",
                c => new
                    {
                        VehicleMaintenanceID = c.Int(nullable: false, identity: true),
                        VehicleID = c.Int(nullable: false),
                        DateOfService = c.DateTime(nullable: false),
                        Meter = c.Int(),
                        Grease = c.Boolean(nullable: false),
                        Service = c.Boolean(nullable: false),
                        FuelAmount = c.Decimal(precision: 18, scale: 2),
                        Oil = c.Boolean(),
                    })
                .PrimaryKey(t => t.VehicleMaintenanceID);
            
            CreateTable(
                "dbo.TrailerTypes",
                c => new
                    {
                        TrailerTypeID = c.Int(nullable: false, identity: true),
                        TrailerTypeName = c.String(nullable: false, maxLength: 255, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TrailerTypeID);
            
            CreateTable(
                "dbo.TimeClockEntries",
                c => new
                    {
                        TimeClockEntryID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        OriginalClockIn = c.DateTime(),
                        OriginalClockOut = c.DateTime(),
                        ClockIn = c.DateTime(),
                        ClockOut = c.DateTime(),
                        HoursWorked = c.Decimal(precision: 18, scale: 4),
                        AssignmentType = c.Int(),
                        CostCenterID = c.Int(),
                        DrivenVehicleID = c.Int(),
                        Lunch = c.Boolean(nullable: false),
                        ApprovedBy = c.String(unicode: false),
                        ApprovedDateTime = c.DateTime(),
                        Notes = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.TimeClockEntryID);
            
            CreateTable(
                "dbo.RediMixDriverAssignments",
                c => new
                    {
                        RediMixDriverAssignmentID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        VehicleID = c.Int(nullable: false),
                        DriverID = c.Int(),
                    })
                .PrimaryKey(t => t.RediMixDriverAssignmentID);
            
            CreateTable(
                "dbo.RediMixOrderStatus",
                c => new
                    {
                        OrderStatusID = c.Int(nullable: false, identity: true),
                        OrderStatusName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderStatusID);
            
            CreateTable(
                "dbo.RediMixLoadTypes",
                c => new
                    {
                        RediMixLoadTypeID = c.Int(nullable: false, identity: true),
                        RediMixLoadTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RediMixLoadTypeID);
            
            CreateTable(
                "dbo.RediMixLoads",
                c => new
                    {
                        RediMixLoadID = c.Int(nullable: false, identity: true),
                        RediMixOrderID = c.Int(),
                        LoadNumber = c.Int(),
                        VehicleID = c.Int(),
                        DriverID = c.Int(),
                        RediMixLoadTypeID = c.Int(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        YardsConcrete = c.Decimal(precision: 18, scale: 4),
                        IsShortLoad = c.Boolean(nullable: false),
                        TenOne = c.Boolean(nullable: false),
                        TenOneTime = c.DateTime(),
                        TenTwo = c.Boolean(nullable: false),
                        TenTwoTime = c.DateTime(),
                        TenThree = c.Boolean(nullable: false),
                        TenThreeTime = c.DateTime(),
                        TenNine = c.Boolean(nullable: false),
                        TenNineTime = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                    })
                .PrimaryKey(t => t.RediMixLoadID);
            
            CreateTable(
                "dbo.RediMixDestinations",
                c => new
                    {
                        RediMixDestinationID = c.Int(nullable: false, identity: true),
                        City = c.String(maxLength: 150, unicode: false),
                        State = c.String(maxLength: 2, unicode: false),
                        ZipCode = c.String(maxLength: 50, unicode: false),
                        Latitude = c.Double(),
                        Longitude = c.Double(),
                        NYMilesFromMiddlebury = c.Decimal(precision: 18, scale: 2),
                        VTMilesFromMiddlebury = c.Decimal(precision: 18, scale: 2),
                        NYMilesFromRutland = c.Decimal(precision: 18, scale: 2),
                        VTMilesFromRutland = c.Decimal(precision: 18, scale: 2),
                        NYMilesFromCrownPoint = c.Decimal(precision: 18, scale: 2),
                        VTMilesFromCrownPoint = c.Decimal(precision: 18, scale: 2),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RediMixDestinationID);
            
            CreateTable(
                "dbo.RediMixOrders",
                c => new
                    {
                        RediMixOrderID = c.Int(nullable: false, identity: true),
                        Customer = c.String(maxLength: 50, unicode: false),
                        Location = c.String(maxLength: 50, unicode: false),
                        RediMixDestinationID = c.Int(),
                        LocationID = c.Int(),
                        OrderStatusID = c.Int(),
                        StartDate = c.DateTime(),
                        StartTime = c.DateTime(),
                        Duration = c.Decimal(precision: 18, scale: 4),
                        YardsConcrete = c.Decimal(precision: 18, scale: 4),
                        MixID = c.Int(),
                        NumberOfLoads = c.Int(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                        Notes = c.String(unicode: false),
                        IsComplete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RediMixOrderID);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LocationID);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        VehicleID = c.Int(nullable: false, identity: true),
                        VehicleCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        VehicleName = c.String(maxLength: 255, unicode: false),
                        VehicleNumber = c.String(maxLength: 50, unicode: false),
                        VehicleTypeID = c.Int(nullable: false),
                        DefaultDriverID = c.Int(),
                        LocationID = c.Int(),
                        EZPass = c.String(maxLength: 50, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        Length = c.Int(),
                        Make = c.String(maxLength: 50, unicode: false),
                        Registration = c.String(maxLength: 50, unicode: false),
                        RegistrationDate = c.DateTime(),
                        RegistrationExpiration = c.DateTime(),
                        VIN = c.String(maxLength: 50, unicode: false),
                        Weight = c.Int(),
                        Year = c.Int(),
                        TrailerTypeID = c.Int(),
                        NextOil = c.Int(),
                        NextGrease = c.Int(),
                        NextService = c.DateTime(),
                        CurrentMeterReading = c.Int(),
                        LastOil = c.Int(),
                        LastGrease = c.Int(),
                        LastService = c.DateTime(),
                        ShouldExportUsage = c.Boolean(nullable: false),
                        IsOnRoad = c.Boolean(nullable: false),
                        NumberOfAxles = c.Int(),
                        GrossAxleWeightRating = c.Int(),
                        GrossVehicleWeightRating = c.Int(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VehicleID);
            
            CreateTable(
                "dbo.HoursAssignments",
                c => new
                    {
                        HoursAssignmentID = c.Int(nullable: false, identity: true),
                        AssignmentTypeID = c.Int(nullable: false),
                        DateWorked = c.DateTime(nullable: false),
                        DepartmentID = c.Int(),
                        DrivenVehicleID = c.Int(),
                        EmployeeID = c.Int(nullable: false),
                        JobID = c.Int(),
                        OvertimeHours = c.Decimal(nullable: false, precision: 18, scale: 4, storeType: "numeric"),
                        RegularHours = c.Decimal(nullable: false, precision: 18, scale: 4, storeType: "numeric"),
                        MaintainedVehicleID = c.Int(),
                    })
                .PrimaryKey(t => t.HoursAssignmentID);
            
            CreateTable(
                "dbo.EmployeeStatuses",
                c => new
                    {
                        EmployeeStatusID = c.Int(nullable: false, identity: true),
                        EmployeeStatusName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeStatusID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        EmployeeNumber = c.String(maxLength: 50, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 50, unicode: false),
                        FirstName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Mobile = c.String(maxLength: 50, unicode: false),
                        Phone = c.String(maxLength: 50, unicode: false),
                        Street = c.String(maxLength: 255, unicode: false),
                        City = c.String(maxLength: 50, unicode: false),
                        State = c.String(maxLength: 50, unicode: false),
                        Zip = c.String(maxLength: 50, unicode: false),
                        SSN = c.String(maxLength: 50, unicode: false),
                        BirthDate = c.DateTime(),
                        DepartmentID = c.Int(),
                        LocationID = c.Int(),
                        DefaultCostCenterID = c.Int(),
                        ShiftStartTime = c.DateTime(),
                        ShiftEndTime = c.DateTime(),
                        IsHourly = c.Boolean(nullable: false),
                        IsDriver = c.Boolean(nullable: false),
                        IsProjectManager = c.Boolean(nullable: false),
                        EmployeeStatusID = c.Int(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        VacationHours = c.Decimal(precision: 18, scale: 4),
                        YearsOfService = c.Decimal(precision: 18, scale: 4),
                        VehicleID = c.Int(),
                        Rate = c.Decimal(storeType: "smallmoney"),
                        EmergencyContactName = c.String(maxLength: 200, unicode: false),
                        EmergencyContactPhone = c.String(maxLength: 50, unicode: false),
                        IsAdministrator = c.Boolean(nullable: false),
                        UserName = c.String(maxLength: 50, unicode: false),
                        DriverKey = c.String(maxLength: 20, unicode: false),
                        IsRediMixVisible = c.Boolean(nullable: false),
                        DefaultAssignmentTypeID = c.Int(),
                        Active = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        DepartmentCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        DepartmentName = c.String(nullable: false, maxLength: 100, unicode: false),
                        ViewPointOffsetAccount = c.String(maxLength: 50, unicode: false),
                        DefaultAssignmentTypeID = c.Int(),
                        DefaultCostCenterID = c.Int(),
                        DefaultLunch = c.Boolean(nullable: false),
                        AutoAdjustClockIn = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Loads",
                c => new
                    {
                        LoadID = c.Int(nullable: false, identity: true),
                        LoadNumber = c.Int(),
                        CarraraDepartureDate = c.DateTime(),
                        CarraraDepartureTime = c.DateTime(),
                        DeliveryDate = c.DateTime(),
                        DeliveryTime = c.DateTime(),
                        DriverID = c.Int(),
                        HasDriverBeenContacted = c.Boolean(nullable: false),
                        IndependentDriverID = c.Int(),
                        IsDropLoad = c.Boolean(nullable: false),
                        IsLoaded = c.Boolean(nullable: false),
                        JobID = c.Int(),
                        JobSiteLocationID = c.Int(),
                        LoadTypeID = c.Int(),
                        MarkList = c.String(maxLength: 255, unicode: false),
                        RequiredTrailerTypeID = c.Int(),
                        SiteArrivalDate = c.DateTime(),
                        SiteArrivalTime = c.DateTime(),
                        SiteDepartureDate = c.DateTime(),
                        SiteDepartureTime = c.DateTime(),
                        TotalMarkWeight = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        TotalWeight = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        TrailerID = c.Int(),
                        VehicleID = c.Int(),
                        DriverNotes = c.String(unicode: false),
                        Notes = c.String(unicode: false),
                        MarkLayout = c.String(unicode: false),
                        DepartmentID = c.Int(),
                        AssignmentTypeID = c.Int(),
                        IsQuickLoad = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                        LoadStatusID = c.Int(),
                        AltSequenceNumber = c.Int(),
                        RouteID = c.Int(),
                    })
                .PrimaryKey(t => t.LoadID);
            
            CreateTable(
                "dbo.ContactTypes",
                c => new
                    {
                        ContactTypeID = c.Int(nullable: false, identity: true),
                        ContactTypeName = c.String(nullable: false, maxLength: 64, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ContactTypeID);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactID = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(maxLength: 100, unicode: false),
                        Company = c.String(maxLength: 100, unicode: false),
                        FirstName = c.String(maxLength: 50, unicode: false),
                        LastName = c.String(maxLength: 50, unicode: false),
                        HomePhone = c.String(maxLength: 50, unicode: false),
                        WorkPhone = c.String(maxLength: 50, unicode: false),
                        MobilePhone = c.String(maxLength: 50, unicode: false),
                        FaxNumber = c.String(maxLength: 50, unicode: false),
                        EMail = c.String(maxLength: 100, unicode: false),
                        StreetAddress = c.String(maxLength: 100, unicode: false),
                        City = c.String(maxLength: 50, unicode: false),
                        State = c.String(maxLength: 50, unicode: false),
                        Zip = c.String(maxLength: 50, unicode: false),
                        ContactTypeID = c.Int(),
                        Notes = c.String(unicode: false),
                        LastUpdated = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ContactID);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobID = c.Int(nullable: false, identity: true),
                        JobNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        JobName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ErectorID = c.Int(),
                        ProjectManagerID = c.Int(),
                        Street = c.String(maxLength: 100, unicode: false),
                        City = c.String(maxLength: 50, unicode: false),
                        State = c.String(maxLength: 50, unicode: false),
                        ZIP = c.String(maxLength: 50, unicode: false),
                        GeneralContractor = c.String(maxLength: 100, unicode: false),
                        JobStatusID = c.Int(),
                        Directions = c.String(unicode: false),
                        TravelTime = c.Decimal(precision: 18, scale: 4),
                        RouteID = c.Int(),
                    })
                .PrimaryKey(t => t.JobID);
            
            CreateTable(
                "dbo.Pours",
                c => new
                    {
                        PourID = c.Int(nullable: false, identity: true),
                        PourDate = c.DateTime(nullable: false),
                        BedID = c.Int(nullable: false),
                        PourStatusID = c.Int(nullable: false),
                        PourTime = c.DateTime(),
                        MixID = c.Int(),
                        SlipNumber = c.String(maxLength: 50, unicode: false),
                        DefaultJobID = c.Int(),
                        DefaultMarkTypeID = c.Int(),
                        ThreeInchStrands = c.Int(),
                        FiveInchStrands = c.Int(),
                        SixInchStrands = c.Int(),
                        JackNumber = c.String(maxLength: 100, unicode: false),
                        ReleaseTestDate = c.DateTime(),
                        ReleaseTest1 = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        ReleaseTest2 = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        ReleaseTestAverage = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        _28DayTestDate = c.DateTime(name: "28DayTestDate"),
                        _28DayTest1 = c.Decimal(name: "28DayTest1", precision: 18, scale: 4, storeType: "numeric"),
                        _28DayTest2 = c.Decimal(name: "28DayTest2", precision: 18, scale: 4, storeType: "numeric"),
                        _28DayTestAverage = c.Decimal(name: "28DayTestAverage", precision: 18, scale: 4, storeType: "numeric"),
                        OtherTestDate = c.DateTime(),
                        OtherTest1 = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        OtherTest2 = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        OtherTestAverage = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        NumberOfCylinders = c.Int(),
                        Workability = c.String(maxLength: 50, unicode: false),
                        Thickness = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        YardsConcrete = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        YardsGrout = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        Location = c.String(maxLength: 100, unicode: false),
                        UnitWeight = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        ConcreteTemperature = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        AmbientTemperature = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        Weather = c.String(maxLength: 250, unicode: false),
                        Yield = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        Slump = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        VSI = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        Air = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        SpecReleaseStrength = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        Spec28DayStrength = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        SpecMaxSlump = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        SpecAir = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        SpecAirError = c.Decimal(precision: 18, scale: 4, storeType: "numeric"),
                        SpecNotes = c.String(maxLength: 250, unicode: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                        PourScheduleNotes = c.String(unicode: false, storeType: "text"),
                        Notes = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.PourID);
            
            CreateTable(
                "dbo.Mixes",
                c => new
                    {
                        MixID = c.Int(nullable: false, identity: true),
                        MixName = c.String(nullable: false, maxLength: 50, unicode: false),
                        MixDescription = c.String(maxLength: 150, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MixID);
            
            CreateTable(
                "dbo.Beds",
                c => new
                    {
                        BedID = c.Int(nullable: false, identity: true),
                        BedName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Width = c.Decimal(precision: 18, scale: 4),
                        Length = c.Decimal(precision: 18, scale: 4),
                        StrandLength = c.Decimal(precision: 18, scale: 4),
                        MaxNumOfPoursPerDay = c.Int(),
                        Active = c.Boolean(nullable: false),
                        DefaultMixID = c.Int(),
                        DefaultJackNumber = c.String(maxLength: 100, unicode: false),
                        DefaultNumOfCylinders = c.Int(),
                        DefaultSlump = c.Decimal(precision: 18, scale: 4),
                        DefaultReleaseSpec = c.Decimal(precision: 18, scale: 4),
                        Default28DaySpec = c.Decimal(precision: 18, scale: 4),
                    })
                .PrimaryKey(t => t.BedID);
            
            CreateTable(
                "dbo.ApplicationErrors",
                c => new
                    {
                        ApplicationErrorID = c.Int(nullable: false, identity: true),
                        Message = c.String(unicode: false),
                        StackTrace = c.String(unicode: false),
                        Username = c.String(maxLength: 50, unicode: false),
                        Date = c.DateTime(),
                        Source = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ApplicationErrorID);
            
            CreateIndex("dbo.AspNetUsers", "EmployeeID");
            CreateIndex("dbo.RouteMileage", "StateID");
            CreateIndex("dbo.RouteMileage", "RouteID");
            CreateIndex("dbo.PourDetails", "MarkID");
            CreateIndex("dbo.PourDetails", "PourID");
            CreateIndex("dbo.MarkTypes", "MarkSizeTypeID");
            CreateIndex("dbo.MarkTypes", "WeightFormulaTypeID");
            CreateIndex("dbo.Marks", "JobID");
            CreateIndex("dbo.Marks", "MarkTypeID");
            CreateIndex("dbo.LoadMarks", "MarkID");
            CreateIndex("dbo.LoadMarks", "LoadID");
            CreateIndex("dbo.JobSiteLocations", "JobID");
            CreateIndex("dbo.TimeClockPermissions", "DepartmentID");
            CreateIndex("dbo.TimeClockPermissions", "EmployeeID");
            CreateIndex("dbo.TimeCards", "EmployeeID");
            CreateIndex("dbo.NonConformingReports", "CompletedByID");
            CreateIndex("dbo.NonConformingReports", "JobID");
            CreateIndex("dbo.NonConformingReports", "PourID");
            CreateIndex("dbo.ModulePermissions", "ModuleID");
            CreateIndex("dbo.ModulePermissions", "EmployeeID");
            CreateIndex("dbo.VehicleTypes", "MeterTypeID");
            CreateIndex("dbo.VehicleMaintenance", "VehicleID");
            CreateIndex("dbo.TimeClockEntries", "DrivenVehicleID");
            CreateIndex("dbo.TimeClockEntries", "EmployeeID");
            CreateIndex("dbo.RediMixDriverAssignments", "DriverID");
            CreateIndex("dbo.RediMixDriverAssignments", "VehicleID");
            CreateIndex("dbo.RediMixLoads", "RediMixLoadTypeID");
            CreateIndex("dbo.RediMixLoads", "DriverID");
            CreateIndex("dbo.RediMixLoads", "VehicleID");
            CreateIndex("dbo.RediMixLoads", "RediMixOrderID");
            CreateIndex("dbo.RediMixOrders", "MixID");
            CreateIndex("dbo.RediMixOrders", "OrderStatusID");
            CreateIndex("dbo.RediMixOrders", "LocationID");
            CreateIndex("dbo.RediMixOrders", "RediMixDestinationID");
            CreateIndex("dbo.Vehicles", "TrailerTypeID");
            CreateIndex("dbo.Vehicles", "LocationID");
            CreateIndex("dbo.Vehicles", "VehicleTypeID");
            CreateIndex("dbo.HoursAssignments", "MaintainedVehicleID");
            CreateIndex("dbo.HoursAssignments", "JobID");
            CreateIndex("dbo.HoursAssignments", "EmployeeID");
            CreateIndex("dbo.HoursAssignments", "DrivenVehicleID");
            CreateIndex("dbo.HoursAssignments", "DepartmentID");
            CreateIndex("dbo.Employees", "VehicleID");
            CreateIndex("dbo.Employees", "EmployeeStatusID");
            CreateIndex("dbo.Employees", "LocationID");
            CreateIndex("dbo.Employees", "DepartmentID");
            CreateIndex("dbo.Loads", "RouteID");
            CreateIndex("dbo.Loads", "LoadStatusID");
            CreateIndex("dbo.Loads", "DepartmentID");
            CreateIndex("dbo.Loads", "VehicleID");
            CreateIndex("dbo.Loads", "TrailerID");
            CreateIndex("dbo.Loads", "RequiredTrailerTypeID");
            CreateIndex("dbo.Loads", "LoadTypeID");
            CreateIndex("dbo.Loads", "JobSiteLocationID");
            CreateIndex("dbo.Loads", "JobID");
            CreateIndex("dbo.Loads", "IndependentDriverID");
            CreateIndex("dbo.Loads", "DriverID");
            CreateIndex("dbo.Contacts", "ContactTypeID");
            CreateIndex("dbo.Jobs", "RouteID");
            CreateIndex("dbo.Jobs", "JobStatusID");
            CreateIndex("dbo.Jobs", "ProjectManagerID");
            CreateIndex("dbo.Jobs", "ErectorID");
            CreateIndex("dbo.Pours", "DefaultMarkTypeID");
            CreateIndex("dbo.Pours", "DefaultJobID");
            CreateIndex("dbo.Pours", "MixID");
            CreateIndex("dbo.Pours", "PourStatusID");
            CreateIndex("dbo.Pours", "BedID");
            CreateIndex("dbo.Beds", "DefaultMixID");
            AddForeignKey("dbo.AspNetUsers", "EmployeeID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.Pours", "BedID", "dbo.Beds", "BedID");
            AddForeignKey("dbo.Pours", "PourStatusID", "dbo.PourStatus", "PourStatusID");
            AddForeignKey("dbo.Pours", "MixID", "dbo.Mixes", "MixID");
            AddForeignKey("dbo.Pours", "DefaultJobID", "dbo.Jobs", "JobID");
            AddForeignKey("dbo.NonConformingReports", "JobID", "dbo.Jobs", "JobID");
            AddForeignKey("dbo.Jobs", "JobStatusID", "dbo.JobStatus", "JobStatusID");
            AddForeignKey("dbo.JobSiteLocations", "JobID", "dbo.Jobs", "JobID");
            AddForeignKey("dbo.Loads", "IndependentDriverID", "dbo.Contacts", "ContactID");
            AddForeignKey("dbo.RouteMileage", "RouteID", "dbo.Routes", "RouteID");
            AddForeignKey("dbo.RouteMileage", "StateID", "dbo.States", "StateID");
            AddForeignKey("dbo.Loads", "RouteID", "dbo.Routes", "RouteID");
            AddForeignKey("dbo.Jobs", "RouteID", "dbo.Routes", "RouteID");
            AddForeignKey("dbo.Loads", "LoadTypeID", "dbo.LoadTypes", "LoadTypeID");
            AddForeignKey("dbo.Loads", "LoadStatusID", "dbo.LoadStatus", "LoadStatusID");
            AddForeignKey("dbo.PourDetails", "MarkID", "dbo.Marks", "MarkID");
            AddForeignKey("dbo.PourDetails", "PourID", "dbo.Pours", "PourID");
            AddForeignKey("dbo.MarkTypes", "WeightFormulaTypeID", "dbo.WeightFormulaTypes", "WeightFormulaTypeID");
            AddForeignKey("dbo.Pours", "DefaultMarkTypeID", "dbo.MarkTypes", "MarkTypeID");
            AddForeignKey("dbo.MarkTypes", "MarkSizeTypeID", "dbo.MarkSizeTypes", "MarkSizeTypeID");
            AddForeignKey("dbo.Marks", "MarkTypeID", "dbo.MarkTypes", "MarkTypeID");
            AddForeignKey("dbo.LoadMarks", "MarkID", "dbo.Marks", "MarkID");
            AddForeignKey("dbo.Marks", "JobID", "dbo.Jobs", "JobID");
            AddForeignKey("dbo.LoadMarks", "LoadID", "dbo.Loads", "LoadID");
            AddForeignKey("dbo.Loads", "JobSiteLocationID", "dbo.JobSiteLocations", "JobSiteLocationID");
            AddForeignKey("dbo.Loads", "JobID", "dbo.Jobs", "JobID");
            AddForeignKey("dbo.Loads", "DepartmentID", "dbo.Departments", "DepartmentID");
            AddForeignKey("dbo.TimeClockPermissions", "EmployeeID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.TimeClockPermissions", "DepartmentID", "dbo.Departments", "DepartmentID");
            AddForeignKey("dbo.TimeClockEntries", "EmployeeID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.TimeCards", "EmployeeID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.RediMixLoads", "DriverID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.RediMixDriverAssignments", "DriverID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.NonConformingReports", "CompletedByID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.NonConformingReports", "PourID", "dbo.Pours", "PourID");
            AddForeignKey("dbo.ModulePermissions", "ModuleID", "dbo.Modules", "ModuleID");
            AddForeignKey("dbo.ModulePermissions", "EmployeeID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.Loads", "DriverID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.Jobs", "ProjectManagerID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.HoursAssignments", "EmployeeID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.Vehicles", "VehicleTypeID", "dbo.VehicleTypes", "VehicleTypeID");
            AddForeignKey("dbo.VehicleTypes", "MeterTypeID", "dbo.MeterTypes", "MeterTypeID");
            AddForeignKey("dbo.VehicleMaintenance", "VehicleID", "dbo.Vehicles", "VehicleID");
            AddForeignKey("dbo.Vehicles", "TrailerTypeID", "dbo.TrailerTypes", "TrailerTypeID");
            AddForeignKey("dbo.Loads", "RequiredTrailerTypeID", "dbo.TrailerTypes", "TrailerTypeID");
            AddForeignKey("dbo.TimeClockEntries", "DrivenVehicleID", "dbo.Vehicles", "VehicleID");
            AddForeignKey("dbo.RediMixDriverAssignments", "VehicleID", "dbo.Vehicles", "VehicleID");
            AddForeignKey("dbo.Vehicles", "LocationID", "dbo.Locations", "LocationID");
            AddForeignKey("dbo.RediMixOrders", "OrderStatusID", "dbo.RediMixOrderStatus", "OrderStatusID");
            AddForeignKey("dbo.RediMixLoads", "RediMixOrderID", "dbo.RediMixOrders", "RediMixOrderID", cascadeDelete: true);
            AddForeignKey("dbo.RediMixLoads", "VehicleID", "dbo.Vehicles", "VehicleID");
            AddForeignKey("dbo.RediMixLoads", "RediMixLoadTypeID", "dbo.RediMixLoadTypes", "RediMixLoadTypeID");
            AddForeignKey("dbo.RediMixOrders", "RediMixDestinationID", "dbo.RediMixDestinations", "RediMixDestinationID");
            AddForeignKey("dbo.RediMixOrders", "MixID", "dbo.Mixes", "MixID");
            AddForeignKey("dbo.RediMixOrders", "LocationID", "dbo.Locations", "LocationID");
            AddForeignKey("dbo.Employees", "LocationID", "dbo.Locations", "LocationID");
            AddForeignKey("dbo.Loads", "TrailerID", "dbo.Vehicles", "VehicleID");
            AddForeignKey("dbo.Loads", "VehicleID", "dbo.Vehicles", "VehicleID");
            AddForeignKey("dbo.HoursAssignments", "MaintainedVehicleID", "dbo.Vehicles", "VehicleID");
            AddForeignKey("dbo.HoursAssignments", "DrivenVehicleID", "dbo.Vehicles", "VehicleID");
            AddForeignKey("dbo.Employees", "VehicleID", "dbo.Vehicles", "VehicleID");
            AddForeignKey("dbo.HoursAssignments", "JobID", "dbo.Jobs", "JobID");
            AddForeignKey("dbo.HoursAssignments", "DepartmentID", "dbo.Departments", "DepartmentID");
            AddForeignKey("dbo.Employees", "EmployeeStatusID", "dbo.EmployeeStatuses", "EmployeeStatusID");
            AddForeignKey("dbo.Employees", "DepartmentID", "dbo.Departments", "DepartmentID");
            AddForeignKey("dbo.Jobs", "ErectorID", "dbo.Contacts", "ContactID");
            AddForeignKey("dbo.Contacts", "ContactTypeID", "dbo.ContactTypes", "ContactTypeID");
            AddForeignKey("dbo.Beds", "DefaultMixID", "dbo.Mixes", "MixID");
        }
    }
}
