using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPOTUSHKI
{
    public class OrderedProducts
    {
        public OrderedProducts(string article, int count)
        {
            this.ArticleNumber = article;
            this.OrderedCount = count;
        }

        public string ArticleNumber { get; set; }
        public int OrderedCount { get; set; }
    }
}
