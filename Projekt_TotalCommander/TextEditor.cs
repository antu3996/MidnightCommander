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

        private int HighlightStartX = -1;
        private int HighlightStartY = -1;
        private int HighlightEndX = -1;
        private int HighlightEndY = -1;
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
            int absoluteCharIndex = /*this.GetCurrentCharPos()*/this.HighlightStartX;
            int totalChars = /*this.GetTotalCharsCount()*/this.HighlightStartY;
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
                    if (!this.HighlightOn)
                    {
                        for (int j = 0; j < temp.Length; j++)
                        {

                            if (j == this.SelectedX - this.Left && i == this.SelectedY)
                            {
                                this.Drawer.CursorX = this.SelectedX - this.Left+this.Drawer.InitX;
                                this.Drawer.CursorY = this.SelectedY - this.Top +this.Drawer.InitY;
                                this.Drawer.BackColor = this.Highlight_Back;
                                this.Drawer.ForeColor = this.HighLight_Fore;
                                this.Drawer.Write(temp[j].ToString());
                            }

                            //else
                            //{

                            //}
                        }
                    }
                    //else
                    //{
                    if (this.HighlightStartX > -1 && this.HighlightStartY > -1 && this.HighlightEndX > -1 && this.HighlightEndY > -1)
                    {
                        int topY = this.HighlightStartY < this.HighlightEndY ? this.HighlightStartY : this.HighlightEndY;
                        int botY = this.HighlightStartY < this.HighlightEndY ? this.HighlightEndY : this.HighlightStartY;
                        int topX = this.HighlightStartY < this.HighlightEndY ? this.HighlightStartX : this.HighlightEndX;
                        int botX = this.HighlightStartY < this.HighlightEndY ? this.HighlightEndX : this.HighlightStartX;
                        /*CELKEm Důležité funkce*/
                        if (topY < botY)
                        {
                            if (i == topY /*&& j == topX - this.Left*/)
                            {
                                this.BetweenToEnd(topX, i, temp); //middle to end
                            }
                            if (i == botY /*&& j == botX - this.Left*/)
                            {
                                this.StartToBetween(botX, i, temp); //middle to start
                            }
                            if (i > topY && i < botY && (botY - topY > 1))
                            {
                                this.StartToEnd(i, temp); //whole line
                            }
                        }
                        if (topY == botY)
                        {
                            int firstX = topX < botX ? topX : botX;
                            int lastX = topX < botX ? botX : topX;
                            if (i == topY || i == botY)
                            {
                                this.BetweenToBetween(firstX, lastX, i, temp); //middle to Selected
                            }
                        }
                    }
                    //}
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
                Console.SetCursorPosition(this.SelectedX - this.Left+this.Drawer.InitX, this.SelectedY - this.Top+ this.Drawer.InitY);
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
                    
                    if (this.HighlightOn)
                    {
                        this.HighlightEndX = this.SelectedX;
                        this.HighlightEndY = this.SelectedY;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    this.Y_Down();
                    this.X_CheckIfOutsideDataCount();

                    this.X_CheckIfOutsideLeft();

                    if (this.HighlightOn)
                    {
                        this.HighlightEndX = this.SelectedX;
                        this.HighlightEndY = this.SelectedY;
                    }
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    this.X_Left();

                    if (this.HighlightOn)
                    {
                        this.HighlightEndX = this.SelectedX;
                        this.HighlightEndY = this.SelectedY;
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    this.X_Right();


                    if (this.HighlightOn)
                    {
                        this.HighlightEndX = this.SelectedX;
                        this.HighlightEndY = this.SelectedY;
                    }
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (this.SelectedX == 0)
                    {
                        this.Data.Insert(this.SelectedY, new List<char>() { '¬' });
                        this.Y_Down();
                    }
                    else
                    {
                        if (this.SelectedX == this.Data[this.SelectedY].Count - 1)
                        {
                            this.Data.Insert(this.SelectedY + 1, new List<char>() { '¬' });
                            this.Y_Down();
                            this.SelectedX = 0;

                            this.X_CheckIfOutsideLeft();
                        }
                        else
                        {
                            if (this.SelectedX > 0 && this.SelectedX < this.Data[this.SelectedY].Count - 1)
                            {
                                List<char> temp = new List<char>(this.Data[this.SelectedY].GetRange(this.SelectedX, this.Data[this.SelectedY].Count - this.SelectedX));
                                this.Data.Insert(this.SelectedY + 1, temp);
                                this.Data[this.SelectedY].RemoveRange(this.SelectedX, this.Data[this.SelectedY].Count - this.SelectedX);
                                this.Data[this.SelectedY].Add('¬');

                                this.Y_Down();
                                this.SelectedX = 0; 

                                this.X_CheckIfOutsideLeft();
                            }
                        }
                    }
                    this.CurrFile.FileDataChanged = true;
                    if (this.HighlightOn)
                    {
                        this.HighlightEndX = this.SelectedX;
                        this.HighlightEndY = this.SelectedY;
                    }
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
                    if (this.HighlightOn)
                    {
                        this.HighlightEndX = this.SelectedX;
                        this.HighlightEndY = this.SelectedY;
                    }


                }
                else if (key.Key == ConsoleKey.F2 || key.Key == ConsoleKey.F10)
                {
                    this.CurrFile.tempData = this.Data;
                } 
                else if (key.Key == ConsoleKey.F3)
                {
                    if (HighlightOn == false)
                    {
                        this.HighlightOn = true;
                        this.HighlightStartX = this.SelectedX;
                        this.HighlightStartY = this.SelectedY;
                        this.HighlightEndX = this.SelectedX;
                        this.HighlightEndY = this.SelectedY;
                    }
                    else
                    {
                        this.HighlightOn = false;
                    }
                }
                else if (key.Key == ConsoleKey.F5)
                {
                    this.CopyHighlightToNewLocation(this.SelectedX, this.SelectedY);
                }
                else
                {
                    if (!char.IsControl(key.KeyChar))
                    {
                        this.Data[ this.SelectedY].Insert(this.SelectedX, key.KeyChar);
                        this.X_Right();
                        this.CurrFile.FileDataChanged = true;

                        if (this.HighlightOn)
                        {
                            this.HighlightEndX = this.SelectedX;
                            this.HighlightEndY = this.SelectedY;
                        }
                    }
                }
            }
        }

        public void Update()
        {
            
        }

        public void CopyTextToNewPlace(List<List<char>> copyText, int newX,int newY)
        {
            for (int i = this.HighlightStartY; i < this.Data.Count; i++)
            {
            }
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
        /*public void Highlight(int currAbsPosX,int currAbsPosY,string line)
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
                            this.Drawer.CursorY = topY - this.Top;
                            this.Drawer.CursorX = topX - this.Left;
                            this.Drawer.BackColor = this.Highlight_Back;
                            this.Drawer.ForeColor = this.HighLight_Fore;
                            this.Drawer.Write(line.Substring(topX - this.Left));
                        }
                        if (currAbsPosY == botY && currAbsPosX <= botX)
                        {
                            this.Drawer.CursorX = 0;
                            this.Drawer.CursorY = botY - this.Top;
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
            
        }*/
        public void BetweenToEnd(int startX,int startY,string line)
        {
            //middle to end
            if (line.Length > 0)
            {
                int actualStartX = this.Left > startX ? this.Left : startX;
                this.Drawer.CursorY = startY - this.Top + this.Drawer.InitY;
                this.Drawer.CursorX = actualStartX - this.Left + this.Drawer.InitX;
                this.Drawer.BackColor = this.Highlight_Back;
                this.Drawer.ForeColor = this.HighLight_Fore;
                this.Drawer.Write(line.Substring(actualStartX - this.Left));
            }
        }
        public void StartToBetween(int startX, int startY, string line)
        {
            //middle to start

            if (line.Length > 0)
            {
                int actualStartX = this.Left > startX ? this.Left : startX;
                this.Drawer.CursorX = 0 + this.Drawer.InitX;
                this.Drawer.CursorY = startY - this.Top  + this.Drawer.InitY;
                this.Drawer.BackColor = this.Highlight_Back;
                this.Drawer.ForeColor = this.HighLight_Fore;
                this.Drawer.Write(line.Substring(0, actualStartX - this.Left+1));
            }
        }
        public void StartToEnd(int startY, string line)
        {
            //whole line
            if (line.Length > 0)
            {
                this.Drawer.CursorX = 0 + this.Drawer.InitX;
                this.Drawer.CursorY = startY - this.Top + this.Drawer.InitY;
                this.Drawer.BackColor = this.Highlight_Back;
                this.Drawer.ForeColor = this.HighLight_Fore;
                this.Drawer.Write(line);
            }
        }
        public void BetweenToBetween(int startX,int endX,int Y, string line)
        {
            //middle to selected in one line
            if (line.Length > 0)
            {
                int actualStartX = this.Left > startX ? this.Left : startX;
                int actualEndX = endX < this.Left+this.Drawer.MaxWidthWrite ? endX: this.Left + this.Drawer.MaxWidthWrite;
                this.Drawer.CursorX = actualStartX - this.Left + this.Drawer.InitX;
                this.Drawer.CursorY = Y - this.Top  + this.Drawer.InitY;
                this.Drawer.BackColor = this.Highlight_Back;
                this.Drawer.ForeColor = this.HighLight_Fore;
                this.Drawer.Write(line.Substring(actualStartX - this.Left, actualEndX-actualStartX+1));
            }
        }

        public void CopyHighlightToNewLocation(int newX, int newY)
        {
            List<char> currentLine = new List<char>(this.Data[newY]);
            int topY = this.HighlightStartY < this.HighlightEndY ? this.HighlightStartY : this.HighlightEndY;
            int botY = this.HighlightStartY < this.HighlightEndY ? this.HighlightEndY : this.HighlightStartY;
            int topX = this.HighlightStartY < this.HighlightEndY ? this.HighlightStartX : this.HighlightEndX;
            int botX = this.HighlightStartY < this.HighlightEndY ? this.HighlightEndX : this.HighlightStartX;

            if (topY < botY)
            {
                List<List<char>> copyData = new List<List<char>>();
                for (int i = topY; i <= botY; i++)
                {
                    if (i == topY)
                    {
                        List<char> line = new List<char>(this.Data[i].GetRange(topX, this.Data[i].Count - topX));
                        copyData.Add(line);
                    }
                    else if (i == botY)
                    {
                        List<char> line = new List<char>(this.Data[i].GetRange(0, botX + 1));
                        copyData.Add(line);
                    }
                    else
                    {
                        copyData.Add(new List<char>(this.Data[i]));
                    }
                }
                copyData[0].InsertRange(0, currentLine.GetRange(0, newX));
                if (copyData[copyData.Count - 1].Last() == '¬')
                {
                    copyData.Add(new List<char>());
                }
                copyData[copyData.Count - 1].AddRange(currentLine.GetRange(newX, currentLine.Count - newX));
                this.Data.RemoveAt(newY);
                this.Data.InsertRange(newY, copyData);
            }
            if (topY == botY)
            {
                int firstX = topX < botX ? topX : botX;
                int lastX = topX < botX ? botX : topX;
                List<char> copyline = new List<char>(this.Data[topY].GetRange(firstX, lastX - firstX + 1));
                if (copyline.Last() == '¬')
                {
                    this.Data.Insert(newY + 1, new List<char>());
                    this.Data[newY + 1].InsertRange(0,currentLine.GetRange(newX,currentLine.Count- newX));
                    this.Data[newY].RemoveRange(newX, currentLine.Count - newX);
                    this.Data[newY].InsertRange(newX, copyline);
                }
                else
                {
                    this.Data[newY].InsertRange(newX, copyline);
                }
            }
        }
    }
}
