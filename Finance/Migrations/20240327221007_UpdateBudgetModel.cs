using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBudgetModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrentTransactions_Budgets_BudgetId",
                table: "CurrentTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrentTransactions",
                table: "CurrentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CurrentTransactions_BudgetId",
                table: "CurrentTransactions");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "CurrentTransactions");

            migrationBuilder.RenameTable(
                name: "CurrentTransactions",
                newName: "Transactions");

            migrationBuilder.RenameColumn(
                name: "BudgetCategory",
                table: "Budgets",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "BudgetAmount",
                table: "Budgets",
                newName: "AmountAlotted");

            migrationBuilder.RenameColumn(
                name: "BudgetId",
                table: "Budgets",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "CurrentTransactions");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Budgets",
                newName: "BudgetCategory");

            migrationBuilder.RenameColumn(
                name: "AmountAlotted",
                table: "Budgets",
                newName: "BudgetAmount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Budgets",
                newName: "BudgetId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CurrentTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BudgetId",
                table: "CurrentTransactions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrentTransactions",
                table: "CurrentTransactions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentTransactions_BudgetId",
                table: "CurrentTransactions",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentTransactions_Budgets_BudgetId",
                table: "CurrentTransactions",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "BudgetId");
        }
    }
}
