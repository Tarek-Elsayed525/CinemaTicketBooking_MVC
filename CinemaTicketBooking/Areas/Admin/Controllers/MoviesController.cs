using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaTicketBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        //readonly private ApplicationDbContext _Context = new ApplicationDbContext();
        readonly private IReposetory<Movie> _movieReposetory;     //= new Reposetory<Movie>();
        readonly private IReposetory<Actor> _actorReposetory;     // = new Reposetory<Actor>();
        readonly private IReposetory<Cinema> _cinemaReposetory;    // = new Reposetory<Cinema>();
        readonly private IReposetory<Category> _categoryReposetory;    // = new Reposetory<Category>();
        readonly private IReposetory<MovieActor> _movieactorReposetory;    // = new Reposetory<MovieActor>();
        readonly private IReposetory<MovieSubImage> _moviesubimageReposetory;    //= new Reposetory<MovieSubImage>();

        public MoviesController(IReposetory<Movie> movieReposetory, IReposetory<Actor> actorReposetory, IReposetory<Cinema> cinemaReposetory, IReposetory<Category> categoryReposetory, IReposetory<MovieActor> movieactorReposetory, IReposetory<MovieSubImage> moviesubimageReposetory)
        {
            _movieReposetory = movieReposetory;
            _actorReposetory = actorReposetory;
            _cinemaReposetory = cinemaReposetory;
            _categoryReposetory = categoryReposetory;
            _movieactorReposetory = movieactorReposetory;
            _moviesubimageReposetory = moviesubimageReposetory;
        }

        public IActionResult Index(int page = 1)
        {

            int pageSize = 5;
            //int totalMovies = _Context.Movies.Count();
            int totalMovies = _movieReposetory.Query().Count();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalMovies / pageSize);
            if (page > ViewBag.TotalPages) page = ViewBag.TotalPages;
            if (page < 1) page = 1;


            //var movies = _Context.Movies.Include(m => m.Category).Include(m => m.Cinema).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var movies = _movieReposetory.Query(includes: [m => m.Category, m => m.Cinema]).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return View(movies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CinemaTicketBooking.View_Model.MovieFormVM
            {
                Categories = _categoryReposetory.Query().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),

                Cinemas = _cinemaReposetory.Query().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),


                Actors = _actorReposetory.Query().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList()
            };
            return View(viewModel);

        }

        [HttpPost]

        public async Task<IActionResult> Create(CinemaTicketBooking.View_Model.MovieFormVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _categoryReposetory.Query().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                viewModel.Cinemas = _cinemaReposetory.Query().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                viewModel.Actors = _actorReposetory.Query().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();
                return View(viewModel);
            }
            string fileName = string.Empty;
            if (viewModel.MainImage != null && viewModel.MainImage.Length > 0)
            {
                fileName = Guid.NewGuid().ToString() + "-" + viewModel.MainImage.FileName;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/moviesImages", fileName);
                using (var stream = System.IO.File.Create(filepath))
                {
                    viewModel.MainImage.CopyTo(stream);
                }

            }
            var movie = new CinemaTicketBooking.Models.Movie
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                ReleaseDate = viewModel.ReleaseDate,
                Status = viewModel.Status,
                CategoryId = viewModel.CategoryId,
                CinemaId = viewModel.CinemaId,
                MainImage = fileName
            };
            //_Context.Movies.Add(movie);
            await _movieReposetory.AddAsync(movie);
            //_Context.SaveChanges();
            await _movieReposetory.SaveAsync();

            if (viewModel.SubImages != null && viewModel.SubImages.Count > 0)
            {
                foreach (var subImage in viewModel.SubImages)
                {
                    if (subImage != null && subImage.Length > 0)
                    {
                        var SubFileNames = Guid.NewGuid().ToString() + "-" + subImage.FileName;
                        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MoviesSubImages", SubFileNames);
                        using (var stream = System.IO.File.Create(filepath))
                        {
                            subImage.CopyTo(stream);
                        }
                        var movieSubImage = new CinemaTicketBooking.Models.MovieSubImage
                        {
                            SubImage = SubFileNames,
                            MovieId = movie.Id
                        };
                        //_Context.MovieSubImages.Add(movieSubImage);
                        await _moviesubimageReposetory.AddAsync(movieSubImage);
                    }
                }
                await _moviesubimageReposetory.SaveAsync();
            }

            if (viewModel.SelectedActorIds != null && viewModel.SelectedActorIds.Count > 0)
            {
                foreach (var actorId in viewModel.SelectedActorIds)
                {
                    var movieActor = new CinemaTicketBooking.Models.MovieActor
                    {
                        MovieId = movie.Id,
                        ActorId = actorId
                    };
                    //_Context.MovieActors.Add(movieActor);
                    await _movieactorReposetory.AddAsync(movieActor);
                }
                //_Context.SaveChanges();
                await _movieactorReposetory.SaveAsync();
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var movie = _Context.Movies.Include(m => m.MovieActors).Include(s => s.MoviesSubImages).FirstOrDefault(m => m.Id == id);
            var movie = await _movieReposetory.GetOneAsync(m => m.Id == id, [m => m.MovieActors, s => s.MoviesSubImages]);
            if (movie == null)
            {
                return NotFound();
            }
            var viewModel = new CinemaTicketBooking.View_Model.MovieFormVM
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Price = movie.Price,
                ReleaseDate = movie.ReleaseDate,
                Status = movie.Status,
                CategoryId = movie.CategoryId,
                CinemaId = movie.CinemaId,

                ExistingMainImage = movie.MainImage,
                ExistingSubImages = movie.MoviesSubImages.ToList(),

                Categories = _categoryReposetory.Query().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                Cinemas = _cinemaReposetory.Query().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                Actors = _actorReposetory.Query().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList(),
                SelectedActorIds = movie.MovieActors.Select(ma => ma.ActorId).ToList()
            };
            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CinemaTicketBooking.View_Model.MovieFormVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _categoryReposetory.Query().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                viewModel.Cinemas = _cinemaReposetory.Query().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                viewModel.Actors = _actorReposetory.Query().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                }).ToList();
                return View(viewModel);
            }
            //var movie = _Context.Movies.Include(m => m.MovieActors).Include(s => s.MoviesSubImages).FirstOrDefault(m => m.Id == viewModel.Id);
            var movie = await _movieReposetory.GetOneAsync(m => m.Id == viewModel.Id, [m => m.MovieActors, s => s.MoviesSubImages]);
            if (movie == null)
            {
                return NotFound();
            }
            movie.Name = viewModel.Name;
            movie.Description = viewModel.Description;
            movie.Price = viewModel.Price;
            movie.ReleaseDate = viewModel.ReleaseDate;
            movie.Status = viewModel.Status;
            movie.CategoryId = viewModel.CategoryId;
            movie.CinemaId = viewModel.CinemaId;

            if (viewModel.MainImage != null && viewModel.MainImage.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + "-" + viewModel.MainImage.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/moviesImages", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.MainImage.CopyTo(stream);
                }

                if (!string.IsNullOrEmpty(viewModel.ExistingMainImage))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/moviesImages", viewModel.ExistingMainImage);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                movie.MainImage = fileName;
            }
            else
            {
                movie.MainImage = viewModel.ExistingMainImage;
            }

            //var existingSubImages = _Context.MovieSubImages.Where(s => s.MovieId == movie.Id).ToList();

            var existingSubImages = _moviesubimageReposetory.Query(s => s.MovieId == movie.Id).ToList();

            var keptSubImageIds = viewModel.ExistingSubImageIds ?? new List<int>();

            var subImagesToRemove = existingSubImages.Where(s => !keptSubImageIds.Contains(s.Id)).ToList();

            foreach (var subImg in subImagesToRemove)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MoviesSubImages", subImg.SubImage);
                System.Diagnostics.Debug.WriteLine("Trying to delete file at: " + filePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _moviesubimageReposetory.Remove(subImg);
            }

            if (viewModel.SubImages != null && viewModel.SubImages.Count > 0)
            {
                foreach (var subFile in viewModel.SubImages)
                {
                    if (subFile.Length > 0)
                    {
                        string subFileName = Guid.NewGuid().ToString() + "-" + subFile.FileName;
                        var subFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MoviesSubImages", subFileName);

                        using (var stream = new FileStream(subFilePath, FileMode.Create))
                        {
                            subFile.CopyTo(stream);
                        }
                        var movieSubImage = new CinemaTicketBooking.Models.MovieSubImage
                        {
                            MovieId = movie.Id,
                            SubImage = subFileName
                        };
                        await _moviesubimageReposetory.AddAsync(movieSubImage);
                    }
                }

                await _moviesubimageReposetory.SaveAsync();
            }

            var existingActorIds = movie.MovieActors.Select(ma => ma.ActorId).ToList();
            var selectedActorIds = viewModel.SelectedActorIds ?? new List<int>();

            var actorsToAdd = selectedActorIds.Except(existingActorIds).ToList();
            foreach (var actorId in actorsToAdd)
            {
                var mvactor = new CinemaTicketBooking.Models.MovieActor
                {
                    MovieId = movie.Id,
                    ActorId = actorId
                };

                await _movieactorReposetory.AddAsync(mvactor);

                //_Context.MovieActors.Add(new CinemaTicketBooking.Models.MovieActor
                //{
                //    MovieId = movie.Id,
                //    ActorId = actorId
                //});
            }

            var actorsToRemove = movie.MovieActors.Where(ma => !selectedActorIds.Contains(ma.ActorId)).ToList();
            if (actorsToRemove.Any())
            {
                //_Context.MovieActors.RemoveRange(actorsToRemove);
                _movieactorReposetory.RemoveRange(actorsToRemove);
            }

            //_Context.SaveChanges();
            await _movieactorReposetory.SaveAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            //var movie = _Context.Movies.Include(m => m.MovieActors).Include(s => s.MoviesSubImages).FirstOrDefault(m => m.Id == id);
            var movie = await _movieReposetory.GetOneAsync(m => m.Id == id, [m => m.MovieActors, s => s.MoviesSubImages]);
            if (movie == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(movie.MainImage))
            {
                var mainImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/moviesImages", movie.MainImage);
                if (System.IO.File.Exists(mainImagePath))
                {
                    System.IO.File.Delete(mainImagePath);
                }
            }
            foreach (var subImg in movie.MoviesSubImages)
            {
                var subImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MoviesSubImages", subImg.SubImage);
                if (System.IO.File.Exists(subImagePath))
                {
                    System.IO.File.Delete(subImagePath);
                }
            }
            //_Context.MovieActors.RemoveRange(movie.MovieActors);
            _movieactorReposetory.RemoveRange(movie.MovieActors);
            //_Context.MovieSubImages.RemoveRange(movie.MoviesSubImages);
            _moviesubimageReposetory.RemoveRange(movie.MoviesSubImages);
            //_Context.Movies.Remove(movie);
            _movieReposetory.Remove(movie);
            //_Context.SaveChanges();
            await _movieReposetory.SaveAsync();
            return RedirectToAction("Index");
        }

    }
}
