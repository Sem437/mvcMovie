using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvcMovie.Data;
using mvcMovie.Models.Movie;

namespace mvcMovie.Controllers.Movies
{
   
    public class MoviesController : Controller
    {
        private readonly mvcMovieContext _context;

        public MoviesController(mvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        [Authorize(Roles = "AdminOnly")]
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
                else if (maxMovieTime < minMovieTime)
                {
                    return View(await movies.ToListAsync());
                }

            }

            if (!String.IsNullOrEmpty(timeOrder))
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

            if (releaseYear.HasValue)
            {
                var releaseYearString = releaseYear.Value.ToString();
                movies = movies.Where(s => s.ReleaseData.ToString().Contains(releaseYearString)); //Contains = %releaseYear%
                ViewData["releaseYear"] = releaseYear;
            }

            if (!String.IsNullOrEmpty(releaseOrder))
            {
                switch (releaseOrder)
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

            if (!String.IsNullOrEmpty(genreFilter))
            {
                var genreFilterString = genreFilter.ToString();

                switch (genreFilter)
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

        // GET: Movies/Details/5
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

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,MovieTime,ReleaseData,Genre,price,ImgLink,Description")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,MovieTime,ReleaseData,Genre,price,ImgLink,Description")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5      
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
       
    }
}