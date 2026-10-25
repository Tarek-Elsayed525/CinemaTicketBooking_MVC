using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CinemaTicketBooking.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        readonly private ApplicationDbContext _Context;//= new ApplicationDbContext();

        readonly private IReposetory<Movie> _movieReposetory;
        readonly private IReposetory<Cinema> _cinemaReposetory;

        public HomeController(IReposetory<Movie> movieReposetory, IReposetory<Cinema> cinemaReposetory, ApplicationDbContext context)
        {
            _movieReposetory = movieReposetory;
            _cinemaReposetory = cinemaReposetory;
            _Context = context;
        }

        public async Task<IActionResult> IndexAsync(int page = 1)
        {
            int pagesize = 6;
            int totalMovies = _cinemaReposetory.Query().Count();

            //var cinemas = _Context.Cinemas.ToList();
            var cinemas = await _cinemaReposetory.GetAllAsync();

            cinemas = cinemas.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalMovies / (double)pagesize);
            if (ViewBag.CurrentPage == 0)
            {
                ViewBag.CurrentPage = 1;
            }
            if (ViewBag.CurrentPage > ViewBag.TotalPages)
            {
                ViewBag.CurrentPage = ViewBag.TotalPages;
            }

            return View(cinemas);
        }
        public async Task<IActionResult> CinemaMoviesAsync(int id, int page = 1)
        {
            int pagesize = 6;
            int totalMovies = _movieReposetory.Query().Count(m => m.CinemaId == id);
            //var cinema = _Context.Cinemas.FirstOrDefault(c => c.Id == id);
            var cinema = await _cinemaReposetory.GetOneAsync(c => c.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }
            //var movies = _Context.Movies.Include(m => m.Category).Where(m => m.CinemaId == id).ToList();

            var movies = await _movieReposetory.GetAllAsync(m => m.CinemaId == id, includes: [c => c.Category]);
            ViewBag.CinemaName = cinema?.Name;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalMovies / (double)pagesize);

            if (ViewBag.CurrentPage == 0)
            {
                ViewBag.CurrentPage = 1;
            }
            if (ViewBag.CurrentPage > ViewBag.TotalPages)
            {
                ViewBag.CurrentPage = ViewBag.TotalPages;
            }
            return View(movies);
        }
        public async Task<IActionResult> MovieDetails(int id)
        {
            var movie = _Context.Movies.Include(m => m.Category).Include(c => c.Cinema).Include(a => a.MovieActors).ThenInclude(ma => ma.Actor).FirstOrDefault(m => m.Id == id);
            //var movie = await _movieReposetory.GetOneAsync(m => m.Id == id, [m => m.Category, c => c.Cinema, a => a.MovieActors]);



            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
