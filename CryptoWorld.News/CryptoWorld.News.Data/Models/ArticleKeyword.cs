namespace CryptoWorld.News.Data.Models
{
    public class ArticleKeyword
    {
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public int KeywordId { get; set; }

        public virtual Keyword Keyword { get; set; }
    }
}