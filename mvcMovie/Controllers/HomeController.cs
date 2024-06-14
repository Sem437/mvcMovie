using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        
        // GET: Movies
        public async Task<IActionResult> Index(string MovieTitle, int? minMovieTime, int? maxMovieTime, string timeOrder, 
            int? releaseYear, string releaseOrder, string genreFilter)
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
                    movies = movies.Where(s => s.MovieTime == minMovieTime);
                    ViewData["minMovieTime"] = minMovieTime;
                }
                else if(maxMovieTime < minMovieTime)
                {
                    return View(await movies.ToListAsync());
                }
              
            }

            if(!String.IsNullOrEmpty(timeOrder))
            {
                switch (timeOrder) 
                {
                    case "Low2High":
                        movies = movies.OrderBy(m => m.MovieTime);
                        break;
                    case "High2Low":
                        movies = movies.OrderByDescending(m => m.MovieTime);
                        break;
                }
                ViewData["timeOrder"] = timeOrder;
            }

            if(releaseYear.HasValue)
            {
                var releaseYearString = releaseYear.Value.ToString();
                movies = movies.Where(s => s.ReleaseData.ToString().Contains(releaseYearString)); //Contains = %releaseYear%
                ViewData["releaseYear"] = releaseYear;
            }

            if(!String.IsNullOrEmpty(releaseOrder))
            {
                switch(releaseOrder) 
                {
                    case "new2old":
                        movies = movies.OrderByDescending(m => m.ReleaseData);
                        break;
                    case "old2new":
                        movies = movies.OrderBy(m => m.ReleaseData);
                        break;
                }
                ViewData["releaseOrder"] = releaseOrder;
            }

            if(!String.IsNullOrEmpty(genreFilter))
            {
                var genreFilterString = genreFilter.ToString();

                switch(genreFilter)
                {
                    case "Action":
                        movies = movies.Where(s => s.Genre.ToString().Contains(genreFilterString));
                        break;
                    case "Crime":
                        movies = movies.Where(s => s.Genre.ToString().Contains(genreFilterString));
                        break;
                    case "Comedy":
                        movies = movies.Where(s => s.Genre.ToString().Contains(genreFilterString));
                        break;
                    case "Science Fiction":
                        movies = movies.Where(s => s.Genre.ToString().Contains(genreFilterString));
                        break;
                }
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
