
using MongoDB.Driver.Core.Operations;

namespace ConsoleMenu
{
    class ConsoleMenuItem<T>
    {
        private readonly string text;
        private readonly T? data;
        public bool isSelected;

        public ConsoleMenuItem(string text, T data)
        {
            this.data = data;
            this.text = text;
            this.isSelected = false;
        }

        public ConsoleMenuItem(string text)
        {
            this.text = text;
            this.isSelected = false;
        }

        public T? getData()
        {
            return this.data;
        }

        public void writeConsole()
        {
            if (this.isSelected)
            {
                Console.Write("[█] ");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
                Console.Write("[ ] ");

            Console.WriteLine(this.text);
            Console.ForegroundColor = SelectorMenu<T>.getDefaultColor();
        }
    }

    class SelectorMenu<T>
    {
        private const int UNASSIGEND = -1;
        private string? menuTitle;
        private int selectedIdx = 0;
        private List<ConsoleMenuItem<T>> menuEntries;
        private static readonly ConsoleColor defaultColor = ConsoleColor.Gray;

        public static ConsoleColor getDefaultColor()
        {
            return defaultColor;
        }

        public SelectorMenu(List<ConsoleMenuItem<T>> menuEntries, int defaultIdx = 0, string? menuTitle = null)
        {
            if (menuEntries.Count == 0)
                throw new Exception("List must contain at least one record");
            this.menuTitle = menuTitle;
            Console.ForegroundColor = SelectorMenu<T>.defaultColor;
            this.menuEntries = menuEntries;
            this.menuEntries.ForEach(entry => { entry.isSelected = false; });
            this.setSelectedEntry(defaultIdx);
        }

        public int getSelectedIndex()
        {
            return this.selectedIdx;
        }

        public T? getData()
        {
            return this.menuEntries[this.getSelectedIndex()].getData();
        }

        public void setSelectedEntry(int selectedIdx)
        {
            if (selectedIdx + 1 > this.menuEntries.Count)
                throw new Exception("Selected index is larger than list index range");
            this.menuEntries[this.selectedIdx].isSelected = false;
            this.menuEntries[selectedIdx].isSelected = true;
            this.selectedIdx = selectedIdx;
        }

        public void printMenu()
        {
            Console.Clear();
            if (this.menuTitle != null)
                Console.WriteLine(this.menuTitle);
            this.menuEntries.ForEach(entry => entry.writeConsole());
        }

        public void moveCursorUp()
        {
            int newIndex = selectedIdx - 1;
            if (newIndex < 0)
                newIndex = this.menuEntries.Count - 1;
            this.setSelectedEntry(newIndex);
        }

        public void moveCursorDown()
        {
            int newIndex = selectedIdx + 1;
            if (newIndex > this.menuEntries.Count - 1)
                newIndex = 0;

            this.setSelectedEntry(newIndex);
        }

        public int runConsoleMenu()
        {
            this.printMenu();
            ConsoleKey? pressedKey = null;
            int selectedValue = UNASSIGEND;
            while (pressedKey != ConsoleKey.Escape || pressedKey != ConsoleKey.Enter)
            {
                pressedKey = Console.ReadKey().Key;
                if (pressedKey == ConsoleKey.UpArrow || pressedKey == ConsoleKey.K)
                    this.moveCursorUp();
                if (pressedKey == ConsoleKey.DownArrow || pressedKey == ConsoleKey.J)
                    this.moveCursorDown();
                if (pressedKey == ConsoleKey.Escape)
                    break;
                if (pressedKey == ConsoleKey.Enter)
                {
                    selectedValue = this.selectedIdx;
                    break;
                }
                this.printMenu();
            }
            Console.Clear();
            return selectedValue;
        }
    };
}
