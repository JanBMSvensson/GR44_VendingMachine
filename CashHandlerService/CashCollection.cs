using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR44.VendingMachine.Payments
{

    public class CashCollection : Dictionary<CashUnit, int>
    {
        /// <summary>
        /// The total sum of all money it contains
        /// </summary>
        public int Sum => this.Sum(item => (int)item.Key * item.Value);

        /// <summary>
        /// Fills the bag with a random number of money (only for simulations).
        /// </summary>
        /// <param name="maxCountOfEachUnit">The highest number of items of a single money unit.</param>
        public void FillRandomCash(int maxCountOfEachUnit)
        {
            Random rand = new();
            Array.ForEach<CashUnit>(Enum.GetValues<CashUnit>(), MoneyUnit => Add(MoneyUnit, rand.Next(maxCountOfEachUnit)));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal CashCollection() { }
        protected CashCollection(IEnumerable<KeyValuePair<CashUnit, int>> collection) : base(collection) { }


        /// <summary>
        /// Overloads the +operator (newSumBag = moneyBag1 <b>+</b> moneyBag2)
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns>A new BagOfMoney containing all money from both bags.</returns>
        public static CashCollection operator +(CashCollection b1, CashCollection b2)
        {
            CashCollection newBag = new(b1);

            foreach (var item in b2)
                if (newBag.ContainsKey(item.Key))
                    newBag[item.Key] += item.Value;
                else
                    newBag.Add(item.Key, item.Value);

            return newBag;
        }

        /// <summary>
        /// Overloads the -operator (newDiffBag = moneyBag1 <b>-</b> moneyBag2)
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2">All money in this bag must exist in the previous bag b1</param>
        /// <returns>A new BagOfMoney containing all money left in b1 after removing all money contained in b2.</returns>
        /// <exception cref="Exception"></exception>
        public static CashCollection operator -(CashCollection b1, CashCollection b2)
        {
            CashCollection newBag = new(b1);

            foreach (var item in b2)
                if (!newBag.ContainsKey(item.Key) || newBag[item.Key] < item.Value)
                    throw new Exception($"Can't remove {item.Value} of {item.Key}");
                else
                    newBag[item.Key] -= item.Value;

            return newBag;
        }

        /// <summary>
        /// Tries to select money of different units until it reaches the exactAmount (check .Sum property to find out if it was possible)
        /// </summary>
        /// <param name="exactAmount">Selects money until it reaches this value.</param>
        /// <returns>A new BagOfMoney that contains the exactAmount or as close as possible (might not be enough money in the bag).</returns>
        public CashCollection SelectAmount(int exactAmount)
        {
            CashCollection bagToReturn = new();

            foreach (var item in this)
            {
                int itemsNeeded = exactAmount / (int)item.Key; // i.e. amount 125 / MoneyUnit 100 (_100Kr) = 1
                if (itemsNeeded > 0 && itemsNeeded <= item.Value)
                {
                    // take the money needed
                    bagToReturn.Add(item.Key, itemsNeeded);
                }
                else if (itemsNeeded > 0 && item.Value > 0)
                {
                    // don't have enough but take all there is
                    bagToReturn.Add(item.Key, item.Value);
                }
            }

            return bagToReturn;
        }
    }
}
