using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AH.CancerConnect.AdminAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSymptomConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "ref",
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ProviderPool",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4075), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4077) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ProviderPool",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4078), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4078) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ProviderPool",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4080), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4080) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4178), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4179) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4180), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4181) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4182), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4182) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4184), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4184) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4186), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4186) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4187), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4188) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4189), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4189) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4191), new DateTime(2025, 10, 23, 23, 3, 4, 877, DateTimeKind.Utc).AddTicks(4191) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ref");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ProviderPool",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2363), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2372) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ProviderPool",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2373), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2373) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "ProviderPool",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2374), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2375) });

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

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2571), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2571) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2573), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2573) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2575), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2575) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2577), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2577) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2578), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2579) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2580), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2580) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2582), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2582) });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "SymptomConfiguration",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DateCreated", "DateModified" },
                values: new object[] { new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2583), new DateTime(2025, 10, 23, 22, 58, 30, 741, DateTimeKind.Utc).AddTicks(2584) });
        }
    }
}
