
using System.ComponentModel;
using static System.Console;

namespace GR44_W2_SodaCrate
{
    internal class ConsoleMenu
    {
        List<ConsoleMenuItem> menuItems = new();
        List<ConsoleKey> validKeys = new();
        string MenuTitle;

        public ConsoleMenu(string menuTitle)
        {
            validKeys.Add(ConsoleKey.Escape);
            MenuTitle = menuTitle;
        }

        internal void Add(params ConsoleMenuItem[] items)
        {
            foreach (var item in items)
            {
                validKeys.Add(ConsoleKey.F1 + menuItems.Count);
                menuItems.Add(item);
            }

        }

        internal void WriteMenuTitle()
        {
            ResetColor();
            WriteLine(MenuTitle);
            WriteLine();
        }

        internal void WriteMenu()
        {
            ResetColor();

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (menuItems[i].IsDeactivatedRule is not null && menuItems[i].IsDeactivatedRule()) // Unless I missunderstand the &&-operator, this compiler warning is wrong!
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                else
                    Console.ResetColor();

                WriteLine($"F{i + 1}".PadRight(4) + menuItems[i].GetMenuText.Invoke());
            }

            ResetColor();
            WriteLine();
            WriteLine("Press ESC to exit");

        }

        internal ConsoleMenuItem? Wait()
        {
            ConsoleKey KeyPressed;
            ConsoleMenuItem? SelectedItem = null;
            bool BadKey = true;

            do
            {
                do { KeyPressed = ReadKey(true).Key; }
                while (!validKeys.Contains(KeyPressed));

                if (KeyPressed == ConsoleKey.Escape)
                {
                    BadKey = false;
                }
                else
                {
                    SelectedItem = menuItems[KeyPressed - ConsoleKey.F1];
                    if (!SelectedItem.IsDeactivated)
                    {
                        menuItems[KeyPressed - ConsoleKey.F1].DoAction();
                        BadKey = false;
                    }

                }

            } while (BadKey);

            return SelectedItem;
        }
    }

    internal class ConsoleMenuItem
    {
        internal delegate string MenuText();
        internal delegate void MenuAction();
        internal delegate bool DeactivationRule();

        //internal string Key { get; set; }
        internal MenuText GetMenuText { get; }
        MenuAction? MenuTriggerAction { get; }
        internal DeactivationRule? IsDeactivatedRule { get; }
        internal bool IsDeactivated { get { return IsDeactivatedRule is not null && IsDeactivatedRule(); } }
        internal ConsoleMenuItem(MenuText getMenuText, MenuAction menuAction, DeactivationRule? deactivateWhen = null)
        {
            //Key= key;
            GetMenuText = getMenuText;
            MenuTriggerAction = menuAction;
            IsDeactivatedRule = deactivateWhen;
        }
        internal void DoAction() { MenuTriggerAction?.Invoke(); }
    }

}
