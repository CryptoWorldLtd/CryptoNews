using CryptoWorld.News.Data.Models.Common;

namespace CryptoWorld.News.Data.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}