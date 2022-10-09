using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GR44.VendingMachine.SalesPromotions;
using GR44.VendingMachine.Products;
using GR44.VendingMachine.Orders;

namespace GR44.VendingMachine
{
    public class PromotableProduct : IPromotable
    {
        Product Product { get; }
        string IPromotable.Key => Product.Code;

        decimal IPromotable.Price => Product.Price;

        internal PromotableProduct(Product product) 
        {
            Product = product;

            PromotableProduct x = new(new Toy("", 0, ""));

            GR44.VendingMachine.SalesPromotions.SalesPromotionService  B = new();
            B.Add(new MultiSell(x, 2, 3));

            
        }
    }

    public class ProductOrderItem : IOrderItem
    {
        public Product Product { get; }
        public int ItemCount { get; set; }
        string IOrderItem.ItemName => Product.Name;
        int IOrderItem.ItemPrice => Product.Price;

        public ProductOrderItem(Product p)
        {
            Product = p;
            ItemCount = 1;
        }
    }


}
