using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CinemaTicketBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        //readonly private ApplicationDbContext _Context = new ApplicationDbContext();
        readonly private IReposetory<Movie> _movieReposetory;
        readonly private IReposetory<Cinema> _cinemaReposetory;    // = new Reposetory<Cinema>();
        readonly private IReposetory<Category> _categoryReposetory;    // = new Reposetory<Category>();
        readonly private IReposetory<Actor> _actorReposetory;     // = new Reposetory<Actor>();

        public HomeController(IReposetory<Movie> movieReposetory, IReposetory<Cinema> cinemaReposetory, IReposetory<Category> categoryReposetory, IReposetory<Actor> actorReposetory)
        {
            _movieReposetory = movieReposetory;
            _cinemaReposetory = cinemaReposetory;
            _categoryReposetory = categoryReposetory;
            _actorReposetory = actorReposetory;
        }

        public IActionResult Index()
        {

            ViewBag.CinemasCount = _cinemaReposetory.Query().Count();
            ViewBag.MoviesCount = _movieReposetory.Query().Count();
            ViewBag.CategoriesCount = _categoryReposetory.Query().Count();
            ViewBag.ActorsCount = _actorReposetory.Query().Count();

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
