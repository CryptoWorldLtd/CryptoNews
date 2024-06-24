using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptоWorld.News.Core.ViewModels.Home_Page
{
    public class HomePageNewsModel
    {
        [Required]
        string Title {  get; set; } 

        [Required]
        string Content { get; set; }

        public string ImageUrl {  get; set; }

        public string DateTime { get; set; }

    }
}
