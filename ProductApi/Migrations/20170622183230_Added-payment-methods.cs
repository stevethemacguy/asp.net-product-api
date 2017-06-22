using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProductApi.Migrations
{
    public partial class Addedpaymentmethods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodUsedId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "BillingAddressId",
                table: "PaymentMethods",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CardNumber",
                table: "PaymentMethods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardType",
                table: "PaymentMethods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomCardName",
                table: "PaymentMethods",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpirationDate",
                table: "PaymentMethods",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "IsValid",
                table: "PaymentMethods",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "PaymentMethods",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecurityCode",
                table: "PaymentMethods",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PaymentMethods",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillingAddressId = table.Column<int>(nullable: true),
                    CardNumber = table.Column<int>(nullable: false),
                    CardType = table.Column<int>(nullable: false),
                    CustomCardName = table.Column<string>(nullable: true),
                    ExpirationDate = table.Column<DateTimeOffset>(nullable: false),
                    IsValid = table.Column<string>(nullable: true),
                    SecurityCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethod_BillingAddresses_BillingAddressId",
                        column: x => x.BillingAddressId,
                        principalTable: "BillingAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_BillingAddressId",
                table: "PaymentMethods",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_PaymentMethodId",
                table: "PaymentMethods",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_BillingAddressId",
                table: "PaymentMethod",
                column: "BillingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethod_PaymentMethodUsedId",
                table: "Orders",
                column: "PaymentMethodUsedId",
                principalTable: "PaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_BillingAddresses_BillingAddressId",
                table: "PaymentMethods",
                column: "BillingAddressId",
                principalTable: "BillingAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_PaymentMethod_PaymentMethodId",
                table: "PaymentMethods",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethod_PaymentMethodUsedId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_BillingAddresses_BillingAddressId",
                table: "PaymentMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_PaymentMethod_PaymentMethodId",
                table: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_BillingAddressId",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_PaymentMethodId",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "CustomCardName",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "SecurityCode",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PaymentMethods");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodUsedId",
                table: "Orders",
                column: "PaymentMethodUsedId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
