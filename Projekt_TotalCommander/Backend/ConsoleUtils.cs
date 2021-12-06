using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    
    public class ConsoleUtils2
    {
        public int CursorX { get; set; }
        public int CursorY { get; set; }
        public int InitX { get; set; }
        public int InitY { get; set; }
        public int MaxWidthWrite { get; set; }
        public int MaxHeightWrite { get; set; }
        public ConsoleColor BackColor { get; set; }
        public ConsoleColor ForeColor { get; set; }

        public ConsoleUtils2(int x, int y, int widthDraw, int heightDraw,ConsoleColor fore, ConsoleColor back)
        {
            this.InitX = x;
            this.InitY = y;
            this.MaxHeightWrite = heightDraw;
            this.MaxWidthWrite = widthDraw;
            this.CursorX = x;
            this.CursorY = y;
            this.BackColor = back;
            this.ForeColor = fore;
        }
        public void ResetToOrigin()
        {
            this.CursorX = this.InitX;
            this.CursorY = this.InitY;
        }
        public void WriteLine(string input = "")
        {
            if (CursorY - InitY <= MaxHeightWrite && this.CursorX<this.InitX+this.MaxWidthWrite)
            {
                Console.BackgroundColor = this.BackColor;
                Console.ForegroundColor = this.ForeColor;
                Console.SetCursorPosition(CursorX, CursorY);
                string temp = input;
                if (CursorX + input.Length - InitX >= MaxWidthWrite)
                {
                    temp = input.Substring(0, input.Length - (CursorX + input.Length - InitX - MaxWidthWrite));
                }
                else
                {
                    temp = input.PadRight(this.MaxWidthWrite + this.InitX - CursorX, ' ');
                }
                Console.Write(temp);
            }
            CursorY = CursorY + 1;
            CursorX = InitX;
        }
        public void Write(string input)
        {
            if (CursorY - InitY <= MaxHeightWrite && this.CursorX < this.InitX + this.MaxWidthWrite)
            {
                Console.BackgroundColor = this.BackColor;
                Console.ForegroundColor = this.ForeColor;
                Console.SetCursorPosition(CursorX, CursorY);
                string temp = input;
                if (CursorX + input.Length - InitX >= MaxWidthWrite)
                {
                    temp = input.Substring(0, input.Length - (CursorX + input.Length - InitX - MaxWidthWrite));
                    //CursorX = this.InitX+this.MaxWidthWrite;
                }
                //else
                //{
                    CursorX = CursorX + temp.Length;
                //}
                Console.Write(temp);
            }
        }
        public void Clear()
        {
            Console.BackgroundColor = this.BackColor;
            for (int i = InitY; i < InitY+MaxHeightWrite; i++)
            {
                Console.SetCursorPosition(InitX, i);
                Console.Write(new string(' ', this.MaxWidthWrite));
            }
        }
        public static void DrawRectangleBorder(int x, int y, int width, int height)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("┌"+ "".PadRight(width - 2, '─')+ "┐");
            Console.SetCursorPosition(x, y + height);
            Console.Write("└" + "".PadRight(width - 2, '─') + "┘");
            DrawVerticalLine(x, y + 1, height - 2);
            DrawVerticalLine(x + width, y + 1, height - 2);
        }
        public static void DrawVerticalLine(int x, int y, int height)
        {
            for (int i = y; i < y + height + 1; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write('│');
            }
        }
        public static void DrawHorizontalLine(int x, int y, int width)
        {
            for (int i = x; i < x + width + 1; i++)
            {
                Console.SetCursorPosition(i, y);
                Console.Write('─');
            }
        }
    }
}
