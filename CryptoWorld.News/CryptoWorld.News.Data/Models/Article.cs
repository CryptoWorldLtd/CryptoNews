using CryptoWorld.News.Data.Models.Common;

namespace CryptoWorld.News.Data.Models
{
    public class Article : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PublicationDate { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public Guid SourceId { get; set; }
        public virtual Source Source { get; set; }
        public virtual List<Comment> Comments { get; set; } = [];
        public virtual List<ArticleKeyword> Keywords { get; set; } =[];
    }
}