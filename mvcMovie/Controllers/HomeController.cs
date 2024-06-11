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
        public async Task<IActionResult> Index(string MovieTitle, int? minMovieTime, int? maxMovieTime, string priceOrder)
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
                ViewData["titleMovie"] = MovieTitle;
            }

            if (minMovieTime.HasValue)
            {

                if (minMovieTime.HasValue && maxMovieTime.HasValue)
                {
                    movies = movies.Where(s => s.MovieTime >= minMovieTime && s.MovieTime <= maxMovieTime);
                    ViewData["minMovieTime"] = minMovieTime;
                    ViewData["maxMovieTime"] = maxMovieTime;
                }
                else if (minMovieTime.HasValue)
                {
                    movies = movies.Where(s => s.MovieTime >= minMovieTime);
                    ViewData["minMovieTime"] = minMovieTime;
                }
                else if(maxMovieTime < minMovieTime)
                {
                    return View(await movies.ToListAsync());
                }
              
            }

            if(!String.IsNullOrEmpty(priceOrder))
            {
                switch (priceOrder) 
                {
                    case "Low2High":
                        movies = movies.OrderBy(m => m.price);
                        break;
                    case "High2Low":
                        movies = movies.OrderByDescending(m => m.price);
                        break;
                }
                ViewData["priceOrder"] = priceOrder;
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
