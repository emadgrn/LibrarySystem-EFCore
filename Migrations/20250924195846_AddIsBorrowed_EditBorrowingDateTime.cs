using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HW12.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBorrowed_EditBorrowingDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BorrowedDate",
                table: "BorrowedBooks",
                newName: "BorrowingDateTime");

            migrationBuilder.AddColumn<bool>(
                name: "IsBorrowed",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBorrowed",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "BorrowingDateTime",
                table: "BorrowedBooks",
                newName: "BorrowedDate");
        }
    }
}
