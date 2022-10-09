using System;

using GR44.VendingMachine.ServiceBase;
using GR44.VendingMachine.Products;
using GR44.VendingMachine.Payments;

namespace GR44.VendingMachine
{
    public interface IVendingMachineService
    {
        ServiceResponse PurchaseItem(int row, int column);
        string[,] ShowAll();

        /// <summary>
        /// Fill a specific slot in the machine with new products
        /// </summary>
        /// <param name="row">The row in the machine (0-indexed)</param>
        /// <param name="column">The column in the machine (0-indexed)</param>
        /// <param name="product">The product to add into the slot</param>
        /// <param name="productCount">The number of products put into the machine</param>
        /// <returns>The resulting number of empty spaces in that slot</returns>
        ServiceResponse<int> FillStorage(int row, int column, Product product, int productCount);

        public ServiceResponse<Product> GetProductDetails(int row, int column);
        ServiceResponse PayMoney(int amount);
        ServiceResponse<(Product[] Products, CashCollection ExchangeMoney)> EndTransaction();

    }

}
