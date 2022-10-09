
using GR44.VendingMachine;
using GR44.VendingMachine.Products;
using VendingMachine_ConsoleUI;


VendingMachineService vendingMachine = new(5,8);
Product placeholder = new Food("Snickers", 12, "F-SNI", "peanuts");
vendingMachine.FillStorage(0, 0, placeholder, 15);
vendingMachine.FillStorage(0, 1, placeholder, 15);

placeholder = new Toy("Bouncing ball (mixed colors)", 25, "T-BBM");
vendingMachine.FillStorage(0, 2, placeholder, 10);

placeholder = new Drink("Fanta", 17, "D-FNT");
vendingMachine.FillStorage(0, 3, placeholder, 15);

placeholder = new Drink("Guinness (50cl)", 95, "D-GNS", 4.6m);
vendingMachine.FillStorage(1, 0, placeholder, 6);

VendingMachine_PurchaseUI.Show(vendingMachine);

