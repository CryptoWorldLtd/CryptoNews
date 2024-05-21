using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Comment
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; } = null!;

        public DateTime PublishedOn { get; set; }

        public string PublisherId { get; set; } = string.Empty;

        public virtual IdentityUser Publisher { get; set; } = null!;
    }
}