namespace Data.Models
{
    public class Keyword : BaseModel
    {
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
    }
}
