using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR44.VendingMachine.Products
{
    public class ProductService
    {
        private Dictionary<string, Product> _products = new();

        //public Product Add(Product p) => _products.Add(p.Code, p);




        public ProductService()
        {
            Product p;
            p = new Food("Snickers", 12, "F-SNK", "peanuts");
            _products.Add(p.Code, p);

            p = new Toy("Big Bouncing Ball (mixed colors)", 23, "T-BBB");
            _products.Add(p.Code, p);

            p = new Drink("Julmust", 15, "D-JMT");
            _products.Add(p.Code, p);


        }

        public Product GetProduct(string code) => _products[code];

    }
}
