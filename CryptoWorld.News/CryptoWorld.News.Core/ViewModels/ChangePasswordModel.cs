using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptоWorld.News.Core.ViewModels
{
    public class ChangePasswordModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "The NewPassword and Confirmed password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
