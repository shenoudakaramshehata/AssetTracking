using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetProject.Migrations.Asset
{
    public partial class RegroupContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionLogs",
                columns: table => new
                {
                    ActionLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionLogTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionLogs", x => x.ActionLogId);
                });

            migrationBuilder.CreateTable(
                name: "ActionTypes",
                columns: table => new
                {
                    ActionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionTypeTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTypes", x => x.ActionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "assetBrokens",
                columns: table => new
                {
                    AssetBrokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateBroken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetBrokens", x => x.AssetBrokenId);
                });

            migrationBuilder.CreateTable(
                name: "AssetLosts",
                columns: table => new
                {
                    AssetLostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateLost = table.Column<DateTime>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetLosts", x => x.AssetLostId);
                });

            migrationBuilder.CreateTable(
                name: "AssetMaintainanceFrequencies",
                columns: table => new
                {
                    AssetMaintainanceFrequencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetMaintainanceFrequencyTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetMaintainanceFrequencies", x => x.AssetMaintainanceFrequencyId);
                });

            migrationBuilder.CreateTable(
                name: "AssetMovementDirections",
                columns: table => new
                {
                    AssetMovementDirectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetMovementDirectionTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetMovementDirections", x => x.AssetMovementDirectionId);
                });

            migrationBuilder.CreateTable(
                name: "AssetStatuses",
                columns: table => new
                {
                    AssetStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetStatusTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetStatuses", x => x.AssetStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryTIAR = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "DepreciationMethods",
                columns: table => new
                {
                    DepreciationMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepreciationMethodTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepreciationMethods", x => x.DepreciationMethodId);
                });

            migrationBuilder.CreateTable(
                name: "DisposeAssets",
                columns: table => new
                {
                    DisposeAssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateDisposed = table.Column<DateTime>(type: "date", nullable: false),
                    DisposeTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisposeAssets", x => x.DisposeAssetId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Insurances",
                columns: table => new
                {
                    InsuranceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolicyNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    Deductible = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Permium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insurances", x => x.InsuranceId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationParentId = table.Column<int>(type: "int", nullable: true),
                    LocationTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_Locations_LocationParentId",
                        column: x => x.LocationParentId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintainanceStatuses",
                columns: table => new
                {
                    MaintainanceStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintainanceStatusTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintainanceStatuses", x => x.MaintainanceStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Months",
                columns: table => new
                {
                    MonthId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Months", x => x.MonthId);
                });

            migrationBuilder.CreateTable(
                name: "sellAssets",
                columns: table => new
                {
                    SellAssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoldTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaleAmount = table.Column<double>(type: "float", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sellAssets", x => x.SellAssetId);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tele = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                });

            migrationBuilder.CreateTable(
                name: "Technicians",
                columns: table => new
                {
                    TechnicianId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technicians", x => x.TechnicianId);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonPhone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorId);
                });

            migrationBuilder.CreateTable(
                name: "WeekDays",
                columns: table => new
                {
                    WeekDayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekDayTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekDays", x => x.WeekDayId);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.TypeId);
                    table.ForeignKey(
                        name: "FK_Types_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCategoryTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubCategoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                    table.ForeignKey(
                        name: "FK_Tenants_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetLeasings",
                columns: table => new
                {
                    AssetLeasingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetLeasings", x => x.AssetLeasingId);
                    table.ForeignKey(
                        name: "FK_AssetLeasings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetMovements",
                columns: table => new
                {
                    AssetMovementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpolyeeID = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    ActionTypeId = table.Column<int>(type: "int", nullable: true),
                    AssetMovementDirectionId = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetMovements", x => x.AssetMovementId);
                    table.ForeignKey(
                        name: "FK_AssetMovements_ActionTypes_ActionTypeId",
                        column: x => x.ActionTypeId,
                        principalTable: "ActionTypes",
                        principalColumn: "ActionTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetMovements_AssetMovementDirections_AssetMovementDirectionId",
                        column: x => x.AssetMovementDirectionId,
                        principalTable: "AssetMovementDirections",
                        principalColumn: "AssetMovementDirectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetMovements_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetMovements_Employees_EmpolyeeID",
                        column: x => x.EmpolyeeID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetMovements_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetMovements_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetRepairs",
                columns: table => new
                {
                    AssetRepairId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepairCost = table.Column<double>(type: "float", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TechnicianId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRepairs", x => x.AssetRepairId);
                    table.ForeignKey(
                        name: "FK_AssetRepairs_Technicians_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Technicians",
                        principalColumn: "TechnicianId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_Contracts_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseSerial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Purchasedate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true),
                    VendorId = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: true),
                    Net = table.Column<double>(type: "float", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_Purchases_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchases_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetTagId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetCost = table.Column<double>(type: "float", nullable: false),
                    AssetSerialNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetPurchaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepreciableAsset = table.Column<bool>(type: "bit", nullable: false),
                    DepreciableCost = table.Column<double>(type: "float", nullable: true),
                    SalvageValue = table.Column<double>(type: "float", nullable: true),
                    AssetLife = table.Column<int>(type: "int", nullable: true),
                    DateAcquired = table.Column<DateTime>(type: "date", nullable: true),
                    DepreciationMethodId = table.Column<int>(type: "int", nullable: true),
                    AssetStatusId = table.Column<int>(type: "int", nullable: true),
                    VendorId = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetId);
                    table.ForeignKey(
                        name: "FK_Assets_AssetStatuses_AssetStatusId",
                        column: x => x.AssetStatusId,
                        principalTable: "AssetStatuses",
                        principalColumn: "AssetStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_DepreciationMethods_DepreciationMethodId",
                        column: x => x.DepreciationMethodId,
                        principalTable: "DepreciationMethods",
                        principalColumn: "DepreciationMethodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assets_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseAssets",
                columns: table => new
                {
                    PurchaseAssetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: true),
                    Net = table.Column<double>(type: "float", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseAssets", x => x.PurchaseAssetId);
                    table.ForeignKey(
                        name: "FK_PurchaseAssets_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseAssets_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "PurchaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetBrokenDetails",
                columns: table => new
                {
                    AssetBrokenDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetBrokenId = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetBrokenDetails", x => x.AssetBrokenDetailsId);
                    table.ForeignKey(
                        name: "FK_AssetBrokenDetails_assetBrokens_AssetBrokenId",
                        column: x => x.AssetBrokenId,
                        principalTable: "assetBrokens",
                        principalColumn: "AssetBrokenId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetBrokenDetails_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetContracts",
                columns: table => new
                {
                    AssetContractID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetContracts", x => x.AssetContractID);
                    table.ForeignKey(
                        name: "FK_AssetContracts_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetContracts_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetDisposeDetails",
                columns: table => new
                {
                    AssetDisposeDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisposeAssetId = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetDisposeDetails", x => x.AssetDisposeDetailsId);
                    table.ForeignKey(
                        name: "FK_AssetDisposeDetails_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetDisposeDetails_DisposeAssets_DisposeAssetId",
                        column: x => x.DisposeAssetId,
                        principalTable: "DisposeAssets",
                        principalColumn: "DisposeAssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assetDocuments",
                columns: table => new
                {
                    AssetDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetDocuments", x => x.AssetDocumentId);
                    table.ForeignKey(
                        name: "FK_assetDocuments_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetLeasingDetails",
                columns: table => new
                {
                    AssetLeasingDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetLeasingId = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetLeasingDetails", x => x.AssetLeasingDetailsId);
                    table.ForeignKey(
                        name: "FK_AssetLeasingDetails_AssetLeasings_AssetLeasingId",
                        column: x => x.AssetLeasingId,
                        principalTable: "AssetLeasings",
                        principalColumn: "AssetLeasingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetLeasingDetails_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetLogs",
                columns: table => new
                {
                    AssetLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    ActionLogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetLogs", x => x.AssetLogId);
                    table.ForeignKey(
                        name: "FK_AssetLogs_ActionLogs_ActionLogId",
                        column: x => x.ActionLogId,
                        principalTable: "ActionLogs",
                        principalColumn: "ActionLogId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetLogs_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetLostDetails",
                columns: table => new
                {
                    AssetLostDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetLostId = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetLostDetails", x => x.AssetLostDetailsId);
                    table.ForeignKey(
                        name: "FK_AssetLostDetails_AssetLosts_AssetLostId",
                        column: x => x.AssetLostId,
                        principalTable: "AssetLosts",
                        principalColumn: "AssetLostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetLostDetails_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetMaintainances",
                columns: table => new
                {
                    AssetMaintainanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetMaintainanceTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetMaintainanceDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetMaintainanceDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaintainanceStatusId = table.Column<int>(type: "int", nullable: false),
                    AssetMaintainanceDateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssetMaintainanceRepairesCost = table.Column<double>(type: "float", nullable: false),
                    AssetMaintainanceRepeating = table.Column<bool>(type: "bit", nullable: false),
                    AssetMaintainanceFrequencyId = table.Column<int>(type: "int", nullable: true),
                    TechnicianId = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    WeeklyPeriod = table.Column<int>(type: "int", nullable: true),
                    WeekDayId = table.Column<int>(type: "int", nullable: true),
                    MonthlyPeriod = table.Column<int>(type: "int", nullable: true),
                    MonthlyDay = table.Column<int>(type: "int", nullable: true),
                    MonthId = table.Column<int>(type: "int", nullable: true),
                    YearlyDay = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetMaintainances", x => x.AssetMaintainanceId);
                    table.ForeignKey(
                        name: "FK_AssetMaintainances_AssetMaintainanceFrequencies_AssetMaintainanceFrequencyId",
                        column: x => x.AssetMaintainanceFrequencyId,
                        principalTable: "AssetMaintainanceFrequencies",
                        principalColumn: "AssetMaintainanceFrequencyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetMaintainances_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetMaintainances_MaintainanceStatuses_MaintainanceStatusId",
                        column: x => x.MaintainanceStatusId,
                        principalTable: "MaintainanceStatuses",
                        principalColumn: "MaintainanceStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetMaintainances_Months_MonthId",
                        column: x => x.MonthId,
                        principalTable: "Months",
                        principalColumn: "MonthId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetMaintainances_Technicians_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Technicians",
                        principalColumn: "TechnicianId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetMaintainances_WeekDays_WeekDayId",
                        column: x => x.WeekDayId,
                        principalTable: "WeekDays",
                        principalColumn: "WeekDayId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetMovementDetails",
                columns: table => new
                {
                    AssetMovementDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetMovementId = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetMovementDetails", x => x.AssetMovementDetailsId);
                    table.ForeignKey(
                        name: "FK_AssetMovementDetails_AssetMovements_AssetMovementId",
                        column: x => x.AssetMovementId,
                        principalTable: "AssetMovements",
                        principalColumn: "AssetMovementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetMovementDetails_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetPhotos",
                columns: table => new
                {
                    AssetPhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetPhotos", x => x.AssetPhotoId);
                    table.ForeignKey(
                        name: "FK_AssetPhotos_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetRepairDetails",
                columns: table => new
                {
                    AssetRepairDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetRepairId = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRepairDetails", x => x.AssetRepairDetailsId);
                    table.ForeignKey(
                        name: "FK_AssetRepairDetails_AssetRepairs_AssetRepairId",
                        column: x => x.AssetRepairId,
                        principalTable: "AssetRepairs",
                        principalColumn: "AssetRepairId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetRepairDetails_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetSellDetails",
                columns: table => new
                {
                    AssetSellDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellAssetId = table.Column<int>(type: "int", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetSellDetails", x => x.AssetSellDetailsId);
                    table.ForeignKey(
                        name: "FK_AssetSellDetails_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetSellDetails_sellAssets_SellAssetId",
                        column: x => x.SellAssetId,
                        principalTable: "sellAssets",
                        principalColumn: "SellAssetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetsInsurances",
                columns: table => new
                {
                    AssetsInsuranceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    InsuranceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsInsurances", x => x.AssetsInsuranceId);
                    table.ForeignKey(
                        name: "FK_AssetsInsurances_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "AssetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetsInsurances_Insurances_InsuranceId",
                        column: x => x.InsuranceId,
                        principalTable: "Insurances",
                        principalColumn: "InsuranceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ActionLogs",
                columns: new[] { "ActionLogId", "ActionLogTitle" },
                values: new object[,]
                {
                    { 1, "Asset Purchase" },
                    { 18, "Asset Maintainance" },
                    { 17, "CheckOut" },
                    { 16, "CheckIn" },
                    { 14, "Repair Asset" },
                    { 13, "Repair Asset" },
                    { 12, "Broken Asset" },
                    { 11, "Dispose Asset " },
                    { 10, "Sell Asset" },
                    { 15, "Asset Leasing" },
                    { 8, "Dettached Asset Document" },
                    { 7, "Dettached Asset Insurance" },
                    { 6, "Dettached Asset Contract" },
                    { 5, "Add New Asset Photo" },
                    { 9, "Delete Asset Photo" },
                    { 4, "Creation Link Document" },
                    { 3, "Creation Link Insurance" },
                    { 2, "Creation Link Contract" }
                });

            migrationBuilder.InsertData(
                table: "ActionTypes",
                columns: new[] { "ActionTypeId", "ActionTypeTitle" },
                values: new object[,]
                {
                    { 2, "To Department" },
                    { 1, "To Employee" }
                });

            migrationBuilder.InsertData(
                table: "AssetMaintainanceFrequencies",
                columns: new[] { "AssetMaintainanceFrequencyId", "AssetMaintainanceFrequencyTitle" },
                values: new object[,]
                {
                    { 1, "Daily" },
                    { 2, "Weekly" },
                    { 3, "Monthly" },
                    { 4, "Yearly" }
                });

            migrationBuilder.InsertData(
                table: "AssetMovementDirections",
                columns: new[] { "AssetMovementDirectionId", "AssetMovementDirectionTitle" },
                values: new object[,]
                {
                    { 1, "CheckOut" },
                    { 2, "CheckIn" }
                });

            migrationBuilder.InsertData(
                table: "AssetStatuses",
                columns: new[] { "AssetStatusId", "AssetStatusTitle" },
                values: new object[,]
                {
                    { 7, "Sold" },
                    { 9, "InMaimtainance" },
                    { 8, "Broken" },
                    { 6, "Leased" },
                    { 4, "Lost" },
                    { 3, "InRepair" },
                    { 2, "CheckedOut" },
                    { 1, "CheckedIn(Avaliable)" },
                    { 5, "Disposed" }
                });

            migrationBuilder.InsertData(
                table: "DepreciationMethods",
                columns: new[] { "DepreciationMethodId", "DepreciationMethodTitle" },
                values: new object[,]
                {
                    { 1, "Straight Line" },
                    { 2, "Declining Balance" },
                    { 3, "Double Declining Balance" },
                    { 4, "150% Declining Balance" },
                    { 5, "Sum of the Years' Digits" }
                });

            migrationBuilder.InsertData(
                table: "Months",
                columns: new[] { "MonthId", "MonthTitle" },
                values: new object[,]
                {
                    { 5, "May" },
                    { 12, "December" }
                });

            migrationBuilder.InsertData(
                table: "Months",
                columns: new[] { "MonthId", "MonthTitle" },
                values: new object[,]
                {
                    { 11, "November" },
                    { 10, "October" },
                    { 9, "September" },
                    { 8, "August" },
                    { 3, "March" },
                    { 6, "June" },
                    { 4, "April" },
                    { 2, "February" },
                    { 1, "January" },
                    { 7, "July" }
                });

            migrationBuilder.InsertData(
                table: "WeekDays",
                columns: new[] { "WeekDayId", "WeekDayTitle" },
                values: new object[,]
                {
                    { 5, "Wednesday" },
                    { 4, "Tuesday" },
                    { 6, "Thursday" },
                    { 2, "Sunday" },
                    { 1, "Saturday" },
                    { 3, "Monday" },
                    { 7, "Friday" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetBrokenDetails_AssetBrokenId",
                table: "AssetBrokenDetails",
                column: "AssetBrokenId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetBrokenDetails_AssetId",
                table: "AssetBrokenDetails",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetContracts_AssetId",
                table: "AssetContracts",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetContracts_ContractId",
                table: "AssetContracts",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposeDetails_AssetId",
                table: "AssetDisposeDetails",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetDisposeDetails_DisposeAssetId",
                table: "AssetDisposeDetails",
                column: "DisposeAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_assetDocuments_AssetId",
                table: "assetDocuments",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetLeasingDetails_AssetId",
                table: "AssetLeasingDetails",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetLeasingDetails_AssetLeasingId",
                table: "AssetLeasingDetails",
                column: "AssetLeasingId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetLeasings_CustomerId",
                table: "AssetLeasings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetLogs_ActionLogId",
                table: "AssetLogs",
                column: "ActionLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetLogs_AssetId",
                table: "AssetLogs",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetLostDetails_AssetId",
                table: "AssetLostDetails",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetLostDetails_AssetLostId",
                table: "AssetLostDetails",
                column: "AssetLostId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintainances_AssetId",
                table: "AssetMaintainances",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintainances_AssetMaintainanceFrequencyId",
                table: "AssetMaintainances",
                column: "AssetMaintainanceFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintainances_MaintainanceStatusId",
                table: "AssetMaintainances",
                column: "MaintainanceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintainances_MonthId",
                table: "AssetMaintainances",
                column: "MonthId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintainances_TechnicianId",
                table: "AssetMaintainances",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMaintainances_WeekDayId",
                table: "AssetMaintainances",
                column: "WeekDayId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMovementDetails_AssetId",
                table: "AssetMovementDetails",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMovementDetails_AssetMovementId",
                table: "AssetMovementDetails",
                column: "AssetMovementId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMovements_ActionTypeId",
                table: "AssetMovements",
                column: "ActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMovements_AssetMovementDirectionId",
                table: "AssetMovements",
                column: "AssetMovementDirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMovements_DepartmentId",
                table: "AssetMovements",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMovements_EmpolyeeID",
                table: "AssetMovements",
                column: "EmpolyeeID");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMovements_LocationId",
                table: "AssetMovements",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMovements_StoreId",
                table: "AssetMovements",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetPhotos_AssetId",
                table: "AssetPhotos",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRepairDetails_AssetId",
                table: "AssetRepairDetails",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRepairDetails_AssetRepairId",
                table: "AssetRepairDetails",
                column: "AssetRepairId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRepairs_TechnicianId",
                table: "AssetRepairs",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetStatusId",
                table: "Assets",
                column: "AssetStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_DepreciationMethodId",
                table: "Assets",
                column: "DepreciationMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ItemId",
                table: "Assets",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_StoreId",
                table: "Assets",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_VendorId",
                table: "Assets",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSellDetails_AssetId",
                table: "AssetSellDetails",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetSellDetails_SellAssetId",
                table: "AssetSellDetails",
                column: "SellAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetsInsurances_AssetId",
                table: "AssetsInsurances",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetsInsurances_InsuranceId",
                table: "AssetsInsurances",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_VendorId",
                table: "Contracts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BrandId",
                table: "Items",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_TypeId",
                table: "Items",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationParentId",
                table: "Locations",
                column: "LocationParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAssets_ItemId",
                table: "PurchaseAssets",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseAssets_PurchaseId",
                table: "PurchaseAssets",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_StoreId",
                table: "Purchases",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_VendorId",
                table: "Purchases",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CountryId",
                table: "Tenants",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Types_BrandId",
                table: "Types",
                column: "BrandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetBrokenDetails");

            migrationBuilder.DropTable(
                name: "AssetContracts");

            migrationBuilder.DropTable(
                name: "AssetDisposeDetails");

            migrationBuilder.DropTable(
                name: "assetDocuments");

            migrationBuilder.DropTable(
                name: "AssetLeasingDetails");

            migrationBuilder.DropTable(
                name: "AssetLogs");

            migrationBuilder.DropTable(
                name: "AssetLostDetails");

            migrationBuilder.DropTable(
                name: "AssetMaintainances");

            migrationBuilder.DropTable(
                name: "AssetMovementDetails");

            migrationBuilder.DropTable(
                name: "AssetPhotos");

            migrationBuilder.DropTable(
                name: "AssetRepairDetails");

            migrationBuilder.DropTable(
                name: "AssetSellDetails");

            migrationBuilder.DropTable(
                name: "AssetsInsurances");

            migrationBuilder.DropTable(
                name: "PurchaseAssets");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "assetBrokens");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "DisposeAssets");

            migrationBuilder.DropTable(
                name: "AssetLeasings");

            migrationBuilder.DropTable(
                name: "ActionLogs");

            migrationBuilder.DropTable(
                name: "AssetLosts");

            migrationBuilder.DropTable(
                name: "AssetMaintainanceFrequencies");

            migrationBuilder.DropTable(
                name: "MaintainanceStatuses");

            migrationBuilder.DropTable(
                name: "Months");

            migrationBuilder.DropTable(
                name: "WeekDays");

            migrationBuilder.DropTable(
                name: "AssetMovements");

            migrationBuilder.DropTable(
                name: "AssetRepairs");

            migrationBuilder.DropTable(
                name: "sellAssets");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Insurances");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ActionTypes");

            migrationBuilder.DropTable(
                name: "AssetMovementDirections");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Technicians");

            migrationBuilder.DropTable(
                name: "AssetStatuses");

            migrationBuilder.DropTable(
                name: "DepreciationMethods");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
