namespace mvcMovie.Models.Movie
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int MovieTime { get; set; }
        public DateOnly ReleaseData { get; set; }
        public string Genre { get; set; }
        public decimal price { get; set; }
        public string ImgLink { get; set; }
        public string Description { get; set; }
        public string? movieFile { get; set; }
    }
}
