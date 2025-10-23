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
                name: "SymptomCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SymptomConfiguration",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SymptomId = table.Column<int>(type: "int", nullable: false),
                    SeverityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ToolTipContent = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    AlertTrigger = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ShowForBreast = table.Column<bool>(type: "bit", nullable: false),
                    ShowForLung = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomConfiguration_Symptom_SymptomId",
                        column: x => x.SymptomId,
                        principalSchema: "ref",
                        principalTable: "Symptom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "SymptomRange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SymptomId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SymptomValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomRange_SymptomCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "SymptomCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SymptomRange_Symptom_SymptomId",
                        column: x => x.SymptomId,
                        principalSchema: "ref",
                        principalTable: "Symptom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "ProviderPool",
                columns: new[] { "Id", "DateCreated", "DateModified", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(5980), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(5983), "Primary care team for oncology patients", true, "Provider Pool A" },
                    { 2, new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(5986), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(5987), "Surgical care team for cancer treatment", true, "Provider Pool B" },
                    { 3, new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(5989), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(5990), "Radiation and chemotherapy specialist team", true, "Provider Pool C" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "SymptomConfiguration",
                columns: new[] { "Id", "AlertTrigger", "DateCreated", "DateModified", "IsActive", "Notes", "SeverityType", "ShowForBreast", "ShowForLung", "SymptomId", "ToolTipContent" },
                values: new object[,]
                {
                    { 1, "When Severe is indicated for 1+ day or Moderate is indicated for 2+ days", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6134), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6135), true, "Used CTCAE (Common Terminology Criteria for Adverse Events); Consider incorporating GAD-7 and PHQ-2 screening tools.", "Mild, Moderate, Severe", true, true, 1, "Mild: not interfering with day to day activities; Moderate: interfering somewhat with day to day activities; Severe: severely limiting daily activities including self-care." },
                    { 2, "When 2 days of moderate or 1 score of Severe", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6140), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6141), true, "Alert triggered-When 2 days of moderate or 1 score of Severe. Ask the Dietician for input.", "Mild, Moderate, Severe", true, true, 2, "Mild: loss of appetite without changing your eating habits; Moderate: your eating habits have changed but you haven't lost any weight; Severe: You are unable to eat or drink and have lost weight." },
                    { 3, "When Yes is indicated for 1+ day", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6144), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6145), true, "Could we use directions provided by the surgeon? Need guidance from Tess Abrahmson.", "Yes or No", true, true, 3, "Bleeding through bandage within an hour, bruising, having nose bleeds, bleeding gums, blood in urine, etc." },
                    { 4, "When 2 days of moderate or 1 score of Severe", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6148), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6148), true, "Use CTCAE 'flu like symptoms'. Trigger: Moderate 2 days, severe 1 day", "Mild, Moderate, Severe", true, true, 4, "Mild: does not impact your activities; Moderate: somewhat limit your activities and self-care; Severe: severely or completely limit your activities and self-care" },
                    { 5, "When 2 days of moderate or 1 score of Severe", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6153), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6154), true, "When moderate is listed for 2 days or severe for 1 day", "Mild, Moderate, Severe", true, true, 5, "Mild: occasional symptoms or symptoms that come and go, occasionally may use a bowel medication like a stool softener or laxative; Moderate: persistent symptoms, regularly use bowel medications like a laxative or enema, somewhat limits your activities; Severe: does not resolve with bowel medications, severely limits activities and self-care" },
                    { 6, "When 2 days of moderate or 1 score of Severe", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6156), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6157), true, "Moderate 2-3 days; severe 1 day", "Mild, Moderate, Severe", true, true, 6, "Mild: cough does not require any over the counter medication, does not interfere with your activities; Moderate: Cough requires over the counter medications, somewhat limiting your activities; Severe: cough is severe requiring medical intervention, severely limits activities and self-care" },
                    { 7, "When Severe is indicated for 1+ day", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6159), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6160), true, "Need to take into consideration if patient has chronic depression. Consider Social Worker assistance.", "Mild, Moderate, Severe", true, true, 7, "Mild: not interfering with day to day activities; Moderate: interfering somewhat with day to day activities; Severe: severely limiting daily activities including self-care." },
                    { 8, "When 2 days of moderate or 1 score of Severe", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6162), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6163), true, "Moderate x 2 days, severe x1 day (from Oncology RN Guideline)", "Mild, Moderate, Severe", true, true, 8, "Mild: increase of less than 4 stools/day over baseline; mild increase in ostomy output compared to baseline; Moderate: Increase 4-6 stools/day over baseline; moderate increase in ostomy output compared to baseline, limiting activities; Severe: Increase of 7 or more stools per day over baseline; severe increase in ostomy output, limiting activities and self care" },
                    { 9, "When 2 days of moderate or 1 score of Severe", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6165), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6166), true, "Moderate x2, severe x 1 (from oncology RN guideline)", "Mild, Moderate, Severe", true, true, 9, "Mild: fatigue relieved by rest; Moderate: fatigue not relieved by rest, limiting activities; Severe: Fatigue not relieved by rest, limiting self care" },
                    { 10, "Trigger when 1 day of Yes", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6168), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6169), true, "x1 of mild. If patients have a fever, they are instructed to call their care team.", "Yes or No", true, true, 10, "No when temp is 100.4 or lower; Yes when temp is over 100.4" },
                    { 11, "When 1 days of moderate or 1 score of Severe", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6171), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6172), true, "x1 of moderate (oncology RN guideline)", "Mild, Moderate, Severe", true, true, 11, "Mild: loss of appetite without changing your eating habits, that are resolved with prescribed medication; Moderate: your eating habits have changed but you haven't lost any weight or any episode of vomiting, that are not resolved with prescribed medication; Severe: You are unable to eat or drink and have lost weight or any episode of vomiting that are not resolved with prescribed medication." },
                    { 12, "When an 4 or above is indicated for 1 time", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6174), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6175), true, "Moderate and severe x1 (ONS Telephone triage). Used CTCAE grading criteria", "1 to 10 Scale", true, true, 12, "1-3: mild pain; 4-6: moderate pain; limiting activities; 7-10: severe pain, limiting self care" },
                    { 13, "When Severe is indicated for 2+ days", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6177), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6178), true, "CTCAE grading. How do we account for an expected side effect?", "Mild, Moderate, Severe", true, true, 13, "Mild: Noticeable but not impactful on day-to-day activities; Moderate: Includes symptoms like hot or cold sensitivity, decrease in grip strength, or stability on feet; Severe: Symptoms such as inability to button clothing or pick up smaller items." },
                    { 14, "When 1 days of moderate or 1 day of Severe", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6252), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6253), true, "Moderate x1; severe x1 (ONS Telephone triage)", "Mild, Moderate, Severe", true, true, 14, "Mild: Shortness of breath with moderate exertion; Moderate: Shortness of breath with minimal exertion; limits activities; Severe: Shortness of breath at rest, limiting self care" },
                    { 15, "Trigger 1 day of Yes", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6256), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6256), true, null, "Yes or No", true, true, 15, "Yes a new rash" },
                    { 16, "Trigger 1 day of Yes", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6259), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6259), true, "Should we generalize this and skin rash to skin changes?", "Yes or No", true, true, 16, "Yes there is redness" },
                    { 17, "Trigger 1 day of Yes", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6262), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6262), true, "Yes for both breast and lung", "Yes or No", true, true, 17, "Yes: Swelling anywhere on body" },
                    { 18, "Trigger 1 day of Yes", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6264), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6265), true, "Yes for both breast and lung", "Yes or No", true, true, 18, "Yes, mouth sores present" },
                    { 19, "Trigger when score 4 or higher for 4 days in a row or score 4 or higher for 4 days within 10 days", new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6267), new DateTime(2025, 10, 23, 22, 15, 11, 287, DateTimeKind.Utc).AddTicks(6268), true, "Recommendation from Social Work Team and Operations", "Scale of 0-10", true, true, 19, "Distress is an unpleasant experience of a mental, physical, social, or spiritual nature. It can affect the way you think, feel, or act. Distress may make it harder to cope with having cancer, its symptoms, or its treatment. Instructions: Please circle the number (0–10) that best describes how much distress you have been experiencing in the past week, including today." }
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

            migrationBuilder.CreateIndex(
                name: "IX_SymptomConfiguration_IsActive",
                schema: "dbo",
                table: "SymptomConfiguration",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomConfiguration_ShowForBreast_IsActive",
                schema: "dbo",
                table: "SymptomConfiguration",
                columns: new[] { "ShowForBreast", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_SymptomConfiguration_ShowForLung_IsActive",
                schema: "dbo",
                table: "SymptomConfiguration",
                columns: new[] { "ShowForLung", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_SymptomConfiguration_SymptomId",
                schema: "dbo",
                table: "SymptomConfiguration",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomRange_CategoryId",
                table: "SymptomRange",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomRange_SymptomId",
                table: "SymptomRange",
                column: "SymptomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Provider",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SymptomConfiguration",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SymptomRange");

            migrationBuilder.DropTable(
                name: "ProviderPool",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SymptomCategory");
        }
    }
}
