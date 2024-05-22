namespace Data.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
    }
}