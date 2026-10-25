using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class add_data_MovieActor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO MovieActors (MovieId, ActorId) VALUES (2, 1), (2, 2),(3, 2), (3, 3),(4, 1), (4, 4),(5, 3), (5, 5),(6, 4), (6, 5);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM MovieActors");
        }
    }
}
