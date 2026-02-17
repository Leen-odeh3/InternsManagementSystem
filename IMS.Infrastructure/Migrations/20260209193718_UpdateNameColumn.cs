using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_AspNetUsers_Id",
                table: "Trainees");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_AspNetUsers_Id",
                table: "Trainers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Trainers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Trainees",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_AspNetUsers_UserId",
                table: "Trainees",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_AspNetUsers_UserId",
                table: "Trainers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_AspNetUsers_UserId",
                table: "Trainees");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_AspNetUsers_UserId",
                table: "Trainers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Trainers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Trainees",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_AspNetUsers_Id",
                table: "Trainees",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_AspNetUsers_Id",
                table: "Trainers",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
