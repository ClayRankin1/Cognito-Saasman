using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cognito.DataAccess.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Detail");

            migrationBuilder.EnsureSchema(
                name: "Lookup");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Websites",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Url = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetailTypeSourceTypes",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailTypeSourceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentStatuses",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LicenseTypes",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTypes",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusTypes",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(maxLength: 64, nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskStatuses",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeTypes",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    RefreshToken = table.Column<string>(maxLength: 64, nullable: true),
                    RefreshTokenExpiration = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetailTypes",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 64, nullable: false),
                    DetailTypeSourceTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailTypes_DetailTypeSourceTypes_DetailTypeSourceTypeId",
                        column: x => x.DetailTypeSourceTypeId,
                        principalSchema: "Lookup",
                        principalTable: "DetailTypeSourceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Street = table.Column<string>(maxLength: 250, nullable: false),
                    City = table.Column<string>(maxLength: 100, nullable: false),
                    Zip = table.Column<string>(maxLength: 10, nullable: false),
                    StateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_States_StateId",
                        column: x => x.StateId,
                        principalSchema: "Lookup",
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Times",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(maxLength: 64, nullable: false),
                    TimeTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Times", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Times_TimeTypes_TimeTypeId",
                        column: x => x.TimeTypeId,
                        principalSchema: "Lookup",
                        principalTable: "TimeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "User",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    UpdatedByUserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    LicenseTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licenses_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Licenses_LicenseTypes_LicenseTypeId",
                        column: x => x.LicenseTypeId,
                        principalSchema: "Lookup",
                        principalTable: "LicenseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Licenses_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "Detail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    UpdatedByUserId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 255, nullable: false),
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    DocumentStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentStatuses_DocumentStatusId",
                        column: x => x.DocumentStatusId,
                        principalSchema: "Lookup",
                        principalTable: "DocumentStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "User",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.UserId, x.LoginProvider });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "User",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    UpdatedByUserId = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 200, nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    ContactName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    Phone = table.Column<string>(maxLength: 25, nullable: false),
                    TenantName = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenants_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tenants_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tenants_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    UpdatedByUserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    AdminUserId = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Domains_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Domains_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Domains_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DomainLicenses",
                columns: table => new
                {
                    DomainId = table.Column<int>(nullable: false),
                    LicenseId = table.Column<int>(nullable: false),
                    Discount = table.Column<decimal>(nullable: true),
                    Licenses = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainLicenses", x => new { x.DomainId, x.LicenseId });
                    table.ForeignKey(
                        name: "FK_DomainLicenses_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DomainLicenses_Licenses_LicenseId",
                        column: x => x.LicenseId,
                        principalTable: "Licenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    UpdatedByUserId = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(maxLength: 200, nullable: true),
                    Name = table.Column<string>(maxLength: 25, nullable: true),
                    ProjectNo = table.Column<string>(maxLength: 30, nullable: true),
                    ClientNo = table.Column<string>(maxLength: 30, nullable: true),
                    ProjectTypeId = table.Column<int>(nullable: false),
                    IsPrivate = table.Column<bool>(nullable: false),
                    ArchivedOn = table.Column<DateTime>(nullable: true),
                    IsArchived = table.Column<bool>(nullable: false),
                    DomainId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectTypes_ProjectTypeId",
                        column: x => x.ProjectTypeId,
                        principalSchema: "Lookup",
                        principalTable: "ProjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDomains",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    DomainId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDomains", x => new { x.UserId, x.DomainId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserDomains_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDomains_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "User",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDomains_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                schema: "Detail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    UpdatedByUserId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    FormalName = table.Column<string>(maxLength: 100, nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Entity = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 250, nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    Region = table.Column<string>(maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 20, nullable: true),
                    Country = table.Column<string>(maxLength: 50, nullable: true),
                    Phone = table.Column<string>(maxLength: 25, nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    KeyName = table.Column<string>(maxLength: 10, nullable: true),
                    ContactType = table.Column<string>(maxLength: 50, nullable: true),
                    DomainId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contacts_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contacts_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypes",
                schema: "Lookup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(maxLength: 64, nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    IsEvent = table.Column<bool>(nullable: false),
                    ArchivedOn = table.Column<DateTime>(nullable: true),
                    DomainId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTypes_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    UpdatedByUserId = table.Column<int>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Points_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Points_Points_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Points_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Points_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDocuments",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false),
                    DocumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDocuments", x => new { x.ProjectId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "Detail",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProjects",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjects", x => new { x.UserId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_UserProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProjects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    UpdatedByUserId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    NextDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Accrued = table.Column<decimal>(type: "decimal(4, 2)", nullable: true),
                    TaskStatusId = table.Column<int>(nullable: false),
                    IsEvent = table.Column<bool>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    TimeId = table.Column<int>(nullable: false),
                    TaskTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_TaskStatuses_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalSchema: "Lookup",
                        principalTable: "TaskStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalSchema: "Lookup",
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Times_TimeId",
                        column: x => x.TimeId,
                        principalSchema: "Lookup",
                        principalTable: "Times",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccruedTimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    TaskId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccruedTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccruedTimes_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccruedTimes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subtasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TaskId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subtasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subtasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskContacts",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false),
                    ContactId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskContacts", x => new { x.TaskId, x.ContactId });
                    table.ForeignKey(
                        name: "FK_TaskContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalSchema: "Detail",
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskContacts_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskWebsites",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false),
                    WebsiteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskWebsites", x => new { x.TaskId, x.WebsiteId });
                    table.ForeignKey(
                        name: "FK_TaskWebsites_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskWebsites_Websites_WebsiteId",
                        column: x => x.WebsiteId,
                        principalSchema: "dbo",
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                schema: "Detail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    UpdatedByUserId = table.Column<int>(nullable: false),
                    Body = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(maxLength: 250, nullable: true),
                    Source = table.Column<string>(maxLength: 250, nullable: true),
                    SourceId = table.Column<int>(nullable: true),
                    BeginPage = table.Column<int>(nullable: true),
                    BeginLine = table.Column<int>(nullable: true),
                    EndPage = table.Column<int>(nullable: true),
                    EndLine = table.Column<int>(nullable: true),
                    Chrono = table.Column<DateTime>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: true),
                    DetailTypeId = table.Column<int>(nullable: false),
                    TaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Details_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Details_DetailTypes_DetailTypeId",
                        column: x => x.DetailTypeId,
                        principalSchema: "Lookup",
                        principalTable: "DetailTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Details_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Details_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalSchema: "User",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PointDetails",
                columns: table => new
                {
                    PointId = table.Column<int>(nullable: false),
                    DetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointDetails", x => new { x.DetailId, x.PointId });
                    table.ForeignKey(
                        name: "FK_PointDetails_Details_DetailId",
                        column: x => x.DetailId,
                        principalSchema: "Detail",
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PointDetails_Points_PointId",
                        column: x => x.PointId,
                        principalTable: "Points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "DetailTypeSourceTypes",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "System" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "DocumentStatuses",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "Completed" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "LicenseTypes",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Basic" },
                    { 2, "TimeKeeper" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "ProjectTypes",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Personal" },
                    { 2, "Billable" },
                    { 3, "Unbillable" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "States",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 37, "OH" },
                    { 36, "OK" },
                    { 35, "NY" },
                    { 34, "NV" },
                    { 29, "ND" },
                    { 32, "NJ" },
                    { 31, "NH" },
                    { 30, "NE" },
                    { 38, "OR" },
                    { 33, "NM" },
                    { 39, "PA" },
                    { 44, "TX" },
                    { 41, "SC" },
                    { 42, "SD" },
                    { 43, "TN" },
                    { 28, "NC" },
                    { 45, "UT" },
                    { 46, "VA" },
                    { 47, "VT" },
                    { 48, "WA" },
                    { 49, "WI" },
                    { 50, "WV" },
                    { 51, "WY" },
                    { 40, "RI" },
                    { 27, "MT" },
                    { 26, "MS" },
                    { 25, "MO" },
                    { 1, "AL" },
                    { 2, "AK" },
                    { 3, "AR" },
                    { 4, "AZ" },
                    { 5, "CA" },
                    { 6, "CO" },
                    { 7, "CT" },
                    { 8, "DC" },
                    { 10, "FL" },
                    { 11, "GA" },
                    { 12, "HI" },
                    { 9, "DE" },
                    { 14, "ID" },
                    { 24, "MN" },
                    { 13, "IA" },
                    { 22, "ME" },
                    { 21, "MD" },
                    { 20, "MA" },
                    { 23, "MI" },
                    { 18, "KY" },
                    { 17, "KS" },
                    { 16, "IN" },
                    { 15, "IL" },
                    { 19, "LA" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "StatusTypes",
                columns: new[] { "Id", "DisplayOrder", "Label" },
                values: new object[,]
                {
                    { 1, 1, "Pending" },
                    { 6, 2, "Unschedule" },
                    { 9, 3, "Complete" },
                    { 14, 4, "Archived" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "TaskStatuses",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Complete" },
                    { 3, "Archived" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "TimeTypes",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Absolute" },
                    { 2, "Relative" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "DetailTypes",
                columns: new[] { "Id", "DetailTypeSourceTypeId", "Label" },
                values: new object[,]
                {
                    { 2, 1, "Fact" },
                    { 19, 2, "Email" },
                    { 16, 2, "ContactReference" },
                    { 7, 2, "WebReference" },
                    { 4, 2, "Quote" },
                    { 40, 2, "DocReference" },
                    { 25, 1, "List" },
                    { 24, 1, "Draft" },
                    { 12, 1, "Note" },
                    { 5, 1, "Summary" },
                    { 41, 1, "PhoneCall" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Times",
                columns: new[] { "Id", "Label", "TimeTypeId" },
                values: new object[,]
                {
                    { 38, "06:30 PM", 1 },
                    { 29, "02:00 PM", 1 },
                    { 30, "02:30 PM", 1 },
                    { 31, "03:00 PM", 1 },
                    { 32, "03:30 PM", 1 },
                    { 33, "04:00 PM", 1 },
                    { 34, "04:30 PM", 1 },
                    { 35, "05:00 PM", 1 },
                    { 36, "05:30 PM", 1 },
                    { 37, "06:00 PM", 1 },
                    { 39, "07:00 PM", 1 },
                    { 45, "10:00 PM", 1 },
                    { 41, "08:00 PM", 1 },
                    { 42, "08:30 PM", 1 },
                    { 43, "09:00 PM", 1 },
                    { 44, "09:30 PM", 1 },
                    { 28, "01:30 PM", 1 },
                    { 46, "10:30 PM", 1 },
                    { 47, "11:00 PM", 1 },
                    { 48, "11:30 PM", 1 },
                    { 49, "1-ASAP", 2 },
                    { 50, "2-First", 2 },
                    { 51, "3-Today", 2 },
                    { 40, "07:30 PM", 1 },
                    { 27, "01:00 PM", 1 },
                    { 21, "10:00 AM", 1 },
                    { 25, "12:00 PM", 1 },
                    { 1, "12:00 AM", 1 },
                    { 2, "12:30 AM", 1 },
                    { 3, "01:00 AM", 1 },
                    { 4, "01:30 AM", 1 },
                    { 5, "02:00 AM", 1 },
                    { 6, "02:30 AM", 1 },
                    { 7, "03:00 AM", 1 },
                    { 8, "03:30 AM", 1 },
                    { 9, "04:00 AM", 1 },
                    { 10, "04:30 AM", 1 },
                    { 11, "05:00 AM", 1 },
                    { 26, "12:30 PM", 1 },
                    { 12, "05:30 AM", 1 },
                    { 14, "06:30 AM", 1 },
                    { 15, "07:00 AM", 1 },
                    { 16, "07:30 AM", 1 },
                    { 17, "08:00 AM", 1 },
                    { 18, "08:30 AM", 1 },
                    { 19, "09:00 AM", 1 },
                    { 20, "09:30 AM", 1 },
                    { 52, "4-Later", 2 },
                    { 22, "10:30 AM", 1 },
                    { 23, "11:00 AM", 1 },
                    { 24, "11:30 AM", 1 },
                    { 13, "06:00 AM", 1 },
                    { 53, "5-Low", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccruedTimes_TaskId",
                table: "AccruedTimes",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_AccruedTimes_UserId",
                table: "AccruedTimes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_StateId",
                table: "Addresses",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainLicenses_LicenseId",
                table: "DomainLicenses",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_CreatedByUserId",
                table: "Domains",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_TenantId",
                table: "Domains",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Domains_UpdatedByUserId",
                table: "Domains",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_CreatedByUserId",
                table: "Licenses",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_LicenseTypeId",
                table: "Licenses",
                column: "LicenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_UpdatedByUserId",
                table: "Licenses",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PointDetails_PointId",
                table: "PointDetails",
                column: "PointId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_CreatedByUserId",
                table: "Points",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ParentId",
                table: "Points",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ProjectId",
                table: "Points",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_UpdatedByUserId",
                table: "Points",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocuments_DocumentId",
                table: "ProjectDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedByUserId",
                table: "Projects",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DomainId",
                table: "Projects",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectTypeId",
                table: "Projects",
                column: "ProjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UpdatedByUserId",
                table: "Projects",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtasks_TaskId",
                table: "Subtasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtasks_UserId_TaskId",
                table: "Subtasks",
                columns: new[] { "UserId", "TaskId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskContacts_ContactId",
                table: "TaskContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedByUserId",
                table: "Tasks",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskStatusId",
                table: "Tasks",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypeId",
                table: "Tasks",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TimeId",
                table: "Tasks",
                column: "TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UpdatedByUserId",
                table: "Tasks",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWebsites_WebsiteId",
                table: "TaskWebsites",
                column: "WebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_AddressId",
                table: "Tenants",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CreatedByUserId",
                table: "Tenants",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_UpdatedByUserId",
                table: "Tenants",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDomains_DomainId",
                table: "UserDomains",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDomains_RoleId",
                table: "UserDomains",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_ProjectId",
                table: "UserProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CreatedByUserId",
                schema: "Detail",
                table: "Contacts",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_DomainId",
                schema: "Detail",
                table: "Contacts",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UpdatedByUserId",
                schema: "Detail",
                table: "Contacts",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_CreatedByUserId",
                schema: "Detail",
                table: "Details",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_DetailTypeId",
                schema: "Detail",
                table: "Details",
                column: "DetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_TaskId",
                schema: "Detail",
                table: "Details",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_UpdatedByUserId",
                schema: "Detail",
                table: "Details",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CreatedByUserId",
                schema: "Detail",
                table: "Documents",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentStatusId",
                schema: "Detail",
                table: "Documents",
                column: "DocumentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UpdatedByUserId",
                schema: "Detail",
                table: "Documents",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailTypes_DetailTypeSourceTypeId",
                schema: "Lookup",
                table: "DetailTypes",
                column: "DetailTypeSourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypes_DomainId",
                schema: "Lookup",
                table: "TaskTypes",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Times_TimeTypeId",
                schema: "Lookup",
                table: "Times",
                column: "TimeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "User",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "User",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "User",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "User",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "User",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "User",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccruedTimes");

            migrationBuilder.DropTable(
                name: "DomainLicenses");

            migrationBuilder.DropTable(
                name: "PointDetails");

            migrationBuilder.DropTable(
                name: "ProjectDocuments");

            migrationBuilder.DropTable(
                name: "Subtasks");

            migrationBuilder.DropTable(
                name: "TaskContacts");

            migrationBuilder.DropTable(
                name: "TaskWebsites");

            migrationBuilder.DropTable(
                name: "UserDomains");

            migrationBuilder.DropTable(
                name: "UserProjects");

            migrationBuilder.DropTable(
                name: "StatusTypes",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "User");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "User");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "User");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "User");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "User");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Details",
                schema: "Detail");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "Detail");

            migrationBuilder.DropTable(
                name: "Contacts",
                schema: "Detail");

            migrationBuilder.DropTable(
                name: "Websites",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "User");

            migrationBuilder.DropTable(
                name: "LicenseTypes",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "DetailTypes",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "DocumentStatuses",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "DetailTypeSourceTypes",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "TaskStatuses",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "TaskTypes",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Times",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "ProjectTypes",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Domains");

            migrationBuilder.DropTable(
                name: "TimeTypes",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "User");

            migrationBuilder.DropTable(
                name: "States",
                schema: "Lookup");
        }
    }
}
