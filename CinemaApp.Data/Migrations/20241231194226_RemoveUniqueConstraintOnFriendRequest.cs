using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstraintOnFriendRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FriendRequests_ReceiverId_SenderId",
                table: "FriendRequests");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FriendRequests_ReceiverId",
                table: "FriendRequests");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverId_SenderId",
                table: "FriendRequests",
                columns: new[] { "ReceiverId", "SenderId" },
                unique: true);
        }
    }
}
