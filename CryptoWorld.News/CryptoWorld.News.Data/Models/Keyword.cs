using CryptoWorld.News.Data.Models.Common;

namespace CryptoWorld.News.Data.Models
{
    public class Keyword : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Article> Articles { get; set; } = new List<Article>();
    }
}