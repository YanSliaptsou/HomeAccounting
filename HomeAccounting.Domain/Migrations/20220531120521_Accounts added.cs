using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeAccounting.Domain.Migrations
{
    public partial class Accountsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AspNetUsers_AppUserId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_TransactionCategories_TransactionCategoryId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Account_AccountFromId",
                table: "Ledgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Account_AccountToId",
                table: "Ledgers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_Account_TransactionCategoryId",
                table: "Accounts",
                newName: "IX_Accounts_TransactionCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_AppUserId",
                table: "Accounts",
                newName: "IX_Accounts_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AspNetUsers_AppUserId",
                table: "Accounts",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_TransactionCategories_TransactionCategoryId",
                table: "Accounts",
                column: "TransactionCategoryId",
                principalTable: "TransactionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Accounts_AccountFromId",
                table: "Ledgers",
                column: "AccountFromId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Accounts_AccountToId",
                table: "Ledgers",
                column: "AccountToId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AspNetUsers_AppUserId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_TransactionCategories_TransactionCategoryId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Accounts_AccountFromId",
                table: "Ledgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Accounts_AccountToId",
                table: "Ledgers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_TransactionCategoryId",
                table: "Account",
                newName: "IX_Account_TransactionCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_AppUserId",
                table: "Account",
                newName: "IX_Account_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AspNetUsers_AppUserId",
                table: "Account",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_TransactionCategories_TransactionCategoryId",
                table: "Account",
                column: "TransactionCategoryId",
                principalTable: "TransactionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Account_AccountFromId",
                table: "Ledgers",
                column: "AccountFromId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Account_AccountToId",
                table: "Ledgers",
                column: "AccountToId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
