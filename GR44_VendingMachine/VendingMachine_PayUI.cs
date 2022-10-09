using GR44.VendingMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace VendingMachine_ConsoleUI
{
    internal class VendingMachine_PayUI
    {
        static internal void Show(VendingMachineService vendingMachine)
        {
            Clear();
            WriteLine("Cancel (F8)");
            WriteLine();

            foreach (var item in vendingMachine.ListPurchases())
                WriteLine(item.Name.PadRight(40) + (item.Price.ToString() + " kr").PadLeft(8));

            WriteLine("Total:".PadRight(40) + (vendingMachine.OrderSum.ToString() + " kr").PadLeft(8));

            int money = 5;
            var result = vendingMachine.PayMoney(money);
            if (result.Success)
                WriteLine("Paid 5 kr");
            else
                WriteLine(result.Message);

            ReadKey(true);

        }

    }
}
