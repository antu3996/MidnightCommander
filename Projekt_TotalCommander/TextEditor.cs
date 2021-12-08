using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class TextEditor : IGraphicModule
    {
        public bool UpdateVisual { get; set; } = false;
        public bool Redraw { get; set; } = true;

        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleUtils2 Drawer { get; set; }
        public ConsoleUtils2 Header_Drawer { get; set; }
        public bool IsSelected { get; set; }
        public FileUtils CurrFile { get; set; }

        public ConsoleColor Highlight_Back { get; set; }
        public ConsoleColor HighLight_Fore { get; set; }
        public ConsoleColor Normal_Back { get; set; }
        public ConsoleColor Normal_Fore { get; set; }
        public ConsoleColor Header_Fore { get; set; }
        public ConsoleColor Header_Back { get; set; }

        private string FileName;
        public int SelectedX { get; set; } = 0; //č. řádku na obrazovce
        public int SelectedY { get; set; } = 0; //č.sloupce
        public int Top { get; set; } = 0; //počet ř. mimo obrazovku
        public int Left { get; set; } = 0;
        public List<List<char>> Data { get; set; }

        private int HighlightStartX;
        private int HighlightStartY;
        private bool HighlightOn = false;




        public TextEditor(bool isSelected, int x, int y, int w, int h, 
            FileUtils service, ConsoleColor highlight_fore, ConsoleColor highlight_back, 
            ConsoleColor normal_fore, ConsoleColor normal_back, ConsoleColor header_fore, ConsoleColor header_back)
        {
            this.CurrFile = service;
            this.Data = service.GetContentOfFile();
            if (this.Data.Count<1)
            {
                this.Data.Add(new List<char>() { ' ' });
            }
            this.IsSelected = isSelected;
            this.X = x;
            this.Y = y;
            this.Height = h;
            this.Width = w;
            this.Drawer = new ConsoleUtils2(x, y+1, w, h-1, normal_fore, normal_back);
            this.Header_Drawer = new ConsoleUtils2(x, y, w, 1, header_fore, header_back);
            this.HighLight_Fore = highlight_fore;
            this.Highlight_Back = highlight_back;
            this.Normal_Fore = normal_fore;
            this.Normal_Back = normal_back;
            this.Header_Fore = header_fore;
            this.Header_Back = header_back;

          

            this.FileName = this.CurrFile.GetFileName();
        }

        public void DrawHeader()
        {
            //TODO - ASCII CODE AND HEX CODE
            string fileName = this.FileName;
            int columnIndex = this.SelectedX+1;
            int linesOutsideArea = (this.Top + 1);
            int relativeLinePos = this.SelectedY;
            int absoluteLinePos = (this.SelectedY)+1;
            int totalLines = /*this.Data.Count*/0;
            int absoluteCharIndex = /*this.GetCurrentCharPos()*/0;
            int totalChars = /*this.GetTotalCharsCount()*/0;
            string hex = /*BitConverter.ToString(new byte[] { Convert.ToByte(this.Data[this.SelectedY][this.SelectedX]) })*/"kencur";
            int ascii = /*((int)this.Data[this.SelectedY][this.SelectedX])*/0;
            string save_text = this.CurrFile.FileDataChanged == false ? "SAVED" : "UNSAVED";
            this.Header_Drawer.ResetToOrigin();

            this.Header_Drawer.WriteLine($"{fileName}{"".PadRight(10, ' ')}{columnIndex} L:[ " +
                $"{linesOutsideArea}+ {relativeLinePos}   {absoluteLinePos}/{totalLines}] *" +
                $"({absoluteCharIndex}/{totalChars}) "+$"{ascii}".PadLeft(4, '0') + $"  0x{hex}" + $"      {save_text}".PadRight(this.Header_Drawer.MaxWidthWrite,' '));
        }
        public void DrawData()
        {
            this.Drawer.ResetToOrigin();
            for (int i = this.Top; i < this.Top + this.Drawer.MaxHeightWrite; i++)
            {
                int currDrawX;
                int currDrawY;
                if (i < this.Data.Count && this.Data[i].Count > 0 && this.Data[i].Count > this.Left )
                {
                    string temp = new string(this.Data[i].ToArray(), this.Left, this.Data[i].Count - this.Left);
                    //this.Drawer.WriteLine(new string(this.Data[i].ToArray(),this.Left,this.Data[i].Count-this.Left));
                    this.Drawer.WriteLine(temp);
                    currDrawX = this.Drawer.CursorX;
                    currDrawY = this.Drawer.CursorY;
                    //Divné ale rychlé vykreslování
                    for (int j = 0; j < temp.Length; j++)
                    {
                        if (!HighlightOn)
                        {
                            if (j == this.SelectedX - this.Left && i == this.SelectedY)
                            {
                                this.Drawer.CursorX = this.SelectedX - this.Left;
                                this.Drawer.CursorY = this.SelectedY - this.Top + 1;
                                this.Drawer.BackColor = this.Highlight_Back;
                                this.Drawer.ForeColor = this.HighLight_Fore;
                                this.Drawer.Write(temp[j].ToString());
                            }
                        }
                        else
                        {
                            /*CELKEm Důležité funkce*/

                            //if (i == this.HighlightStartY && j == this.HighlightStartX - this.Left)
                            //{
                            //    this.Draw1(j + this.Left, i, temp); //middle to end
                            //}
                            //if (i == this.HighlightStartY && j == this.HighlightStartX - this.Left)
                            //{
                            //    this.Draw2(j + this.Left, i, temp); //middle to start
                            //}
                            //if (i == this.HighlightStartY && j == this.HighlightStartX - this.Left)
                            //{
                            //    this.Draw3(i, temp); //whole line
                            //}
                            if (i == this.HighlightStartY && ((j >= this.HighlightStartX - this.Left && j<=this.SelectedX-this.Left) || (j >= this.SelectedX - this.Left && j <= this.HighlightStartX - this.Left)))
                            {
                                this.Draw4(j+this.Left,i, temp); //middle to Selected
                            }
                        }
                    }
                    this.Drawer.BackColor = this.Normal_Back;
                    this.Drawer.ForeColor = this.Normal_Fore;
                    this.Drawer.CursorX = currDrawX;
                    this.Drawer.CursorY = currDrawY;
                }
                else
                {
                    this.Drawer.WriteLine(new string(' ', this.Drawer.MaxWidthWrite));
                }
            }
        }
        public void Draw()
        {
            if (this.IsSelected || this.Redraw)
            {
                this.DrawHeader();
                this.DrawData();
                Console.CursorVisible = true;
                Console.SetCursorPosition(this.SelectedX-this.Left,this.SelectedY-this.Top+1);
                this.Redraw = false;
            }
        }
        public void Y_Down()
        {
            if (this.SelectedY < this.Data.Count-1)
            {
                this.SelectedY++;

                if (this.SelectedY == this.Top+this.Drawer.MaxHeightWrite)
                {
                    this.Top++;
                }
            }
        }
        public void Y_Up()
        {
            if (this.SelectedY > 0)
            {
                this.SelectedY--;

                if (this.SelectedY == this.Top-1)
                {
                    this.Top--;
                }
            }
        }
        public void X_Left()
        {
            if (this.SelectedX > 0)
            {
                this.SelectedX--;

                if (this.SelectedX == this.Left - 1)
                {
                    this.Left--;
                }
            }
            else
            {
                if (this.SelectedY > 0)
                {
                    this.Y_Up();
                    this.SetXToTheEndOfLine(this.SelectedY);
                    this.X_CheckIfOutsideLeft();
                }
            }
        }
        public void X_Right()
        {
            if (this.SelectedX < this.Data[this.SelectedY].Count - 1)
            {
                this.SelectedX++;

                if (this.SelectedX == this.Left + this.Drawer.MaxWidthWrite)
                {
                    this.Left++;
                }
            }
            else
            {
                if (this.SelectedY < this.Data.Count - 1)
                {
                    this.Y_Down();
                    this.SelectedX = 0;
                    this.X_CheckIfOutsideLeft();
                }
            }
        }
        public void X_CheckIfOutsideDataCount()
        {
            if (this.Data[this.SelectedY].Count - 1 < this.SelectedX + this.Left)
            {
                this.SetXToTheEndOfLine( this.SelectedY);
               
            }
        }
        public void X_CheckIfOutsideLeft()
        {

            if (this.SelectedX <= this.Left - 1)
            {
                this.Left = Math.Max(this.SelectedX - 5, 0);
            }
            if (this.SelectedX >= this.Left + this.Drawer.MaxWidthWrite)
            {
                this.Left = Math.Max(this.SelectedX - 5, 0);
            }
        }
        public void SetXToTheEndOfLine(int lineAbsoluteIndex)
        {
            this.SelectedX = this.Data[lineAbsoluteIndex].Count - 1;
        }
        public void HandleKey(ConsoleKeyInfo key)
        {
            if (this.IsSelected)
            {
                if (key.Key == ConsoleKey.UpArrow)
                {
                    this.Y_Up();
                    this.X_CheckIfOutsideDataCount();
                    this.X_CheckIfOutsideLeft();
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    this.Y_Down();
                    this.X_CheckIfOutsideDataCount();

                    this.X_CheckIfOutsideLeft();
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    this.X_Left();
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    this.X_Right();
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (this.SelectedX == 0)
                    {
                        this.Data.Insert(this.SelectedY, new List<char>() { ' ' });
                        this.Y_Down();
                    }
                    else
                    {
                        if (this.SelectedX == this.Data[this.SelectedY].Count - 1)
                        {
                            this.Data.Insert(this.SelectedY + 1, new List<char>() { ' ' });
                            this.Y_Down();
                            this.SelectedX = 0;

                            this.X_CheckIfOutsideLeft();
                        }
                        else
                        {
                            if (this.SelectedX > 0 && this.SelectedX < this.Data[this.SelectedY].Count - 1)
                            {
                                List<char> temp = this.Data[this.SelectedY].GetRange(this.SelectedX, this.Data[this.SelectedY].Count - this.SelectedX);
                                this.Data.Insert(this.SelectedY + 1, temp);
                                this.Data[this.SelectedY].RemoveRange(this.SelectedX, this.Data[this.SelectedY].Count - this.SelectedX);
                                this.Data[this.SelectedY].Add(' ');

                                this.Y_Down();
                                this.SelectedX = 0; 

                                this.X_CheckIfOutsideLeft();
                            }
                        }
                    }
                    this.CurrFile.FileDataChanged = true;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                   



                    if (this.Data[this.SelectedY].Count > 1)
                    {

                        if (this.SelectedY > 0 && this.SelectedX == 0)
                        {
                            this.SetXToTheEndOfLine(this.SelectedY - 1);
                            this.Data[this.SelectedY - 1].RemoveAt(this.Data[this.SelectedY - 1].Count - 1);
                            this.Data[this.SelectedY - 1].AddRange(this.Data[this.SelectedY]);
                            this.Data.RemoveAt(this.SelectedY);
                            this.Y_Up();
                            this.X_CheckIfOutsideDataCount();

                            this.X_CheckIfOutsideLeft();
                         

                        }
                        else
                        {
                            if (this.SelectedX > 0)
                            {
                                this.Data[this.SelectedY].RemoveAt(this.SelectedX - 1);
                                this.X_Left();
                                this.X_CheckIfOutsideLeft();
                            }
                        }
                    }
                    else
                    {
                        if (this.SelectedY > 0 && this.SelectedX == 0)
                        {
                            this.Data.RemoveAt(this.SelectedY);
                            this.Y_Up();
                            this.SetXToTheEndOfLine(this.SelectedY);
                            this.X_CheckIfOutsideLeft();
                        }
                    }
                    this.CurrFile.FileDataChanged = true;


                }
                else if (key.Key == ConsoleKey.F2 || key.Key == ConsoleKey.F10)
                {
                    this.CurrFile.tempData = this.Data;
                } 
                else if (key.Key == ConsoleKey.F3)
                {
                    this.HighlightOn = true;
                    this.HighlightStartX = this.SelectedX;
                    this.HighlightStartY = this.SelectedY;
                }
                else
                {
                    if (!char.IsControl(key.KeyChar))
                    {
                        this.Data[ this.SelectedY].Insert(this.SelectedX, key.KeyChar);
                        this.X_Right();
                        this.CurrFile.FileDataChanged = true;

                    }
                }
            }
        }

        public void Update()
        {
            
        }

        private int GetCurrentCharPos()
        {
            int currentPos = 0;
            for (int i = 0; i < this.SelectedY; i++)
            {
                    currentPos = currentPos + this.Data[i].Count;
            }
            currentPos = currentPos + this.SelectedX+1;
            return currentPos;
        }
        private int GetTotalCharsCount()
        {
            int totalcount = 0;
            foreach (List<char> item in this.Data)
            {
                totalcount = totalcount + item.Count;
            }
            return totalcount;
        }

        //REFERENCE FUNCTION - DO NOT CALL
        //REFERENCE FUNCTION - DO NOT CALL
        //REFERENCE FUNCTION - DO NOT CALL
        //REFERENCE FUNCTION - DO NOT CALL
        //REFERENCE FUNCTION - DO NOT CALL
        //REFERENCE FUNCTION - DO NOT CALL
        //REFERENCE FUNCTION - DO NOT CALL
        public void Highlight(int currAbsPosX,int currAbsPosY,string line)
        {
                int topY = this.HighlightStartY < this.SelectedY ? this.HighlightStartY : this.SelectedY;
                int botY = this.HighlightStartY < this.SelectedY ? this.SelectedY : this.HighlightStartY;


                if (topY < botY)
                {
                int topX = this.HighlightStartY < this.SelectedY ? this.HighlightStartX : this.SelectedX;
                int botX = this.HighlightStartY < this.SelectedY ? this.SelectedX : this.HighlightStartX;
                if (currAbsPosY >= topY && currAbsPosY <= topY)
                    {
                        if (currAbsPosY == topY && currAbsPosX >= topX)
                        {
                            this.Drawer.CursorY = topY - this.Top + 1;
                            this.Drawer.CursorX = topX - this.Left;
                            this.Drawer.BackColor = this.Highlight_Back;
                            this.Drawer.ForeColor = this.HighLight_Fore;
                            this.Drawer.Write(line.Substring(topX - this.Left));
                        }
                        if (currAbsPosY == botY && currAbsPosX <= botX)
                        {
                            this.Drawer.CursorX = 0;
                            this.Drawer.CursorY = botY - this.Top + 1;
                            this.Drawer.BackColor = this.Highlight_Back;
                            this.Drawer.ForeColor = this.HighLight_Fore;
                            this.Drawer.Write(line.Substring(0, botX - this.Left));
                        }
                        if (currAbsPosY < botY && currAbsPosY > topY)
                        {
                            this.Drawer.CursorX = 0;
                            this.Drawer.CursorY = currAbsPosY - this.Top + 1;
                            this.Drawer.BackColor = this.Highlight_Back;
                            this.Drawer.ForeColor = this.HighLight_Fore;
                            this.Drawer.Write(line);
                        }
                    }
                }
                else
            {
                int firstX = this.HighlightStartX < this.SelectedX ? this.HighlightStartX : this.SelectedX;
                int lastX = this.HighlightStartX < this.SelectedX ? this.SelectedX : this.HighlightStartX;
                this.Drawer.CursorX = firstX - this.Left;
                this.Drawer.CursorY = topY - this.Top + 1;
                this.Drawer.BackColor = this.Highlight_Back;
                this.Drawer.ForeColor = this.HighLight_Fore;
                if (line.Length == 1)
                {
                    this.Drawer.Write(" ");
                }
                else
                {
                    this.Drawer.Write(line.Substring(firstX - this.Left, lastX - firstX - 2 * this.Left + 1));
                }
            }
            
        }
        public void Draw1(int startX,int startY,string line)
        {
            this.Drawer.CursorY = startY- this.Top + 1;
            this.Drawer.CursorX = startX - this.Left;
            this.Drawer.BackColor = this.Highlight_Back;
            this.Drawer.ForeColor = this.HighLight_Fore;
            this.Drawer.Write(line.Substring(startX - this.Left));
        }
        public void Draw2(int startX, int startY, string line)
        {
            this.Drawer.CursorX = 0;
            this.Drawer.CursorY = startY - this.Top + 1;
            this.Drawer.BackColor = this.Highlight_Back;
            this.Drawer.ForeColor = this.HighLight_Fore;
            this.Drawer.Write(line.Substring(0, startX - this.Left));
        }
        public void Draw3(int startY, string line)
        {
            this.Drawer.CursorX = 0;
            this.Drawer.CursorY = startY - this.Top + 1;
            this.Drawer.BackColor = this.Highlight_Back;
            this.Drawer.ForeColor = this.HighLight_Fore;
            this.Drawer.Write(line);
        }
        public void Draw4(int startX, int startY, string line)
        {
            int firstX = this.SelectedX > startX ? startX : this.SelectedX;
            int lastX = this.SelectedX > startX ? this.SelectedX : startX;

            this.Drawer.CursorX = firstX - this.Left;
            this.Drawer.CursorY = startY - this.Top + 1;
            this.Drawer.BackColor = this.Highlight_Back;
            this.Drawer.ForeColor = this.HighLight_Fore;
            this.Drawer.Write(line.Substring(firstX - this.Left, lastX-firstX));
        }
    }
}
