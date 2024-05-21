namespace Data.Models
{
    public class Category
    {
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
    }
}