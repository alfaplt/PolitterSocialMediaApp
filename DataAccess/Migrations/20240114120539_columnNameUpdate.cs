using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class columnNameUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowerId",
                table: "Follows");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "Follows",
                newName: "FollowedId");

            migrationBuilder.RenameIndex(
                name: "IX_Follows_FollowerId",
                table: "Follows",
                newName: "IX_Follows_FollowedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedId",
                table: "Follows",
                column: "FollowedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedId",
                table: "Follows");

            migrationBuilder.RenameColumn(
                name: "FollowedId",
                table: "Follows",
                newName: "FollowerId");

            migrationBuilder.RenameIndex(
                name: "IX_Follows_FollowedId",
                table: "Follows",
                newName: "IX_Follows_FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowerId",
                table: "Follows",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
