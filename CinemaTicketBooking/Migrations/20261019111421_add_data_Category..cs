using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class add_data_Category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Categorys (Name, Description, Status) VALUES ('Action', 'High-octane movies filled with stunts, chases, and excitement.', 1),('Drama', 'Emotional and character-driven storytelling dealing with realistic themes.', 1),('Comedy', 'Lighthearted films designed to make audiences laugh through humorous situations.', 1),('Horror', 'Spine-chilling films intended to frighten, scare, and startle audiences.', 1),('Sci-Fi', 'Futuristic and imaginative films involving science, space, and technology.', 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categorys");
        }
    }
}
