using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemasController : Controller
    {

        //readonly private ApplicationDbContext _Context = new ApplicationDbContext();
        readonly private Reposetory<Cinema> _cinemaReposetory;//= new Reposetory<Cinema>();

        public CinemasController(Reposetory<Cinema> cinemaReposetory)
        {
            _cinemaReposetory = cinemaReposetory;
        }

        public IActionResult Index(int page = 1)
        {
            int pageSize = 5;
            //int totalCinemas = _Context.Cinemas.Count();
            int totalCinemas = _cinemaReposetory.Query().Count();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCinemas / pageSize);

            //var cinemas = _Context.Cinemas.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var cinemas = _cinemaReposetory.Query().Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(cinemas);
        }




        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cinema cinema, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }
            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CinemaImages", fileName);
                using (var stream = System.IO.File.Create(filepath))
                {
                    image.CopyTo(stream);
                }
                cinema.Image = fileName;
            }
            //_Context.Cinemas.Add(cinema);
            await _cinemaReposetory.AddAsync(cinema);


            //_Context.SaveChanges();
            await _cinemaReposetory.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var cinema = _Context.Cinemas.FirstOrDefault(c => c.Id == id);
            var cinema = await _cinemaReposetory.GetOneAsync(c => c.Id == id);
            if (cinema == null)
            {
                return NotFound();
            }
            return View(cinema);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Cinema cinema, IFormFile? image)
        {
            //var oldCinema = _Context.Cinemas.AsNoTracking().FirstOrDefault(c => c.Id == cinema.Id);
            var oldCinema = await _cinemaReposetory.GetOneAsync(c => c.Id == cinema.Id, isTracked: false);
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }
            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CinemaImages", fileName);
                using (var stream = System.IO.File.Create(filepath))
                {
                    image.CopyTo(stream);
                }
                cinema.Image = fileName;
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CinemaImages", oldCinema.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            else
            {
                cinema.Image = oldCinema.Image;
            }
            //_Context.Cinemas.Update(cinema);
            _cinemaReposetory.Update(cinema);
            //_Context.SaveChanges();
            await _cinemaReposetory.SaveAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            //var cinema = _Context.Cinemas.FirstOrDefault(c => c.Id == id);
            var cinema = await _cinemaReposetory.GetOneAsync(c => c.Id == id);
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CinemaImages", cinema.Image);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            if (cinema == null)
            {
                return NotFound();
            }
            //_Context.Cinemas.Remove(cinema);
            _cinemaReposetory.Remove(cinema);
            //_Context.SaveChanges();
            await _cinemaReposetory.SaveAsync();
            return RedirectToAction("Index");

        }
    }
}
