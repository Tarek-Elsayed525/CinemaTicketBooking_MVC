using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class add_data_Movie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Movies (Name, Description, MainImage, Status, Price, ReleaseDate, CategoryId, CinemaId) VALUES ('Inception', 'A thief who steals corporate secrets through the use of dream-sharing technology.', '/moviesImages/1.png', 1, 150.00, '2025-03-15', 1, 1),('The Dark Knight', 'When the menace known as the Joker wreaks havoc and chaos on the people of Gotham.', '/moviesImages/2.png', 1, 180.00, '2025-07-20', 2, 2),('Interstellar', 'A team of explorers travel through a wormhole in space to ensure survival.', '/moviesImages/3.png', 1, 200.00, '2025-11-10', 3, 3),('The Matrix', 'A computer hacker learns from mysterious rebels about the true nature of his reality.', '/moviesImages/4.png', 1, 160.00, '2026-02-01', 4, 4),('Joker', 'In Gotham City, mentally troubled comedian Arthur Fleck embarks on a downward spiral.', '/moviesImages/5.png', 1, 190.00, '2026-06-18', 5, 5);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Movies");
        }
    }
}
