using GR44.VendingMachine;
using static System.Console;

namespace VendingMachine_ConsoleUI
{
    internal class VendingMachine_PurchaseUI
    {
        static CellVisualizer Start(VendingMachineService vendingMachine)
        {
            Clear();
            ForegroundColor = ConsoleColor.White;
            SetCursorPosition(0, 0);
            WriteLine("Buy (SPACE)   Pay (F5)   Cancel (F8)");

            var gridUI = new CellVisualizer(0, 3, vendingMachine.ColumnCount, vendingMachine.RowCount, 5);
            gridUI.WriteBoxes(vendingMachine.ShowAll());
            UpdateUI(vendingMachine, gridUI);

            return gridUI;
        }

        static internal void Show(VendingMachineService vendingMachine)
        {
            var gridUI = Start(vendingMachine);

            bool exit = false;
            while (!exit)
            {
                var keyInfo = ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.F5:
                        VendingMachine_PayUI.Show(vendingMachine);

                        gridUI = Start(vendingMachine);
                        break;

                    case ConsoleKey.F8:
                        vendingMachine.CancelAllPurchases();
                        gridUI.WriteBoxes(vendingMachine.ShowAll());
                        UpdateUI(vendingMachine, gridUI);
                        break;

                    case ConsoleKey.Spacebar:
                        if (vendingMachine.PurchaseItem(gridUI.Y, gridUI.X).Success)
                            UpdateUI(vendingMachine, gridUI);

                        break;

                    case ConsoleKey.LeftArrow:
                        gridUI.MoveCursor(CellVisualizer.Direction.Left);
                        UpdateUI(vendingMachine, gridUI);
                        break;

                    case ConsoleKey.RightArrow:
                        gridUI.MoveCursor(CellVisualizer.Direction.Right);
                        UpdateUI(vendingMachine, gridUI);
                        break;

                    case ConsoleKey.UpArrow:
                        gridUI.MoveCursor(CellVisualizer.Direction.Up);
                        UpdateUI(vendingMachine, gridUI);
                        break;

                    case ConsoleKey.DownArrow:
                        gridUI.MoveCursor(CellVisualizer.Direction.Down);
                        UpdateUI(vendingMachine, gridUI);
                        break;

                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }


        }

        static void UpdateUI(VendingMachineService vendingMachine, CellVisualizer gridUI, int maxWidth = 40, int maxHeight = 10)
        {
            gridUI.SaveCursorPosition();

            string nextProductCode = vendingMachine.ShowSingle(gridUI.Y, gridUI.X);
            gridUI.MoveToCell(gridUI.X, gridUI.Y, false);
            WriteLine(nextProductCode.PadRight(5));

            var p = vendingMachine.GetProductDetails(gridUI.Y, gridUI.X);

            SetCursorPosition(gridUI.RightEdge + 1, gridUI.TopEdge);

            int i = 0;
            if (p.Success && p.ReturnValue is not null)
            {
                string[] T = p.ReturnValue.Examine();

                foreach (var item in T)
                {
                    SetCursorPosition(gridUI.RightEdge + 2, gridUI.TopEdge + i++);
                    WriteLine(item.PadRight(maxWidth));
                }
                while (i < maxHeight)
                {
                    SetCursorPosition(gridUI.RightEdge + 2, gridUI.TopEdge + i++);
                    WriteLine(".".PadRight(maxWidth));
                }
            }
            else
            {
                SetCursorPosition(gridUI.RightEdge + 2, gridUI.TopEdge);
                WriteLine(p.Message?.PadRight(maxWidth));

                while (++i < maxHeight)
                {
                    SetCursorPosition(gridUI.RightEdge + 2, gridUI.TopEdge + i);
                    WriteLine(".".PadRight(maxWidth));
                }
            }

            SetCursorPosition(0, gridUI.TopEdge - 1);
            Write($"Selected {vendingMachine.OrderSum} kr, Paid {vendingMachine.PaidSum} kr, Missing {vendingMachine.MissingAmount} kr".PadRight(50));

            gridUI.RestoreCursorPosition();

        }

    }
}
