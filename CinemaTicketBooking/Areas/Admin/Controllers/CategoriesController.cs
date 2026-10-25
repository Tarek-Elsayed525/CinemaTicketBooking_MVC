using Microsoft.AspNetCore.Mvc;
namespace CinemaTicketBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        //readonly private ApplicationDbContext _Context = new ApplicationDbContext();
        readonly private IReposetory<Category> _categoryReposetory; //  = new Reposetory<Category>();


        public CategoriesController(IReposetory<Category> categoryReposetory)
        {
            _categoryReposetory = categoryReposetory;
        }



        public IActionResult Index(int page = 1)
        {
            int pagesize = 5;
            //int totalCategories = _Context.Categorys.Count();
            int totalCategories = _categoryReposetory.Query().Count();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCategories / pagesize);
            //var categories = _Context.Categorys.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            var categories = _categoryReposetory.Query().Skip((page - 1) * pagesize).Take(pagesize).ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Category category)
        {
            if (ModelState.IsValid)
            {
                //_Context.Categorys.Add(category);
                await _categoryReposetory.AddAsync(category);

                //_Context.SaveChanges();
                await _categoryReposetory.SaveAsync();

                return RedirectToAction("Index");
            }
            return View(category);
        }


        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            //var category = _Context.Categorys.FirstOrDefault(c => c.Id == id);
            var category = await _categoryReposetory.GetOneAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(Category category)
        {
            if (ModelState.IsValid)
            {
                //_Context.Categorys.Update(category);
                _categoryReposetory.Update(category);
                //_Context.SaveChanges();
                await _categoryReposetory.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        public async Task<IActionResult> DeleteAsync(int id)
        {
            //var category = _Context.Categorys.FirstOrDefault(c => c.Id == id);
            var category = await _categoryReposetory.GetOneAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            //_Context.Categorys.Remove(category);
            _categoryReposetory.Remove(category);
            //_Context.SaveChanges();
            await _categoryReposetory.SaveAsync();
            return RedirectToAction("Index");


        }

    }
}

