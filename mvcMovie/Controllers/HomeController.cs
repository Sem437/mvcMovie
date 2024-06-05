using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcMovie.Data;
using mvcMovie.Models;
using System.Diagnostics;

namespace mvcMovie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly  mvcMovieContext _context;
        public HomeController(ILogger<HomeController> logger, mvcMovieContext context)
        {
            _logger = logger;
            _context = context;
        }
        /*
        public IActionResult Index()
        {
            return View();
        }
        */
        // GET: Movies
        public async Task<IActionResult> Index(string MovieTitle)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var movies = from m in _context.Movie
                         select m;

            if (!String.IsNullOrEmpty(MovieTitle))
            {
                movies = movies.Where(s => s.Title!.Contains(MovieTitle));
            }

            return View(await movies.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
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
