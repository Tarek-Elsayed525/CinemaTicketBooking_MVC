using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class add_data_Actors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Actors (Name, Description, Image) values ('Karim Abdel Aziz', 'Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis. Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci.', '/ActorImages/1.png');insert into Actors (Name, Description, Image) values ('Ahmed Ezz', 'Duis bibendum. Morbi non quam nec dui luctus rutrum.', '/ActorImages/2.png');insert into Actors (Name, Description, Image) values ('Ahmed Helmy', 'Nullam varius. Nulla facilisi. Cras non velit nec nisi vulputate nonummy. Maecenas tincidunt lacus at velit. Vivamus vel nulla eget eros elementum pellentesque.', '/ActorImages/3.png');insert into Actors (Name, Description, Image) values ('Menna Shalaby', 'Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis.', '/ActorImages/4.png');insert into Actors (Name, Description, Image) values ('Amr Youssef', 'Curabitur gravida nisi at nibh. In hac habitasse platea dictumst.', '/ActorImages/5.png');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Actors");
        }
    }
}
