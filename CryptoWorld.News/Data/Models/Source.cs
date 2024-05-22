namespace Data.Models
{
    public class Source : BaseModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
    }
}