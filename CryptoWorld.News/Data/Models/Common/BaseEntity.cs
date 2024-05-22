using System.ComponentModel.DataAnnotations;

namespace Data.Models.Common
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
