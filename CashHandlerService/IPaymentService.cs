using GR44.VendingMachine.ServiceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR44.VendingMachine.Payments
{
    public interface IPaymentService
    {

        /// <summary>
        /// Currently paid amount
        /// </summary>
        public int PaidAmount { get; }

        /// <summary>
        /// Add one item of the specified money unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns>true if the cash handler accepts the money, false if it rejects the money</returns>
        public ServiceResponse AddPayment(CashUnit unit);

        /// <summary>
        /// Test if the cash handler has enough change
        /// </summary>
        /// <param name="amountToReturn"></param>
        /// <returns>The transaction can't be finalized if the cash handler can't return the correct amount of change</returns>
        public bool CanReturnExchange(int amountToReturn);

        /// <summary>
        /// Finalize the transaction, subtractinig the sumToPay from the allready paid cash and returning the difference.
        /// </summary>
        /// <param name="sumToPay"></param>
        /// <returns>The change to return to the customer</returns>
        /// <exception cref="Exception"></exception>
        public CashCollection FinalizeTransaction(int sumToPay);

    }
}
