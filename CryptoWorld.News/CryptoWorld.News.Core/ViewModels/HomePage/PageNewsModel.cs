namespace CryptоWorld.News.Core.ViewModels.Home_Page
{
    public class PageNewsModel
    {
        public PageNewsModel(string title, string content, string imageUrl, string datePublished, int rating, string region)
        {
            this.Title= title; 
            this.Content= content;
            this.ImageUrl= imageUrl;
            this.DatePublished= datePublished;
            this.Rating = rating;
            this.Region = region;
        }
      
        public string Title {  get; set; } 
        public string Content { get; set; }
        public string ImageUrl {  get; set; }
        public string DatePublished { get; set; }
        public string Category {  get; set; }
        public int Rating { get; set; }
        public string Region { get; set; }
    }
}