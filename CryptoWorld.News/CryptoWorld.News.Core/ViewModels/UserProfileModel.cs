using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CryptоWorld.News.Core.ViewModels
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression("[a-z0-9!#$%&'+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&'+/=?^_`{|}~-]+)@(?:[a-z0-9](?:[a-z0-9-][a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")]
        public string Email { get; set; }
        public string Img {  get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }

        [RegularExpression("^[\\d]{10}$")]
        public string PhoneNumber { get; set; }

    }
    public enum Gender
    {
        None,
        Male,
        Famele
    }
}
