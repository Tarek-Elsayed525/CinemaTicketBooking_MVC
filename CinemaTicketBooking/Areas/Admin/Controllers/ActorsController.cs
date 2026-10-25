using Microsoft.AspNetCore.Mvc;


namespace CinemaTicketBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorsController : Controller
    {
        //readonly private ApplicationDbContext _Context = new ApplicationDbContext();
        readonly private IReposetory<Actor> _actorReposetory; //= new Reposetory<Actor>();

        public ActorsController(IReposetory<Actor> actorReposetory)
        {
            _actorReposetory = actorReposetory;
        }


        public IActionResult Index(int page = 1)
        {
            int pageSize = 5;
            //int totalActors = _Context.Actors.Count();
            int totalActors = _actorReposetory.Query().Count();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalActors / pageSize);
            var actors = _actorReposetory.Query().Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(actors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Actor actor, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ActorImages", fileName);
                using (var stream = System.IO.File.Create(filepath))
                {
                    image.CopyTo(stream);
                }
                actor.Image = fileName;
            }
            //_Context.Actors.Add(actor);
            await _actorReposetory.AddAsync(actor);
            //_Context.SaveChanges();
            await _actorReposetory.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var actor = _Context.Actors.FirstOrDefault(a => a.Id == id);
            var actor = await _actorReposetory.GetOneAsync(a => a.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Actor actor, IFormFile image)
        {
            //var oldactor = _Context.Actors.AsNoTracking().FirstOrDefault(a => a.Id == actor.Id);
            var oldactor = await _actorReposetory.GetOneAsync(a => a.Id == actor.Id, isTracked: false);
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ActorImages", fileName);
                using (var stream = System.IO.File.Create(filepath))
                {
                    image.CopyTo(stream);
                }
                actor.Image = fileName;
                var oldactorpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ActorImages", oldactor.Image);
                if (System.IO.File.Exists(oldactorpath))
                {
                    System.IO.File.Delete(oldactorpath);
                }
                else
                {
                    actor.Image = oldactor.Image;
                }
            }

            //_Context.Actors.Update(actor);
            _actorReposetory.Update(actor);
            //_Context.SaveChanges();
            await _actorReposetory.SaveAsync();

            return RedirectToAction("Index");

        }


        public async Task<IActionResult> DeleteAsync(int id)
        {
            //var actor = _Context.Actors.FirstOrDefault(a => a.Id == id);
            var actor = await _actorReposetory.GetOneAsync(a => a.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            var actorpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ActorImages", actor.Image);
            if (System.IO.File.Exists(actorpath))
            {
                System.IO.File.Delete(actorpath);
            }
            //_Context.Actors.Remove(actor);
            _actorReposetory.Remove(actor);
            //_Context.SaveChanges();
            await _actorReposetory.SaveAsync();
            return RedirectToAction("Index");
        }



    }
}
