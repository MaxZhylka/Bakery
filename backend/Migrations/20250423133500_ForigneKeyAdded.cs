using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ForigneKeyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_RejectionReasonId",
                table: "LoanApplications",
                column: "RejectionReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_RejectionReasons_RejectionReasonId",
                table: "LoanApplications",
                column: "RejectionReasonId",
                principalTable: "RejectionReasons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_RejectionReasons_RejectionReasonId",
                table: "LoanApplications");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_RejectionReasonId",
                table: "LoanApplications");
        }
    }
}
