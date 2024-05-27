namespace CryptoWorld.News.Data.Models
{
    using Data.Models.Common;
    using Microsoft.AspNetCore.Identity;

	public class ApplicationUser : IdentityUser<Guid>, IAuditable
	{
		public ApplicationUser()
		{
			this.Id = Guid.NewGuid();
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Img { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? DeletedOn { get; set; }
		public virtual List<Comment> Comments { get; set; } = [];
	}
}