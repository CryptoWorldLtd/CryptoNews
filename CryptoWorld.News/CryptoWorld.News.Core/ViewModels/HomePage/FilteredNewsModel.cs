using CryptoWorld.News.Core.Enumerations;

namespace CryptoWorld.News.Core.ViewModels.HomePage
{
    public class FilteredNewsModel
    {
        public const int NewsPerPage = 2;
        public string Category { get; init; } = null!;
        public string SearchTerm { get; init; } = null!;
        public NewsSorting Sorting { get; init; }
        public int CurrentPage { get; init; } = 1;
    }
}