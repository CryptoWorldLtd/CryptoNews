using CryptoWorld.News.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptоWorld.News.Core.ViewModels
{
    public class MyNews
    {
        public IEnumerable<Article>? MyNewsList { get; set; }
    }
}
