using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GR44.VendingMachine.ServiceBase;
using GR44.VendingMachine.Products;
using GR44.VendingMachine.SalesPromotions;
using GR44.VendingMachine.Payments;
using System.Net.Http.Headers;
using System.Reflection.Metadata;

namespace GR44.VendingMachine
{


    public class VendingMachineService : IVendingMachineService
    {
        public readonly int MaxSlotDepth = 20;
        public readonly int MaxRows = 15;
        public readonly int MaxColumns = 15;

        private Queue<Product>[,] _InternalStorage;
        private Queue<Product>[,] _InternalStoragePurchases;
        private ProductService ProductService = new();
        private Payments.CashPaymentService PaymentService = new();
        private Orders.OrderService OrderService = new();

        public int RowCount => _InternalStorage.GetUpperBound(0) + 1;
        public int ColumnCount => _InternalStorage.GetUpperBound(1) + 1;

        public int OrderSum => OrderService.OrderSum;
        public int PaidSum => PaymentService.PaidAmount;
        public int MissingAmount => OrderService.OrderSum - PaymentService.PaidAmount;

        /// <summary>
        /// This constructor initializes a new Vending Machine of the choosen dimensions.
        /// </summary>
        /// <param name="rows">The number of rows in the machine</param>
        /// <param name="columns">The number of columns in the machine</param>
        /// <example>
        /// VendingMachineService service = new(10, 10);
        /// </example>
        public VendingMachineService(byte rows, byte columns)
        {
            if (rows <= 0 || rows >= 20)
                throw new Exception("3984756934876");

            _InternalStorage = new Queue<Product>[rows, columns];
            _InternalStoragePurchases = new Queue<Product>[rows, columns];
        }

        public ServiceResponse<(Product[] Products, CashCollection ExchangeMoney)> EndTransaction()
        {
            int moneyNotUsed = PaymentService.PaidAmount - OrderService.OrderSum;

            if (moneyNotUsed < 0)
                return new ServiceResponse<(Product[], CashCollection)>("Add more money!");

            if (moneyNotUsed > 0 && !PaymentService.CanReturnExchange(moneyNotUsed))
                return new ServiceResponse<(Product[], CashCollection)>("Have no change!");

            return new ServiceResponse<(Product[] Products, CashCollection ExchangeMoney)>(
                (
                    CheckoutPurchasedProducts(), 
                    PaymentService.FinalizeTransaction(OrderService.OrderSum)
                ));
        }

        public ServiceResponse<int> FillStorage(int row, int column, Product product, int productCount = 1)
        {
            if (row > _InternalStorage.GetUpperBound(0) || column > _InternalStorage.GetUpperBound(1))
                return new ServiceResponse<int>("That cell does not exist in the machine!");

            if (_InternalStorage[row, column] == null)
                _InternalStorage[row, column] = new Queue<Product>();

            if (productCount > MaxSlotDepth - _InternalStorage[row, column].Count)
                return new ServiceResponse<int>("They will not fit into that slot!");

            if (_InternalStorage[row, column].Where<Product>(p => p.Code != product.Code).Any())
                return new ServiceResponse<int>("A storage slot may only contain products of the same kind (has the same product code)");

            while(productCount-- > 0)
                _InternalStorage[row, column].Enqueue(product);

            return new ServiceResponse<int>(MaxSlotDepth - _InternalStorage[row, column].Count);
        }

        public ServiceResponse<Product> GetProductDetails(int row, int column)
        {
            if (_InternalStorage[row, column]?.Count > 0) // is null)
                return new ServiceResponse<Product>(_InternalStorage[row, column].Peek());
            else
                return new ServiceResponse<Product>("[Empty]");
        }

        public ServiceResponse PayMoney(int amount)
        {
            CashUnit singlePieceOfCash = PaymentService.GetCashUnit(amount);
            if (singlePieceOfCash == CashUnit.NOT_SET)
                return new ServiceResponse("Not a valid type of cash!");
            else
                return PaymentService.AddPayment(singlePieceOfCash);
        }

        public ServiceResponse PurchaseItem(int row, int column)
        {
            if (_InternalStorage[row, column]?.Count > 0)
            {
                var product = _InternalStorage[row, column].Dequeue();
                if (_InternalStoragePurchases[row, column] is null)
                    _InternalStoragePurchases[row, column] = new Queue<Product>();

                _InternalStoragePurchases[row, column].Enqueue(product);

                OrderService.AddItem(new ProductOrderItem(product));

                return new ServiceResponse();
            }

            return new ServiceResponse("No product");

        }

        public void CancelAllPurchases()
        {
            for (int row = 0; row <= _InternalStoragePurchases.GetUpperBound(0); row++)
                for (int column = 0; column <= _InternalStoragePurchases.GetUpperBound(1); column++)
                    while (_InternalStoragePurchases[row, column]?.Count > 0)
                        _InternalStorage[row, column].Enqueue(_InternalStoragePurchases[row, column].Dequeue());

            OrderService.Clear();
        }

        public Product[] ListPurchases()
        {
            List<Product> products = new();

            for (int row = 0; row <= _InternalStoragePurchases.GetUpperBound(0); row++)
                for (int column = 0; column <= _InternalStoragePurchases.GetUpperBound(1); column++)
                    products.AddRange(_InternalStoragePurchases[row, column]?.ToArray() ?? new Product[] { });

            return products.ToArray();
        }



        public string[,] ShowAll()
        {
            int rows = _InternalStorage.GetUpperBound(0) + 1;
            int columns = _InternalStorage.GetUpperBound(1) + 1;
            string[,] strings = new string[rows, columns];

            for (int row = 0; row < rows; row++)
                for (int column = 0; column < columns; column++)
                    strings[row, column] = _InternalStorage[row, column]?.Count > 0 ? _InternalStorage[row, column].Peek().Code : "";

            return strings;
        }

        public string ShowSingle(int row, int column) => _InternalStorage[row, column]?.Count > 0 ? _InternalStorage[row, column].Peek().Code : "";

        private Product[] CheckoutPurchasedProducts()
        {
            List<Product> products = new();

            for (int row = 0; row <= _InternalStoragePurchases.GetUpperBound(0); row++)
                for (int column = 0; column <= _InternalStoragePurchases.GetUpperBound(1); column++)
                    while (_InternalStoragePurchases[row, column]?.Count > 0)
                        products.Add(_InternalStoragePurchases[row, column].Dequeue());
        
            return products.ToArray();
        }


    }


}
