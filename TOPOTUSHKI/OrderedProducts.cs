using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPOTUSHKI
{
    public class OrderedProducts
    {
        public OrderedProducts(string article, int count, Nullable<int> discount, string manufacturer, string descrip, decimal cost, string name, string photo)
        {
            this.ArticleNumber = article;
            this.OrderedCount = count;
            this.ProductDiscountAmount = discount;
            this.ProductManufacturer = manufacturer;
            this.ProductDescription = descrip;
            this.ProductCost = cost;
            this.ProductName = name;
            this.ProductPhoto = photo;
        }

        public OrderedProducts(){}

        public static OrderedProducts ToOrderedProducts(Product product)
        {
            OrderedProducts some = new OrderedProducts();
            some.ArticleNumber = product.ProductArticleNumber;
            some.OrderedCount = product.ProductQuantityInStock;
            some.ProductDiscountAmount = product.ProductDiscountAmount;
            some.ProductManufacturer = product.ProductManufacturer;
            some.ProductDescription = product.ProductDescription;
            some.ProductCost = product.ProductCost;
            some.ProductName = product.ProductName;
            some.ProductPhoto = product.ProductPhoto;
            return some;
        }

        public string ArticleNumber { get; set; }
        public int OrderedCount { get; set; }

        public Nullable<int> ProductDiscountAmount { get; set; }

        public string ProductManufacturer { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductCost { get; set; }

        public string ProductName { get; set; }

        public string ProductPhoto { get; set; }
    }
}
