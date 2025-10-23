using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AH.CancerConnect.API.Migrations
{
    /// <inheritdoc />
    public partial class drainagesetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "ref");

            migrationBuilder.CreateTable(
                name: "Patient",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MychartLogin = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Symptom",
                schema: "ref",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DisplayTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Invalid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SymptomCategory",
                schema: "ref",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DisplayValue = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrainageSetup",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    HasProviderGoalAmount = table.Column<bool>(type: "bit", nullable: false),
                    GoalDrainageAmount = table.Column<int>(type: "int", nullable: true),
                    ProviderInstructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrainageSetup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrainageSetup_Patient_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "dbo",
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NoteText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RecordingPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Note_Patient_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "dbo",
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SymptomEntry",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomEntry_Patient_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "dbo",
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ToDo",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Detail = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    Alert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDo_Patient_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "dbo",
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SymptomRange",
                schema: "ref",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SymptomId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SymptomValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomRange_SymptomCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "ref",
                        principalTable: "SymptomCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SymptomRange_Symptom_SymptomId",
                        column: x => x.SymptomId,
                        principalSchema: "ref",
                        principalTable: "Symptom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Drain",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrainageSetupId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateArchived = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drain_DrainageSetup_DrainageSetupId",
                        column: x => x.DrainageSetupId,
                        principalSchema: "dbo",
                        principalTable: "DrainageSetup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SymptomDetail",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SymptomEntryId = table.Column<int>(type: "int", nullable: false),
                    SymptomId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SymptomValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomDetail_SymptomCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "ref",
                        principalTable: "SymptomCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SymptomDetail_SymptomEntry_SymptomEntryId",
                        column: x => x.SymptomEntryId,
                        principalSchema: "dbo",
                        principalTable: "SymptomEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SymptomDetail_Symptom_SymptomId",
                        column: x => x.SymptomId,
                        principalSchema: "ref",
                        principalTable: "Symptom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Patient",
                columns: new[] { "Id", "Created", "FirstName", "LastName", "MychartLogin", "Status" },
                values: new object[] { 1, new DateTime(2025, 10, 14, 3, 30, 43, 32, DateTimeKind.Utc).AddTicks(3110), "Test", "User", "testuser", "Active" });

            migrationBuilder.InsertData(
                schema: "ref",
                table: "Symptom",
                columns: new[] { "Id", "Description", "DisplayTitle", "Invalid", "Name" },
                values: new object[,]
                {
                    { 1, "Patient anxiety levels", "Anxiety", false, "Anxiety" },
                    { 2, "Loss of appetite", "Appetite Loss", false, "Appetite Loss" },
                    { 3, "Any bleeding symptoms", "Bleeding", false, "Bleeding" },
                    { 4, "Difficulty with daily activities", "Impaired Activities", false, "Impaired activity (ADLs)" },
                    { 5, "Bowel movement difficulties", "Constipation", false, "Constipation" },
                    { 6, "Persistent coughing", "Cough", false, "Cough" },
                    { 7, "Emotional depression", "Depression", false, "Depression" },
                    { 8, "Loose bowel movements", "Diarrhea", false, "Diarrhea" },
                    { 9, "Tiredness and low energy", "Fatigue", false, "Fatigue" },
                    { 10, "Elevated body temperature", "Fever", false, "Fever" },
                    { 11, "Feeling sick or vomiting", "Nausea/Vomiting", false, "Nausea or Vomiting" },
                    { 12, "Physical pain levels", "Pain", false, "Pain" },
                    { 13, "Peripheral neuropathy symptoms", "Tingling/Numbness", false, "Tingling and Numbness in hands or feet" },
                    { 14, "Difficulty breathing", "Shortness of Breath", false, "Shortness of breath" },
                    { 15, "Skin irritation or rash", "Skin Rash", false, "Skin rash" },
                    { 16, "Red skin conditions", "Skin Redness", false, "Redness of skin" },
                    { 17, "Fluid retention and swelling", "Swelling", false, "Edema/Swelling" },
                    { 18, "Oral cavity sores", "Mouth Sores", false, "Mouth Sores" },
                    { 19, "Overall emotional distress level", "Emotional Distress", false, "Level of Emotional Distress" }
                });

            migrationBuilder.InsertData(
                schema: "ref",
                table: "SymptomCategory",
                columns: new[] { "Id", "DisplayValue", "Name" },
                values: new object[,]
                {
                    { 1, "Mild/Moderate/Severe", "MildModerateSevere" },
                    { 2, "Yes/No", "YesNo" },
                    { 3, "1-10 Scale", "Scale1To10" }
                });

            migrationBuilder.InsertData(
                schema: "ref",
                table: "SymptomRange",
                columns: new[] { "Id", "CategoryId", "SymptomId", "SymptomValue" },
                values: new object[,]
                {
                    { 1, 1, 1, "Mild" },
                    { 2, 1, 1, "Moderate" },
                    { 3, 1, 1, "Severe" },
                    { 4, 1, 2, "Mild" },
                    { 5, 1, 2, "Moderate" },
                    { 6, 1, 2, "Severe" },
                    { 7, 1, 4, "Mild" },
                    { 8, 1, 4, "Moderate" },
                    { 9, 1, 4, "Severe" },
                    { 10, 1, 5, "Mild" },
                    { 11, 1, 5, "Moderate" },
                    { 12, 1, 5, "Severe" },
                    { 13, 1, 6, "Mild" },
                    { 14, 1, 6, "Moderate" },
                    { 15, 1, 6, "Severe" },
                    { 16, 1, 7, "Mild" },
                    { 17, 1, 7, "Moderate" },
                    { 18, 1, 7, "Severe" },
                    { 19, 1, 8, "Mild" },
                    { 20, 1, 8, "Moderate" },
                    { 21, 1, 8, "Severe" },
                    { 22, 1, 9, "Mild" },
                    { 23, 1, 9, "Moderate" },
                    { 24, 1, 9, "Severe" },
                    { 25, 1, 11, "Mild" },
                    { 26, 1, 11, "Moderate" },
                    { 27, 1, 11, "Severe" },
                    { 28, 1, 13, "Mild" },
                    { 29, 1, 13, "Moderate" },
                    { 30, 1, 13, "Severe" },
                    { 31, 1, 14, "Mild" },
                    { 32, 1, 14, "Moderate" },
                    { 33, 1, 14, "Severe" },
                    { 34, 2, 3, "No" },
                    { 35, 2, 3, "Yes" },
                    { 36, 2, 10, "No" },
                    { 37, 2, 10, "Yes" },
                    { 38, 2, 15, "No" },
                    { 39, 2, 15, "Yes" },
                    { 40, 2, 16, "No" },
                    { 41, 2, 16, "Yes" },
                    { 42, 2, 17, "No" },
                    { 43, 2, 17, "Yes" },
                    { 44, 2, 18, "No" },
                    { 45, 2, 18, "Yes" },
                    { 46, 3, 12, "1" },
                    { 47, 3, 12, "2" },
                    { 48, 3, 12, "3" },
                    { 49, 3, 12, "4" },
                    { 50, 3, 12, "5" },
                    { 51, 3, 12, "6" },
                    { 52, 3, 12, "7" },
                    { 53, 3, 12, "8" },
                    { 54, 3, 12, "9" },
                    { 55, 3, 12, "10" },
                    { 56, 3, 19, "1" },
                    { 57, 3, 19, "2" },
                    { 58, 3, 19, "3" },
                    { 59, 3, 19, "4" },
                    { 60, 3, 19, "5" },
                    { 61, 3, 19, "6" },
                    { 62, 3, 19, "7" },
                    { 63, 3, 19, "8" },
                    { 64, 3, 19, "9" },
                    { 65, 3, 19, "10" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drain_DrainageSetupId_IsArchived",
                schema: "dbo",
                table: "Drain",
                columns: new[] { "DrainageSetupId", "IsArchived" });

            migrationBuilder.CreateIndex(
                name: "IX_DrainageSetup_PatientId",
                schema: "dbo",
                table: "DrainageSetup",
                column: "PatientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_PatientId",
                schema: "dbo",
                table: "Note",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomDetail_CategoryId",
                schema: "dbo",
                table: "SymptomDetail",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomDetail_SymptomEntryId",
                schema: "dbo",
                table: "SymptomDetail",
                column: "SymptomEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomDetail_SymptomId",
                schema: "dbo",
                table: "SymptomDetail",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomEntry_PatientId",
                schema: "dbo",
                table: "SymptomEntry",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomRange_CategoryId",
                schema: "ref",
                table: "SymptomRange",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomRange_SymptomId",
                schema: "ref",
                table: "SymptomRange",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDo_PatientId",
                schema: "dbo",
                table: "ToDo",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drain",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Note",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SymptomDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SymptomRange",
                schema: "ref");

            migrationBuilder.DropTable(
                name: "ToDo",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DrainageSetup",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SymptomEntry",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SymptomCategory",
                schema: "ref");

            migrationBuilder.DropTable(
                name: "Symptom",
                schema: "ref");

            migrationBuilder.DropTable(
                name: "Patient",
                schema: "dbo");
        }
    }
}
