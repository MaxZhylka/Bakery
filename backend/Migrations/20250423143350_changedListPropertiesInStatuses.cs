using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class changedListPropertiesInStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_LoanApplicationStatusTypes_LoanApplicationStatusTypeId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_PaymentStatusTypes_PaymentStatusTypeId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LoanApplicationStatusTypeId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_PaymentStatusTypeId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "LoanApplicationStatusTypeId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "PaymentStatusTypeId",
                table: "Loans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanApplicationStatusTypeId",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatusTypeId",
                table: "Loans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanApplicationStatusTypeId",
                table: "Loans",
                column: "LoanApplicationStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_PaymentStatusTypeId",
                table: "Loans",
                column: "PaymentStatusTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_LoanApplicationStatusTypes_LoanApplicationStatusTypeId",
                table: "Loans",
                column: "LoanApplicationStatusTypeId",
                principalTable: "LoanApplicationStatusTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_PaymentStatusTypes_PaymentStatusTypeId",
                table: "Loans",
                column: "PaymentStatusTypeId",
                principalTable: "PaymentStatusTypes",
                principalColumn: "Id");
        }
    }
}
