using CryptoWorld.News.Data.Models.Common;

namespace CryptoWorld.News.Data.Models
{
    public class Source : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public virtual List<Article> Articles { get; set; } = [];
    }
}