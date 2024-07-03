using CryptоWorld.News.Core.ViewModels.Home_Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptоWorld.News.Core.Interfaces
{
    public interface INewsService
    {
        public Task <List<PageNewsModel>> HomePageNews(int pagesCount);
    }
}
