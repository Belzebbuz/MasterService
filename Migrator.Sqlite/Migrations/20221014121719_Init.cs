using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrator.Sqlite.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TelegramUserId = table.Column<long>(type: "INTEGER", nullable: true),
                    TelegramChatBotId = table.Column<long>(type: "INTEGER", nullable: true),
                    ProfilePictureDataUrl = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    TableName = table.Column<string>(type: "TEXT", nullable: true),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OldValues = table.Column<string>(type: "TEXT", nullable: true),
                    NewValues = table.Column<string>(type: "TEXT", nullable: true),
                    AffectedColumns = table.Column<string>(type: "TEXT", nullable: true),
                    PrimaryKey = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayTimetables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartWorkTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndWorkTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayTimetables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayTimetables_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClientId = table.Column<string>(type: "TEXT", nullable: true),
                    Confirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Complited = table.Column<bool>(type: "INTEGER", nullable: false),
                    Canceled = table.Column<bool>(type: "INTEGER", nullable: false),
                    DayTimetableId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientOrders_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientOrders_DayTimetables_DayTimetableId",
                        column: x => x.DayTimetableId,
                        principalTable: "DayTimetables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MasterServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: true),
                    ClientOrderId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterServices_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MasterServices_ClientOrders_ClientOrderId",
                        column: x => x.ClientOrderId,
                        principalTable: "ClientOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    IsChanged = table.Column<bool>(type: "INTEGER", nullable: false),
                    ValueChangedTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AuthorId = table.Column<string>(type: "TEXT", nullable: false),
                    ClientOrderId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderComments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderComments_ClientOrders_ClientOrderId",
                        column: x => x.ClientOrderId,
                        principalTable: "ClientOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MasterServicePhotos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    MasterServiceId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterServicePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterServicePhotos_MasterServices_MasterServiceId",
                        column: x => x.MasterServiceId,
                        principalTable: "MasterServices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Value = table.Column<decimal>(type: "TEXT", nullable: false),
                    MasterServiceId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_MasterServices_MasterServiceId",
                        column: x => x.MasterServiceId,
                        principalTable: "MasterServices",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "42e5251f-5fc5-41b2-ac5b-1cc49cd117ed", "f0bd7c9b-98d7-4043-8739-e0020b696eca", "Basic", "BASIC" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b11754fe-34d8-4b70-84ae-f13dc648456f", "c3205d01-eb4f-4f28-a954-4132d8604e79", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "edcd5632-0a70-4ebd-9b58-926134a181c0", "f29401df-5e1b-4efe-93fc-6cd77ac043de", "Master", "MASTER" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientOrders_ClientId",
                table: "ClientOrders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientOrders_DayTimetableId",
                table: "ClientOrders",
                column: "DayTimetableId");

            migrationBuilder.CreateIndex(
                name: "IX_DayTimetables_AppUserId",
                table: "DayTimetables",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterServicePhotos_MasterServiceId",
                table: "MasterServicePhotos",
                column: "MasterServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterServices_AppUserId",
                table: "MasterServices",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterServices_ClientOrderId",
                table: "MasterServices",
                column: "ClientOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderComments_AuthorId",
                table: "OrderComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderComments_ClientOrderId",
                table: "OrderComments",
                column: "ClientOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_MasterServiceId",
                table: "Prices",
                column: "MasterServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "MasterServicePhotos");

            migrationBuilder.DropTable(
                name: "OrderComments");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "MasterServices");

            migrationBuilder.DropTable(
                name: "ClientOrders");

            migrationBuilder.DropTable(
                name: "DayTimetables");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
