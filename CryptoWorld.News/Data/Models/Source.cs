namespace Data.Models
{
    public class Source : BaseModel<int>
    {
        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
    }
}