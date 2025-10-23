using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AH.CancerConnect.AdminAPI.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "ProviderPool",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderPool", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProviderPoolId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provider_ProviderPool_ProviderPoolId",
                        column: x => x.ProviderPoolId,
                        principalSchema: "dbo",
                        principalTable: "ProviderPool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ProviderPool",
                columns: new[] { "Id", "DateCreated", "DateModified", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 21, 14, 10, 48, 626, DateTimeKind.Utc).AddTicks(8056), new DateTime(2025, 10, 21, 14, 10, 48, 626, DateTimeKind.Utc).AddTicks(8059), "Primary care team for oncology patients", true, "Provider Pool A" },
                    { 2, new DateTime(2025, 10, 21, 14, 10, 48, 626, DateTimeKind.Utc).AddTicks(8062), new DateTime(2025, 10, 21, 14, 10, 48, 626, DateTimeKind.Utc).AddTicks(8063), "Surgical care team for cancer treatment", true, "Provider Pool B" },
                    { 3, new DateTime(2025, 10, 21, 14, 10, 48, 626, DateTimeKind.Utc).AddTicks(8065), new DateTime(2025, 10, 21, 14, 10, 48, 626, DateTimeKind.Utc).AddTicks(8066), "Radiation and chemotherapy specialist team", true, "Provider Pool C" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Provider_IsActive",
                schema: "dbo",
                table: "Provider",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Provider_LastName_FirstName",
                schema: "dbo",
                table: "Provider",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_Provider_ProviderId",
                schema: "dbo",
                table: "Provider",
                column: "ProviderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provider_ProviderPoolId",
                schema: "dbo",
                table: "Provider",
                column: "ProviderPoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderPool_IsActive",
                schema: "dbo",
                table: "ProviderPool",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderPool_Name",
                schema: "dbo",
                table: "ProviderPool",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Provider",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProviderPool",
                schema: "dbo");
        }
    }
}
