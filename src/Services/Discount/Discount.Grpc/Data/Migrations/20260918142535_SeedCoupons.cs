using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Discount.Grpc.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedCoupons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Amount", "Description", "ProductName" },
                values: new object[,]
                {
                    { 1, 10000, "Discounted on Samsung newly launch z fold smartphone", "Samsung Galaxy Z Fold" },
                    { 2, 15000, "Discounted on Samsung newly launch z fold tri smartphone", "Samsung Galaxy Z Fold Tri" },
                    { 3, 12000, "Discounted on apple newly launch iphone duo smartphone", "Iphone Duo" },
                    { 4, 6000, "Discounted on apple newly launch iphone 18 pro max smartphone", "Iphone 18 pro max" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
