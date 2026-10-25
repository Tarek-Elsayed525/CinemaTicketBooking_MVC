using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class add_data_Cinema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Cinemas (Name, Description, Image) VALUES ('Arkan Plaza Cinema', 'Located in Sheikh Zayed, featuring IMAX screens and luxury lounges.', '/Cinemalmages/1.png');INSERT INTO Cinemas (Name, Description, Image) VALUES ('City Stars Cinema', 'Located in Nasr City, one of the biggest cinema complexes in Cairo.', '/Cinemalmages/2.png');INSERT INTO Cinemas (Name, Description, Image) VALUES ('Galaxy Cinema', 'Located in El Manial, offering a great viewing experience with modern sound systems.', '/Cinemalmages/3.png');INSERT INTO Cinemas (Name, Description, Image) VALUES ('Mall of Arabia Cinema', 'Located in 6th of October, featuring multiple screens and VIP halls.', '/Cinemalmages/4.png');INSERT INTO Cinemas (Name, Description, Image) VALUES ('Point 90 Cinema', 'Located in New Cairo near AUC, featuring premium 4DX and standard screens.', '/Cinemalmages/5.png');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Cinemas");
        }
    }
}
