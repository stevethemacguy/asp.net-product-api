using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProductApi.Migrations
{
    public partial class AddedReportEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AverageDiscountAmount = table.Column<decimal>(nullable: false),
                    AverageNumberOfItemsPurchased = table.Column<double>(nullable: false),
                    AverageOrderCost = table.Column<decimal>(nullable: false),
                    GeneratedByUser = table.Column<string>(nullable: true),
                    LargestOrderItemCount = table.Column<int>(nullable: false),
                    LargestOrderTotalPrice = table.Column<decimal>(nullable: false),
                    MostPopularProductId = table.Column<int>(nullable: true),
                    MostPopularProductInLastDaysId = table.Column<int>(nullable: true),
                    MostPopularProductInLastMonthId = table.Column<int>(nullable: true),
                    OrderCount = table.Column<int>(nullable: false),
                    OrderCountByDays = table.Column<int>(nullable: false),
                    OrderCountByMonths = table.Column<int>(nullable: false),
                    OrderCountByWeeks = table.Column<int>(nullable: false),
                    OrderCountByYears = table.Column<int>(nullable: false),
                    OrderWithLargestTotalPrice = table.Column<int>(nullable: false),
                    OrderWithlargestItemCount = table.Column<int>(nullable: false),
                    OrdersCancelled = table.Column<int>(nullable: false),
                    OrdersComplete = table.Column<int>(nullable: false),
                    OrdersPending = table.Column<int>(nullable: false),
                    OrdersProcessing = table.Column<int>(nullable: false),
                    ReportGeneratedDate = table.Column<DateTimeOffset>(nullable: false),
                    SalesInLastMonth = table.Column<decimal>(nullable: false),
                    SalesInLastNumberOfDays = table.Column<decimal>(nullable: false),
                    SalesInLastWeek = table.Column<decimal>(nullable: false),
                    SalesInLastYear = table.Column<decimal>(nullable: false),
                    TotalSales = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Products_MostPopularProductId",
                        column: x => x.MostPopularProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Products_MostPopularProductInLastDaysId",
                        column: x => x.MostPopularProductInLastDaysId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Products_MostPopularProductInLastMonthId",
                        column: x => x.MostPopularProductInLastMonthId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MostPopularProductId",
                table: "Reports",
                column: "MostPopularProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MostPopularProductInLastDaysId",
                table: "Reports",
                column: "MostPopularProductInLastDaysId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MostPopularProductInLastMonthId",
                table: "Reports",
                column: "MostPopularProductInLastMonthId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
