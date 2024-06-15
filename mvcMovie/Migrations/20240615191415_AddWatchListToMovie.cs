using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvcMovie.Migrations
{
    /// <inheritdoc />
    public partial class AddWatchListToMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "watchList",
                table: "Movie",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "watchList",
                table: "Movie");
        }
    }
}
