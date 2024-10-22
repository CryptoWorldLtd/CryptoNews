﻿namespace CryptoWorld.News.Data.Models
{
    using Data.Models.Common;
    using Microsoft.AspNetCore.Identity;

	public class ApplicationRole : IdentityRole<Guid>, IAuditable
	{
		public ApplicationRole()
		{
			this.Id = Guid.NewGuid();
		}
        public ApplicationRole(string roleName) : base(roleName)
        {
            this.Id = Guid.NewGuid();
        }
        public DateTime CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? DeletedOn { get; set; }
	}
}
