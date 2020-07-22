
namespace TestWS.Models.Tickets
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinAge { get; set; }
        public int Duration { get; set; }
        public float Rating { get; set; }
        public Type[] Types { get; set; }
        public Genre[] Genres { get; set; }
        public string ImageURL { get; set; }
    }
}