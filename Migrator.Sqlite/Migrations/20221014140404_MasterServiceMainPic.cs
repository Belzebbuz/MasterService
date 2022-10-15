using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrator.Sqlite.Migrations
{
    public partial class MasterServiceMainPic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42e5251f-5fc5-41b2-ac5b-1cc49cd117ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b11754fe-34d8-4b70-84ae-f13dc648456f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edcd5632-0a70-4ebd-9b58-926134a181c0");

            migrationBuilder.AddColumn<string>(
                name: "MainPucturePathUrl",
                table: "MasterServices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3b2b7256-cd0a-4dbf-b96b-cf07eabdba16", "919fef44-dd26-4896-945f-397ec89ea49f", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "59908b67-f3d7-4a87-9382-f9cc898174c4", "0b8f8cb2-a9e3-4adc-9cd5-8f00bf2edac1", "Basic", "BASIC" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "72794081-1098-4439-8bfa-c17921a85bcc", "bf3bb030-41bb-4632-9878-c444cecbd422", "Master", "MASTER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b2b7256-cd0a-4dbf-b96b-cf07eabdba16");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59908b67-f3d7-4a87-9382-f9cc898174c4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72794081-1098-4439-8bfa-c17921a85bcc");

            migrationBuilder.DropColumn(
                name: "MainPucturePathUrl",
                table: "MasterServices");

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
        }
    }
}
