using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Api.Migrations
{
    public partial class AddEmployeeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("ca268a19-0f39-4d8b-b8d6-5bace54f8027"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c"), new DateTime(1955, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "M001", "William", 1, "Gates" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("265348d2-1276-4ada-ae33-4c1b8348edce"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c"), new DateTime(1998, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "M002", "Kent", 1, "Back" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("47b70abc-98b8-4fdc-b9fa-5dd6716f6e6b"), new Guid("6fb600c1-9011-4fd7-9234-881379716440"), new DateTime(1986, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "G001", "Mary", 2, "King" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("059e2fcb-e5a4-4188-9b46-06184bcb111b"), new Guid("6fb600c1-9011-4fd7-9234-881379716440"), new DateTime(1977, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "G002", "Kevin", 1, "Richardson" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("910e7452-c05f-4bf1-b084-6367873664a1"), new Guid("6fb600c1-9011-4fd7-9234-881379716440"), new DateTime(1982, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "G003", "Frederic", 1, "Pullan" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("a868ff18-3398-4598-b420-4878974a517a"), new Guid("6fb600c1-9011-4fd7-9234-881379716440"), new DateTime(1964, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "A001", "Jack", 1, "Ma" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("2c3bb40c-5907-4eb7-bb2c-7d62edb430c9"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c"), new DateTime(1997, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "A002", "Lorraine", 2, "Shaw" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("e32c33a7-df20-4b9a-a540-414192362d52"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c"), new DateTime(2000, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "A003", "Abel", 2, "Obadiah" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("3fae0ed7-5391-460a-8320-6b0255b62b72"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c"), new DateTime(1972, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "H001", "Alexia", 2, "More" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("1b863e75-8bd8-4876-8292-e99998bfa4b1"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c"), new DateTime(1999, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "H002", "Barton", 2, "Robin" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("c8353598-5b34-4529-a02b-dc7e9f93e59b"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c"), new DateTime(1990, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "H003", "Ted", 1, "Howard" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("ca86eded-a704-4fbc-8d5e-979761a2e0b8"), new Guid("bbdee09c-089b-4d30-bece-44df5923716c"), new DateTime(2000, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "M003", "Victor", 1, "Burns" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("059e2fcb-e5a4-4188-9b46-06184bcb111b"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("1b863e75-8bd8-4876-8292-e99998bfa4b1"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("265348d2-1276-4ada-ae33-4c1b8348edce"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("2c3bb40c-5907-4eb7-bb2c-7d62edb430c9"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("3fae0ed7-5391-460a-8320-6b0255b62b72"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("47b70abc-98b8-4fdc-b9fa-5dd6716f6e6b"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("910e7452-c05f-4bf1-b084-6367873664a1"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("a868ff18-3398-4598-b420-4878974a517a"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("c8353598-5b34-4529-a02b-dc7e9f93e59b"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("ca268a19-0f39-4d8b-b8d6-5bace54f8027"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("ca86eded-a704-4fbc-8d5e-979761a2e0b8"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("e32c33a7-df20-4b9a-a540-414192362d52"));
        }
    }
}
