namespace SPNL_Tema_NewsOriginSearch_API.Models
{
    public class NewsArticle
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        //public string ThumbnailUrl { get; set; }
        public string Provider { get; set; }
        public string DatePublished { get; set; }
        public string Category { get; set; }
    }
}