namespace Data.Models
{
    public class Keyword : BaseModel<int>
    {
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
    }
}
