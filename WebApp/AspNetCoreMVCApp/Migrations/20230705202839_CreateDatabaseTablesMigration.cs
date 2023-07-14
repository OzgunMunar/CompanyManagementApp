using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreMVCApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabaseTablesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeJob",
                columns: table => new
                {
                    EmployeeJobTitleGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeJobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeJob", x => x.EmployeeJobTitleGuid);
                });

            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    OfficeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfficeCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficePhoneNumber = table.Column<int>(type: "int", nullable: false),
                    OfficeAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficePostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office", x => x.OfficeGuid);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeePhoneNumber = table.Column<int>(type: "int", nullable: false),
                    EmployeeEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeJobGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeOfficeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeManagerGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeePassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeGuid);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeJob_EmployeeJobGuid",
                        column: x => x.EmployeeJobGuid,
                        principalTable: "EmployeeJob",
                        principalColumn: "EmployeeJobTitleGuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Office_EmployeeOfficeGuid",
                        column: x => x.EmployeeOfficeGuid,
                        principalTable: "Office",
                        principalColumn: "OfficeGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPhoneNumber = table.Column<int>(type: "int", nullable: false),
                    CustomerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerBoughtFromEmployeeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerGuid);
                    table.ForeignKey(
                        name: "FK_Customer_Employees_CustomerBoughtFromEmployeeGuid",
                        column: x => x.CustomerBoughtFromEmployeeGuid,
                        principalTable: "Employees",
                        principalColumn: "EmployeeGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderIsShipped = table.Column<bool>(type: "bit", nullable: false),
                    OrderComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderCustomerGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderGuid);
                    table.ForeignKey(
                        name: "FK_Order_Customer_OrderCustomerGuid",
                        column: x => x.OrderCustomerGuid,
                        principalTable: "Customer",
                        principalColumn: "CustomerGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerBoughtFromEmployeeGuid",
                table: "Customer",
                column: "CustomerBoughtFromEmployeeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeJobGuid",
                table: "Employees",
                column: "EmployeeJobGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeOfficeGuid",
                table: "Employees",
                column: "EmployeeOfficeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderCustomerGuid",
                table: "Order",
                column: "OrderCustomerGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeJob");

            migrationBuilder.DropTable(
                name: "Office");
        }
    }
}
