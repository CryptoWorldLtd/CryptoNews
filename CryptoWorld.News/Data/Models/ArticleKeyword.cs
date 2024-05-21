namespace Data.Models
{
    public class ArticleKeyword
    {
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; } = null!;

        public int KeywordId { get; set; }

        public virtual Keyword Keyword { get; set; } = null!;
    }
}