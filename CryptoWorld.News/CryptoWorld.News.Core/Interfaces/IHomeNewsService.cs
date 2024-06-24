using CryptоWorld.News.Core.ViewModels.Home_Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptоWorld.News.Core.Interfaces
{
    public interface IHomeNewsService
    {
        Task<HomePageNewsModel> HomePageNewsAsync(HomePageNewsModel model);
    }
}
