using CryptoWorld.News.Data.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace CryptoWorld.News.Data.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        public string Content { get; set; }
        public Guid ArticleId { get; set; }
        public virtual Article Article { get; set; }
        public DateTime PublishedOn { get; set; }
        public Guid PublisherId { get; set; }
        public virtual ApplicationUser Publisher { get; set; }
    }
}