using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class enumChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "LoanApplications");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Payments",
                newName: "PaymentStatusTypeId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Loans",
                newName: "LoanStatusTypeId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "LoanApplications",
                newName: "LoanApplicationStatusTypeId");

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

            migrationBuilder.AddColumn<Guid>(
                name: "RejectionReasonId",
                table: "LoanApplications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoanApplicationStatusTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanApplicationStatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanStatusTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanStatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatusTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RejectionReasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectionReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SettingsData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BackupPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentStatusTypeId",
                table: "Payments",
                column: "PaymentStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanApplicationStatusTypeId",
                table: "Loans",
                column: "LoanApplicationStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanStatusTypeId",
                table: "Loans",
                column: "LoanStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_PaymentStatusTypeId",
                table: "Loans",
                column: "PaymentStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_LoanApplicationStatusTypeId",
                table: "LoanApplications",
                column: "LoanApplicationStatusTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_LoanApplicationStatusTypes_LoanApplicationStatusTypeId",
                table: "LoanApplications",
                column: "LoanApplicationStatusTypeId",
                principalTable: "LoanApplicationStatusTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_LoanApplicationStatusTypes_LoanApplicationStatusTypeId",
                table: "Loans",
                column: "LoanApplicationStatusTypeId",
                principalTable: "LoanApplicationStatusTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_LoanStatusTypes_LoanStatusTypeId",
                table: "Loans",
                column: "LoanStatusTypeId",
                principalTable: "LoanStatusTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_PaymentStatusTypes_PaymentStatusTypeId",
                table: "Loans",
                column: "PaymentStatusTypeId",
                principalTable: "PaymentStatusTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentStatusTypes_PaymentStatusTypeId",
                table: "Payments",
                column: "PaymentStatusTypeId",
                principalTable: "PaymentStatusTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_LoanApplicationStatusTypes_LoanApplicationStatusTypeId",
                table: "LoanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_LoanApplicationStatusTypes_LoanApplicationStatusTypeId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_LoanStatusTypes_LoanStatusTypeId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_PaymentStatusTypes_PaymentStatusTypeId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentStatusTypes_PaymentStatusTypeId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "LoanApplicationStatusTypes");

            migrationBuilder.DropTable(
                name: "LoanStatusTypes");

            migrationBuilder.DropTable(
                name: "PaymentStatusTypes");

            migrationBuilder.DropTable(
                name: "RejectionReasons");

            migrationBuilder.DropTable(
                name: "SettingsData");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentStatusTypeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LoanApplicationStatusTypeId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LoanStatusTypeId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_PaymentStatusTypeId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_LoanApplicationStatusTypeId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "LoanApplicationStatusTypeId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "PaymentStatusTypeId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "RejectionReasonId",
                table: "LoanApplications");

            migrationBuilder.RenameColumn(
                name: "PaymentStatusTypeId",
                table: "Payments",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "LoanStatusTypeId",
                table: "Loans",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "LoanApplicationStatusTypeId",
                table: "LoanApplications",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
