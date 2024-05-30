namespace CryptoWorld.News.Data.Models
{
    public class ArticleKeyword
    {
        public Guid ArticleId { get; set; }
        public virtual Article Article { get; set; }
        public Guid KeywordId { get; set; }
        public virtual Keyword Keyword { get; set; }
    }
}