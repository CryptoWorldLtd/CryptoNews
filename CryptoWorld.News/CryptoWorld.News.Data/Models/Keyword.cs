using Data.Models.Common;

namespace Data.Models
{
    public class Keyword : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
    }
}

