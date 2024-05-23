using Data.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        public string Content { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public DateTime PublishedOn { get; set; }

        public string PublisherId { get; set; }

        public virtual ApplicationUser Publisher { get; set; }
    }
}
