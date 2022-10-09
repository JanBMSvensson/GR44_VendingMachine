
using static System.Console;
namespace VendingMachine_ConsoleUI
{
    class CellVisualizer
    {
        int height, width;
        int screenX, screenY;
        int activeX, activeY;
        int cellSpace;
        int savedCursorX, savedCursorY;

        char[,] BorderChars = { { '┌', '┬', '┐' },
                            { '├', '┼', '┤' },
                            { '└', '┴', '┘' } };
        char HorizontalBorder = '─';
        char VerticalBorder = '│';
        //public readonly char FlagChar = 'X';
        //public readonly char HiddenChar = '░';

        public int X => activeX;
        public int Y => activeY;

        public int LeftEdge => screenX;
        public int RightEdge => screenX + width * (cellSpace + 1);
        public int TopEdge => screenY;
        public int BottomEdge => screenY + height * 2;

        public CellVisualizer(int screenX, int screenY, int width, int height, int cellSpace)
        {
            this.height = height;
            this.width = width;
            this.screenX = screenX;
            this.screenY = screenY;
            this.cellSpace = cellSpace;
            activeX = 0;
            activeY = 0;
        }


        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        public void MoveCursor(Direction d)
        {
            Move(d);
            MoveToCell(this.activeX, this.activeY);
        }

        public void RestoreCursorPosition()
        {
            SetCursorPosition(savedCursorX, savedCursorY);
        }

        public void SaveCursorPosition()
        {
            savedCursorX = CursorLeft;
            savedCursorY = CursorTop;
        }

        public void MoveToCell(int x, int y, bool centerPosition = true)
        {
            int px = 0;
            if(centerPosition)
                px = this.screenX + (cellSpace + 1) / 2 + x * (cellSpace + 1);
            else
                px = this.screenX + 1 + x * (cellSpace + 1);

            int py = this.screenY + 1 + y * 2;
            SetCursorPosition(px, py);
            //Debug.WriteLine($"X={px}, Y={py}");
        }

        private bool Move(Direction d)
        {
            switch (d)
            {
                case Direction.Left:
                    if (activeX == 0)
                        return false;
                    else
                        activeX--;
                    break;

                case Direction.Right:
                    if (activeX == width - 1)
                        return false;
                    else
                        activeX++;
                    break;

                case Direction.Up:
                    if (activeY == 0)
                        return false;
                    else
                        activeY--;
                    break;

                case Direction.Down:
                    if (activeY == height - 1)
                        return false;
                    else
                        activeY++;
                    break;

                default:
                    throw new Exception("8627394876234");

            }

            return true;
        }

        public void WriteBoxes(string[,]? content = null)
        {
            activeX = 0;
            activeY = 0;
            SetCursorPosition(screenX, screenY);
            ForegroundColor = ConsoleColor.DarkGray;
            for (int y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    int cx = y == 0 ? 0 : y == height ? 2 : 1;
                    int cy = x == 0 ? 0 : x == width ? 2 : 1;

                    Write(BorderChars[cx, cy]);
                    if (x < width)
                        Write("".PadLeft(cellSpace).Replace(' ', HorizontalBorder));
                }
                WriteLine();
                if (y < height)
                {
                    for (int x = 0; x <= width; x++)
                    {
                        Write(VerticalBorder);
                        if (x < width)
                        {
                            if (content is null)
                                Write("".PadLeft(cellSpace));
                            else
                                Write(content[y, x].PadRight(cellSpace)); 

                        }
                    }
                    WriteLine();
                }
            }
            MoveToCell(0, 0);
        }

    }
}
