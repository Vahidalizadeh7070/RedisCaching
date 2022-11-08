namespace RedisCaching.Models
{
    public class News
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public string Author { get; set; }
        public DateTime PubDate { get; set; }

        public News()
        {
            Id = "";
            Title = "";
            About = "";
            Author = "";
            PubDate = DateTime.Today;
        }
    }
}