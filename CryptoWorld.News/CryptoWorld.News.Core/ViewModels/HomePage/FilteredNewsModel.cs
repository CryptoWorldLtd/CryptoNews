using CryptoWorld.News.Core.Enumerations;

namespace CryptoWorld.News.Core.ViewModels.HomePage
{
    public class FilteredNewsModel
    {
        public const int NewsPerPage = 5;
        public string Category { get; init; }
        public string Region { get; init; }
        public string SearchTerm { get; init; }
        public NewsSorting Sorting { get; init; }
        public int CurrentPage { get; init; } = 1;
    }
}