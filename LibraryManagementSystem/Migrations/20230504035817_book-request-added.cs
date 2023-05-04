using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    public partial class bookrequestadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoverId",
                table: "BookDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BookDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdded",
                table: "BookDetails",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestedById",
                table: "BookDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookDetails_RequestedById",
                table: "BookDetails",
                column: "RequestedById");

            migrationBuilder.AddForeignKey(
                name: "FK_BookDetails_AspNetUsers_RequestedById",
                table: "BookDetails",
                column: "RequestedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookDetails_AspNetUsers_RequestedById",
                table: "BookDetails");

            migrationBuilder.DropIndex(
                name: "IX_BookDetails_RequestedById",
                table: "BookDetails");

            migrationBuilder.DropColumn(
                name: "CoverId",
                table: "BookDetails");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BookDetails");

            migrationBuilder.DropColumn(
                name: "IsAdded",
                table: "BookDetails");

            migrationBuilder.DropColumn(
                name: "RequestedById",
                table: "BookDetails");
        }
    }
}
