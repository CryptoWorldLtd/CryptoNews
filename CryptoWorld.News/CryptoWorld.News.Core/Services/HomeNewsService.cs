using CryptоWorld.News.Core.Interfaces;
using CryptоWorld.News.Core.ViewModels.Home_Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptоWorld.News.Core.Services
{
    public class HomeNewsService : IHomeNewsService
    {
        public Task<HomePageNewsModel> HomePageNewsAsync(HomePageNewsModel model)
        {
            throw new NotImplementedException();
        }
    }
}
