using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Comment : BaseModel
    {
        [Required]
        public string Content { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public DateTime PublishedOn { get; set; }

        public string PublisherId { get; set; }

        public virtual IdentityUser Publisher { get; set; }
    }
}