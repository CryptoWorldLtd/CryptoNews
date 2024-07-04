using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptоWorld.News.Core.ViewModels
{
    public class ChangeEmailModel
    {
        [Required]
        [EmailAddress]
        public string CurrentEmail { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression("[a-z0-9!#$%&'+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&'+/=?^_`{|}~-]+)@(?:[a-z0-9](?:[a-z0-9-][a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")]
        public string NewEmail { get; set; }
        [Required]
        [Compare(nameof(NewEmail), ErrorMessage = "The New Email and Confirmed email do not match.")]
        public string ConfirmEmail { get; set; }
    }
}
