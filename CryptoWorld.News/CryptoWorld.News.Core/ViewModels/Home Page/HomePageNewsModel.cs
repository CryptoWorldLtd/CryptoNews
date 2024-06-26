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
        public HomePageNewsModel(string title, string content, string imageUrl, string datePublished)
        {
           this.Title= title; 
            this.Content= content;
            this.ImageUrl= imageUrl;
            this.DatePublished= datePublished;

        }
        [Required]
        string Title {  get; set; } 

        [Required]
        string Content { get; set; }

        public string ImageUrl {  get; set; }

        public string DatePublished { get; set; }

    }
}
