using GR44.VendingMachine.ServiceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR44.VendingMachine.Payments
{

    public class CashPaymentService : IPaymentService
    {
        private CashCollection _current = new();
        private CashCollection _justPaid = new();

        public int PaidAmount => _justPaid.Sum;

        /// <summary>
        /// Constructor
        /// </summary>
        public CashPaymentService()
        {
            _current.FillRandomCash(10);
        }

        public ServiceResponse AddPayment(CashUnit unit) 
        {
            Random rand = new();
            if (rand.Next(5) == 3) // money not accepted (1 of 5), try again
                return new ServiceResponse("Your cash was not accepted by the machine");

            if (_justPaid.ContainsKey(unit))
                _justPaid[unit]++;
            else
                _justPaid.Add(unit, 1);

            return new ServiceResponse();
        }

        public CashUnit GetCashUnit(int amount)
        {
            //int Test = (int)amount;

            //if (Test != amount)
            //    throw new Exception("63249857634985");

            if (!Enum.IsDefined(typeof(CashUnit), amount))
                return CashUnit.NOT_SET;

            return (CashUnit)amount;
        }

        public bool CanReturnExchange(int sumToReturn) 
        {
            CashCollection bag = _current + _justPaid;
            return bag.SelectAmount(sumToReturn).Sum == sumToReturn;
        }

        public CashCollection FinalizeTransaction(int sumToPay) 
        {
            if (_justPaid.Sum < sumToPay)
                throw new Exception("3487563948756");

            CashCollection totalMoneyInMachine = _current + _justPaid;

            int AmountToReturn = _justPaid.Sum - sumToPay;
            CashCollection moneyToReturnToCustomer = totalMoneyInMachine.SelectAmount(AmountToReturn);

            if (AmountToReturn != moneyToReturnToCustomer.Sum)
                throw new Exception("8937465983745");

            // all looks good
            _current = totalMoneyInMachine - moneyToReturnToCustomer;
            _justPaid.Clear();
            return moneyToReturnToCustomer;
        }
    }
}
