using CryptoWorld.News.Data.Models.Common;

namespace CryptoWorld.News.Data.Models
{
    public class Article : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublicationDate { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int SourceId { get; set; }

        public virtual Source Source { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<ArticleKeyword> Keywords { get; set; } = new HashSet<ArticleKeyword>();
    }
}

