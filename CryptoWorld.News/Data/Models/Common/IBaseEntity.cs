using System.ComponentModel.DataAnnotations;

namespace Data.Models.Common
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }

        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
