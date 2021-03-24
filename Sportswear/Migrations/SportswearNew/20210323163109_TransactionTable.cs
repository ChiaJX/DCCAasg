using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sportswear.Migrations.SportswearNew
{
    public partial class TransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    transactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(nullable: false),
                    userAddress = table.Column<string>(nullable: false),
                    userPhone = table.Column<string>(nullable: false),
                    orderId = table.Column<string>(nullable: false),
                    product = table.Column<string>(nullable: false),
                    couponId = table.Column<string>(nullable: true),
                    message = table.Column<string>(nullable: true),
                    price = table.Column<decimal>(nullable: false),
                    TransactionDateTime = table.Column<DateTime>(nullable: false),
                    status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.transactionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}
