using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeAccounting.Domain.Migrations
{
    public partial class Updatedentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Currencies_MainCurrencyCode",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyFromCode1",
                table: "ExchangeRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyToCode1",
                table: "ExchangeRates");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Currencies_CurrencyFromId",
                table: "Ledgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Currencies_CurrencyToId",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_CurrencyFromId",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_CurrencyToId",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "Constraint",
                table: "TransactionCategories");

            migrationBuilder.DropColumn(
                name: "CurrencyFromId",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "CurrencyToId",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "CurrencyFromCode",
                table: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "CurrencyToCode",
                table: "ExchangeRates");

            migrationBuilder.RenameColumn(
                name: "CurrencyToCode1",
                table: "ExchangeRates",
                newName: "CurrencyToId");

            migrationBuilder.RenameColumn(
                name: "CurrencyFromCode1",
                table: "ExchangeRates",
                newName: "CurrencyFromId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeRates_CurrencyToCode1",
                table: "ExchangeRates",
                newName: "IX_ExchangeRates_CurrencyToId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeRates_CurrencyFromCode1",
                table: "ExchangeRates",
                newName: "IX_ExchangeRates_CurrencyFromId");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Currencies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MainCurrencyCode",
                table: "AspNetUsers",
                newName: "MainCurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_MainCurrencyCode",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_MainCurrencyId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Ledgers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Constraint",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyId",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CurrencyId",
                table: "Accounts",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Currencies_CurrencyId",
                table: "Accounts",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Currencies_MainCurrencyId",
                table: "AspNetUsers",
                column: "MainCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyFromId",
                table: "ExchangeRates",
                column: "CurrencyFromId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyToId",
                table: "ExchangeRates",
                column: "CurrencyToId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Currencies_CurrencyId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Currencies_MainCurrencyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyFromId",
                table: "ExchangeRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyToId",
                table: "ExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_CurrencyId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "Constraint",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "CurrencyToId",
                table: "ExchangeRates",
                newName: "CurrencyToCode1");

            migrationBuilder.RenameColumn(
                name: "CurrencyFromId",
                table: "ExchangeRates",
                newName: "CurrencyFromCode1");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeRates_CurrencyToId",
                table: "ExchangeRates",
                newName: "IX_ExchangeRates_CurrencyToCode1");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeRates_CurrencyFromId",
                table: "ExchangeRates",
                newName: "IX_ExchangeRates_CurrencyFromCode1");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Currencies",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "MainCurrencyId",
                table: "AspNetUsers",
                newName: "MainCurrencyCode");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_MainCurrencyId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_MainCurrencyCode");

            migrationBuilder.AddColumn<decimal>(
                name: "Constraint",
                table: "TransactionCategories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyFromId",
                table: "Ledgers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyToId",
                table: "Ledgers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyFromCode",
                table: "ExchangeRates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyToCode",
                table: "ExchangeRates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_CurrencyFromId",
                table: "Ledgers",
                column: "CurrencyFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_CurrencyToId",
                table: "Ledgers",
                column: "CurrencyToId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Currencies_MainCurrencyCode",
                table: "AspNetUsers",
                column: "MainCurrencyCode",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyFromCode1",
                table: "ExchangeRates",
                column: "CurrencyFromCode1",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyToCode1",
                table: "ExchangeRates",
                column: "CurrencyToCode1",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Currencies_CurrencyFromId",
                table: "Ledgers",
                column: "CurrencyFromId",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Currencies_CurrencyToId",
                table: "Ledgers",
                column: "CurrencyToId",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
