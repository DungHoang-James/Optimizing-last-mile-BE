using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptimizingLastMile.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "OrderInformation",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentOrderStatus",
                table: "OrderInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDay",
                table: "DriverProfile",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderInformation_AccountId",
                table: "OrderInformation",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderInformation_Account_AccountId",
                table: "OrderInformation",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderInformation_Account_AccountId",
                table: "OrderInformation");

            migrationBuilder.DropIndex(
                name: "IX_OrderInformation_AccountId",
                table: "OrderInformation");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "OrderInformation");

            migrationBuilder.DropColumn(
                name: "CurrentOrderStatus",
                table: "OrderInformation");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDay",
                table: "DriverProfile",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
