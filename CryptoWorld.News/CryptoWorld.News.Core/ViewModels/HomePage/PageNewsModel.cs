namespace CryptоWorld.News.Core.ViewModels.Home_Page
{
    public class PageNewsModel
    {
        public PageNewsModel(string title, string content, string imageUrl, string datePublished)
        {
           this.Title= title; 
            this.Content= content;
            this.ImageUrl= imageUrl;
            this.DatePublished= datePublished;

        }
      
       public string Title {  get; set; } 

        public string Content { get; set; }

        public string ImageUrl {  get; set; }

        //06.06.2024 12:33:36
        public string DatePublished { get; set; }

    }
}
