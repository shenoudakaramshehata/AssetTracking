using AssetProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AssetProject.Data
{
    public class AssetContext : DbContext
    {
        public AssetContext(DbContextOptions<AssetContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            //foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            //}
            modelBuilder.Entity<Asset>().HasIndex(u => u.AssetSerialNo).IsUnique();
            modelBuilder.Entity<Asset>().HasIndex(u => u.AssetTagId).IsUnique();


            //DepreciationMethod
            modelBuilder.Entity<DepreciationMethod>().HasData(new DepreciationMethod {DepreciationMethodId = 1,DepreciationMethodTitle= "Straight Line"});
            modelBuilder.Entity<DepreciationMethod>().HasData(new DepreciationMethod { DepreciationMethodId = 2, DepreciationMethodTitle = "Declining Balance" });
            modelBuilder.Entity<DepreciationMethod>().HasData(new DepreciationMethod { DepreciationMethodId = 3, DepreciationMethodTitle = "Double Declining Balance" });
            modelBuilder.Entity<DepreciationMethod>().HasData(new DepreciationMethod { DepreciationMethodId = 4, DepreciationMethodTitle = "150% Declining Balance" });
            modelBuilder.Entity<DepreciationMethod>().HasData(new DepreciationMethod { DepreciationMethodId = 5, DepreciationMethodTitle = "Sum of the Years' Digits" });

            //ActionType
            modelBuilder.Entity<ActionType>().HasData(new ActionType { ActionTypeId = 1, ActionTypeTitle = "Employee" });
            modelBuilder.Entity<ActionType>().HasData(new ActionType { ActionTypeId = 2, ActionTypeTitle = "Department" });

            //Maintainance Frequency
            modelBuilder.Entity<AssetMaintainanceFrequency>().HasData(new AssetMaintainanceFrequency { AssetMaintainanceFrequencyId = 1, AssetMaintainanceFrequencyTitle= "Daily" });
            modelBuilder.Entity<AssetMaintainanceFrequency>().HasData(new AssetMaintainanceFrequency { AssetMaintainanceFrequencyId = 2, AssetMaintainanceFrequencyTitle = "Weekly" });
            modelBuilder.Entity<AssetMaintainanceFrequency>().HasData(new AssetMaintainanceFrequency { AssetMaintainanceFrequencyId = 3, AssetMaintainanceFrequencyTitle = "Monthly" });
            modelBuilder.Entity<AssetMaintainanceFrequency>().HasData(new AssetMaintainanceFrequency { AssetMaintainanceFrequencyId = 4, AssetMaintainanceFrequencyTitle = "Yearly" });
            //Maintainace Status 
            modelBuilder.Entity<MaintainanceStatus>().HasData(new MaintainanceStatus { MaintainanceStatusId = 1, MaintainanceStatusTitle = "Scheduled" });
            modelBuilder.Entity<MaintainanceStatus>().HasData(new MaintainanceStatus { MaintainanceStatusId = 2, MaintainanceStatusTitle = "In Progress" });
            modelBuilder.Entity<MaintainanceStatus>().HasData(new MaintainanceStatus { MaintainanceStatusId = 3, MaintainanceStatusTitle = "On Hold" });
            modelBuilder.Entity<MaintainanceStatus>().HasData(new MaintainanceStatus { MaintainanceStatusId = 4, MaintainanceStatusTitle = "Cancelled" });
            modelBuilder.Entity<MaintainanceStatus>().HasData(new MaintainanceStatus { MaintainanceStatusId = 5, MaintainanceStatusTitle = "Completed" });
            //Asset Movement Direction 
            modelBuilder.Entity<AssetMovementDirection>().HasData(new AssetMovementDirection { AssetMovementDirectionId = 1, AssetMovementDirectionTitle = "CheckOut" });
            modelBuilder.Entity<AssetMovementDirection>().HasData(new AssetMovementDirection { AssetMovementDirectionId = 2, AssetMovementDirectionTitle = "CheckIn" });

            //Asset Statuses
            modelBuilder.Entity<AssetStatus>().HasData(new AssetStatus { AssetStatusId = 1, AssetStatusTitle = "CheckedIn(Avaliable)" });
            modelBuilder.Entity<AssetStatus>().HasData(new AssetStatus { AssetStatusId = 2, AssetStatusTitle = "CheckedOut" });
            modelBuilder.Entity<AssetStatus>().HasData(new AssetStatus { AssetStatusId = 3, AssetStatusTitle = "InRepair" });
            modelBuilder.Entity<AssetStatus>().HasData(new AssetStatus { AssetStatusId = 4, AssetStatusTitle = "Lost" });
            modelBuilder.Entity<AssetStatus>().HasData(new AssetStatus { AssetStatusId = 5, AssetStatusTitle = "Disposed" });
            modelBuilder.Entity<AssetStatus>().HasData(new AssetStatus { AssetStatusId = 6, AssetStatusTitle = "Leased" });
            modelBuilder.Entity<AssetStatus>().HasData(new AssetStatus { AssetStatusId = 7, AssetStatusTitle = "Sold" });
            modelBuilder.Entity<AssetStatus>().HasData(new AssetStatus { AssetStatusId = 8, AssetStatusTitle = "Broken" });
            modelBuilder.Entity<AssetStatus>().HasData(new AssetStatus { AssetStatusId = 9, AssetStatusTitle = "InMaintainance" });

            //Months
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 1,MonthTitle = "January" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 2, MonthTitle = "February" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 3, MonthTitle = "March" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 4, MonthTitle = "April" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 5, MonthTitle = "May" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 6, MonthTitle = "June" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 7, MonthTitle = "July" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 8, MonthTitle = "August" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 9, MonthTitle = "September" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 10, MonthTitle = "October" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 11, MonthTitle = "November" });
            modelBuilder.Entity<Month>().HasData(new Month { MonthId = 12, MonthTitle = "December" });

            //Week Days
            modelBuilder.Entity<WeekDay>().HasData(new WeekDay { WeekDayId = 1, WeekDayTitle = "Saturday" });
            modelBuilder.Entity<WeekDay>().HasData(new WeekDay { WeekDayId = 2, WeekDayTitle = "Sunday" });
            modelBuilder.Entity<WeekDay>().HasData(new WeekDay { WeekDayId = 3, WeekDayTitle = "Monday" });
            modelBuilder.Entity<WeekDay>().HasData(new WeekDay { WeekDayId = 4, WeekDayTitle = "Tuesday" });
            modelBuilder.Entity<WeekDay>().HasData(new WeekDay { WeekDayId = 5, WeekDayTitle = "Wednesday" });
            modelBuilder.Entity<WeekDay>().HasData(new WeekDay { WeekDayId = 6, WeekDayTitle = "Thursday" });
            modelBuilder.Entity<WeekDay>().HasData(new WeekDay { WeekDayId = 7, WeekDayTitle = "Friday" });


            //Action Logs
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 1, ActionLogTitle = "Asset Purchase" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 2, ActionLogTitle = "Creation Link Contract" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 3, ActionLogTitle = "Creation Link Insurance" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 4, ActionLogTitle = "Creation Link Document" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 5, ActionLogTitle = "Add New Asset Photo" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 6, ActionLogTitle = "Dettached Asset Contract" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 7, ActionLogTitle = "Dettached Asset Insurance" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 8, ActionLogTitle = "Dettached Asset Document" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 9, ActionLogTitle = "Delete Asset Photo" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 10, ActionLogTitle = "Sell Asset" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 11, ActionLogTitle = "Dispose Asset " });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 12, ActionLogTitle = "Broken Asset" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 13, ActionLogTitle = "Repair Asset" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 14, ActionLogTitle = "Asset Lost" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 15, ActionLogTitle = "Asset Leasing" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 16, ActionLogTitle = "CheckIn" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 17, ActionLogTitle = "CheckOut" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 18, ActionLogTitle = "Asset Maintainance" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 19, ActionLogTitle = "Asset Edited" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 20, ActionLogTitle = "Add Asset Waranty" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 21, ActionLogTitle = "Deattach Asset Waranty" });
            modelBuilder.Entity<ActionLog>().HasData(new ActionLog { ActionLogId = 22, ActionLogTitle = "Edit Asset Maintainance" });

        }


        public DbSet<Department> Departments { set; get; }
        public DbSet<Country> Countries { set; get; }
        public DbSet<Location> Locations { set; get; }
        public DbSet<Tenant> Tenants { set; get; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PurchaseAsset> PurchaseAssets { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<DepreciationMethod> DepreciationMethods { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetPhotos> AssetPhotos { get; set; }
        public DbSet<AssetMovement> AssetMovements { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<AssetsInsurance> AssetsInsurances { get; set; }
        public DbSet<AssetContract> AssetContracts { get; set; }
        public DbSet<AssetDocument> assetDocuments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AssetLeasing> AssetLeasings { get; set; }
        public DbSet<SellAsset> sellAssets { get; set; }
        public DbSet<AssetBroken> assetBrokens { get; set; }

        public DbSet<AssetRepair> AssetRepairs { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<AssetLost> AssetLosts { get; set; }
        public DbSet<DisposeAsset> DisposeAssets { get; set; }

        public DbSet<AssetMaintainance> AssetMaintainances { get; set; }
        public DbSet<AssetMaintainanceFrequency> AssetMaintainanceFrequencies { get; set; }
        public DbSet<MaintainanceStatus> MaintainanceStatuses { get; set; }
        public DbSet<WeekDay> WeekDays { get; set; }
        public DbSet<Month> Months { get; set; }
        public DbSet<AssetMovementDirection> AssetMovementDirections { get; set; }
        public DbSet<AssetStatus> AssetStatuses { get; set; }
        public DbSet<AssetLog> AssetLogs { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
        public DbSet<AssetMovementDetails> AssetMovementDetails { get; set; }
        public DbSet<AssetSellDetails> AssetSellDetails { get; set; }
        public DbSet<AssetDisposeDetails> AssetDisposeDetails { get; set; }
        public DbSet<AssetRepairDetails> AssetRepairDetails { get; set; }
        public DbSet<AssetLostDetails> AssetLostDetails { get; set; }
        public DbSet<AssetLeasingDetails> AssetLeasingDetails { get; set; }
        public DbSet<AssetBrokenDetails> AssetBrokenDetails { get; set; }
        public DbSet<AssetWarranty> AssetWarranties { get; set; }

    }
}
