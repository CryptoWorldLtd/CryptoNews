namespace Data.Models
{
    public class Article : BaseModel<int>
    {
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public DateTime PublicationDate { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public int SourceId { get; set; }

        public virtual Source Source { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<ArticleKeyword> Keywords { get; set; } = new HashSet<ArticleKeyword>();
    }
}
