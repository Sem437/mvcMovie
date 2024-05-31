using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mvcMovie.Models.Movie;

namespace mvcMovie.Data
{
    public class mvcMovieContext : DbContext
    {
        public mvcMovieContext (DbContextOptions<mvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<mvcMovie.Models.Movie.Movie> Movie { get; set; } = default!;
    }
}
