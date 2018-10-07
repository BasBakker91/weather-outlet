using System;

namespace WeatherOutlet.Shared.News
{
    public partial class Article
    {
        public Source Source { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
        public Uri UrlToImage { get; set; }
        public DateTimeOffset PublishedAt { get; set; }
        public string Content { get; set; }
    }
}
