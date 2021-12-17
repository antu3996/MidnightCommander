using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    //REFERENČNÍ
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
        public ConsoleColor Selected_Back { get; set; }
        public ConsoleColor Selected_Fore { get; set; }

        private string FileName;
        public int SelectedX { get; set; } = 0; //č. řádku na obrazovce
        public int SelectedY { get; set; } = 0; //č.sloupce
        public int Top { get; set; } = 0; //počet ř. mimo obrazovku
        public int Left { get; set; } = 0;
        public List<List<char>> Data { get; set; }

        public bool FindingMode { get; set; } = false;
        public int HighlightStartX = 0;
        public int HighlightStartY = 0;
        public int HighlightEndX = 0;
        public int HighlightEndY = 0;
        private bool HighlightOn = false;
        private bool ShowHighlight = false;
        public bool FindingInit = true;
        private int HighlightLengthFromOrigin = 0;
        private int HighlightLength = 0;

        public int f_HighlightStartX = 0;
        public int f_HighlightStartY = 0;
        public int f_HighlightEndX = 0;
        public int f_HighlightEndY = 0;



        public TextEditor(bool isSelected, int x, int y, int w, int h,
            FileUtils service, ConsoleColor highlight_fore, ConsoleColor highlight_back,
            ConsoleColor normal_fore, ConsoleColor normal_back, ConsoleColor header_fore, ConsoleColor header_back, ConsoleColor selected_Fore, ConsoleColor selected_Back)
        {
            this.CurrFile = service;
            ////////////this.Data = new List<List<char>>(service.GetContentOfFile());
            if (this.Data.Count < 1)
            {
                this.Data.Add(new List<char>() { '¬' });
            }
            this.IsSelected = isSelected;
            this.X = x;
            this.Y = y;
            this.Height = h;
            this.Width = w;
            this.Drawer = new ConsoleUtils2(x, y + 1, w, h - 1, normal_fore, normal_back);
            this.Header_Drawer = new ConsoleUtils2(x, y, w, 1, header_fore, header_back);
            this.HighLight_Fore = highlight_fore;
            this.Highlight_Back = highlight_back;
            this.Normal_Fore = normal_fore;
            this.Normal_Back = normal_back;
            this.Header_Fore = header_fore;
            this.Header_Back = header_back;
            this.Selected_Back = selected_Back;
            this.Selected_Fore = selected_Fore;


            this.FileName = this.CurrFile.GetFileName();
        }

        public void DrawHeader()
        {
            //TODO - ASCII CODE AND HEX CODE
            string fileName = this.FileName;
            int columnIndex = this.SelectedX + 1;
            int linesOutsideArea = (this.Top + 1);
            int relativeLinePos = this.SelectedY;
            int absoluteLinePos = (this.SelectedY) + 1;
            int totalLines = this.Data.Count;
            int absoluteCharIndex = this.GetCurrentCharPos();
            int totalChars = this.GetTotalCharsCount();
            string hex = /*BitConverter.ToString(new byte[] { Convert.ToByte(this.Data[this.SelectedY][this.SelectedX]) })*/"";
            int ascii = ((int)this.Data[this.SelectedY][this.SelectedX]);
            string save_text = this.CurrFile.FileDataChanged == false ? "SAVED" : "UNSAVED";
            this.Header_Drawer.ResetToOrigin();

            this.Header_Drawer.WriteLine($"{fileName}{"".PadRight(10, ' ')}{columnIndex} L:[ " +
                $"{linesOutsideArea}+ {relativeLinePos}   {absoluteLinePos}/{totalLines}] *" +
                $"({absoluteCharIndex}/{totalChars}) " + $"{ascii}".PadLeft(4, '0') + $"  0x{hex}" + $"      {save_text}".PadRight(this.Header_Drawer.MaxWidthWrite, ' '));
        }
        public void DrawData()
        {
            this.Drawer.ResetToOrigin();
            for (int i = this.Top; i < this.Top + this.Drawer.MaxHeightWrite; i++)
            {
                int currDrawX;
                int currDrawY;
                if (i < this.Data.Count && this.Data[i].Count > 0 && this.Data[i].Count > this.Left)
                {
                    string temp = new string(this.ToArrayV2(this.Data[i],this.Data[i].Count), this.Left, this.Data[i].Count - this.Left);
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
                                this.Drawer.CursorX = this.SelectedX - this.Left + this.Drawer.InitX;
                                this.Drawer.CursorY = this.SelectedY - this.Top + this.Drawer.InitY;
                                this.Drawer.BackColor = this.Selected_Back;
                                this.Drawer.ForeColor = this.Selected_Fore;
                                this.Drawer.Write(temp[j].ToString());
                            }
                        }
                    }
                    if (ShowHighlight)
                    {
                        this.DoHighlight(i, temp);
                    }
                    this.Drawer.BackColor = this.Normal_Back;
                    this.Drawer.ForeColor = this.Normal_Fore;
                    this.Drawer.CursorX = currDrawX;
                    this.Drawer.CursorY = currDrawY;
                }
                else
                {
                    this.Drawer.WriteLine("".PadRight(this.Drawer.MaxWidthWrite,' '));
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
                Console.SetCursorPosition(this.SelectedX - this.Left + this.Drawer.InitX, this.SelectedY - this.Top + this.Drawer.InitY);
                this.Redraw = false;
            }
        }
        public void Y_Down()
        {
            if (this.SelectedY < this.Data.Count - 1)
            {
                this.SelectedY++;

                if (this.SelectedY == this.Top + this.Drawer.MaxHeightWrite)
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

                if (this.SelectedY == this.Top - 1)
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
                this.SetXToTheEndOfLine(this.SelectedY);

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
        public void Y_CheckIfOutsideTop()
        {
            if (this.SelectedY <= this.Top - 1)
            {
                this.Top = Math.Max(this.SelectedY - 5, 0);
            }
            if (this.SelectedY >= this.Top + this.Drawer.MaxHeightWrite)
            {
                this.Top = Math.Max(this.SelectedY - 5, 0);
            }
        }
        public void Y_CheckIfOutsideDataCount()
        {
            if (this.Data.Count - 1 < this.SelectedY + this.Top)
            {
                this.SelectedY = this.Data.Count - 1;
                this.X_CheckIfOutsideDataCount();
                this.X_CheckIfOutsideLeft();
            }
        }
        public void SetXToTheEndOfLine(int lineAbsoluteIndex)
        {
            this.SelectedX = this.Data[lineAbsoluteIndex].Count - 1;
        }
        public void CheckIfDataEmpty()
        {
            if (this.Data.Count == 0 || this.Data[0].Count == 0)
            {
                this.Data.Add(new List<char>() { '¬' });
                this.SelectedX = 0;
                this.SelectedY = 0;
                this.X_CheckIfOutsideLeft();
                this.Y_CheckIfOutsideTop();
            }
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
                    //if (this.HighlightOn)
                    //{
                    //    this.HighlightEndX = this.SelectedX;
                    //    this.HighlightEndY = this.SelectedY;
                    //}
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
                    //if (this.HighlightOn)
                    //{
                    //    this.HighlightEndX = this.SelectedX;
                    //    this.HighlightEndY = this.SelectedY;
                    //}


                }
                else if (key.Key == ConsoleKey.F2 || key.Key == ConsoleKey.F10)
                {
                    ////////this.CurrFile.tempData = this.Data;
                }
                else
                {
                    if (!char.IsControl(key.KeyChar))
                    {
                        this.Data[this.SelectedY].Insert(this.SelectedX, key.KeyChar);
                        this.X_Right();
                        this.CurrFile.FileDataChanged = true;

                        //if (this.HighlightOn)
                        //{
                        //    this.HighlightEndX = this.SelectedX;
                        //    this.HighlightEndY = this.SelectedY;
                        //}
                    }
                }
            }
        }
        public void TurnOnHighlight()
        {
            this.ShowHighlight = true;

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
                if (this.HighlightEndY < this.HighlightStartY)
                {
                    int temp = this.HighlightStartY;
                    this.HighlightStartY = this.HighlightEndY;
                    this.HighlightEndY = temp;
                    int temp2 = this.HighlightEndX;
                    this.HighlightEndX = this.HighlightStartX;
                    this.HighlightStartX = temp2;
                }
            }
        }
        private void UpdateHighlightPos()
        {
            this.HighlightLengthFromOrigin = this.GetLengthBetweenTwoPoints(0, 0, this.HighlightStartX, this.HighlightStartY, this.Data);
            this.HighlightLength = this.GetLengthBetweenTwoPoints(this.HighlightStartX, this.HighlightStartY, this.HighlightEndX, this.HighlightEndY, this.Data);
        }
        public void Update()
        {
            if (HighlightOn)
            {
                this.UpdateHighlightPos();
            }
        }

        private int GetCurrentCharPos()
        {
            int currentPos = 0;
            for (int i = 0; i < this.SelectedY; i++)
            {
                currentPos = currentPos + this.Data[i].Count;
            }
            currentPos = currentPos + this.SelectedX + 1;
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

        public void BetweenToEnd(int startX, int startY, string line)
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
                this.Drawer.CursorY = startY - this.Top + this.Drawer.InitY;
                this.Drawer.BackColor = this.Highlight_Back;
                this.Drawer.ForeColor = this.HighLight_Fore;
                this.Drawer.Write(line.Substring(0, actualStartX - this.Left + 1));
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
        public void BetweenToBetween(int startX, int endX, int Y, string line)
        {
            //middle to selected in one line
            if (line.Length > 0)
            {
                int actualStartX = this.Left > startX ? this.Left : startX;
                int actualEndX = endX < this.Left + this.Drawer.MaxWidthWrite ? endX : this.Left + this.Drawer.MaxWidthWrite;
                this.Drawer.CursorX = actualStartX - this.Left + this.Drawer.InitX;
                this.Drawer.CursorY = Y - this.Top + this.Drawer.InitY;
                this.Drawer.BackColor = this.Highlight_Back;
                this.Drawer.ForeColor = this.HighLight_Fore;
                this.Drawer.Write(line.Substring(actualStartX - this.Left, actualEndX - actualStartX + 1));
            }
        }

        public void DoHighlight(int currentDrawY, string currentline)
        {
            int topY = this.HighlightStartY < this.HighlightEndY ? this.HighlightStartY : this.HighlightEndY;
            int botY = this.HighlightStartY < this.HighlightEndY ? this.HighlightEndY : this.HighlightStartY;
            int topX = this.HighlightStartY < this.HighlightEndY ? this.HighlightStartX : this.HighlightEndX;
            int botX = this.HighlightStartY < this.HighlightEndY ? this.HighlightEndX : this.HighlightStartX;
            /*CELKEm Důležité funkce*/
            if (topY < botY)
            {
                if (currentDrawY == topY /*&& j == topX - this.Left*/)
                {
                    this.BetweenToEnd(topX, currentDrawY, currentline); //middle to end
                }
                if (currentDrawY == botY /*&& j == botX - this.Left*/)
                {
                    this.StartToBetween(botX, currentDrawY, currentline); //middle to start
                }
                if (currentDrawY > topY && currentDrawY < botY && (botY - topY > 1))
                {
                    this.StartToEnd(currentDrawY, currentline); //whole line
                }
            }
            if (topY == botY)
            {
                int firstX = topX < botX ? topX : botX;
                int lastX = topX < botX ? botX : topX;
                if (currentDrawY == topY || currentDrawY == botY)
                {
                    this.BetweenToBetween(firstX, lastX, currentDrawY, currentline); //middle to Selected
                }
            }
        }

        public void DeleteHighlight(int startX,int startY,int endX,int endY,bool keepHighlight)
        {
            if (!HighlightOn && ShowHighlight)
            {
                this.CurrFile.FileDataChanged = true;


                List<char> currentLine = new List<char>();
                int topY = startY < endY ? startY : endY;
                int botY = startY < endY ? endY : startY;
                int topX = startY < endY ? startX: endX;
                int botX = startY < endY ? endX : startX;
                if (topY < botY)
                {
                    currentLine.AddRange(new List<char>(this.Data[topY].GetRange(0, topX)));
                    currentLine.AddRange(new List<char>(this.Data[botY].GetRange(botX + 1, this.Data[botY].Count - botX - 1)));
                    this.Data.RemoveRange(topY, botY - topY + 1);

                    if (currentLine.Count > 0)
                    {
                        this.Data.Insert(topY, currentLine);

                        if (currentLine.Last() != '¬')
                        {
                            if (topY + 1 <= this.Data.Count - 1)
                            {
                                currentLine.AddRange(new List<char>(this.Data[topY + 1]));
                                this.Data.RemoveAt(topY + 1);
                            }
                            else
                            {
                                this.Data[topY].Add('¬');
                            }
                        }
                    }
                    //this.Data[topY].AddRange(this.Data[topY + 1]);
                }
                if (topY == botY)
                {
                    int firstX = topX < botX ? topX : botX;
                    int lastX = topX < botX ? botX : topX;

                        this.Data[topY].RemoveRange(firstX, lastX - firstX + 1);
                        if (this.Data[topY].Count > 0)
                        {
                        if (this.Data[topY].Last() != '¬')
                        {
                            if (topY + 1 <= this.Data.Count - 1)
                            {
                                this.Data[topY].AddRange(this.Data[topY + 1]);
                                this.Data.RemoveAt(topY + 1);
                            }
                            else
                            {
                                this.Data[topY].Add('¬');
                            }
                        }
                        }
                        else
                    {
                        this.Data.RemoveAt(topY);
                    }
                }
                if (!keepHighlight)
                {
                    this.HighlightStartX = 0;
                    this.HighlightStartY = 0;
                    this.HighlightEndX = 0;
                    this.HighlightEndY = 0;
                    this.ShowHighlight = false;
                    this.HighlightOn = false;
                }
                this.CheckIfDataEmpty();
                this.Y_CheckIfOutsideDataCount();
                this.Y_CheckIfOutsideTop();
                this.X_CheckIfOutsideDataCount();
                this.X_CheckIfOutsideLeft();
            }
            
        }
        public void MoveHighlightPosInData(int lengthOffset)
        {
               
                int finalLength = this.HighlightLengthFromOrigin + lengthOffset;
                this.HighlightStartX = this.GetNewCordsOffset(0, 0, finalLength, this.Data).X;
                this.HighlightStartY = this.GetNewCordsOffset(0, 0, finalLength, this.Data).Y;
                this.SetNewHighlight(this.HighlightStartX, this.HighlightStartY, this.HighlightLength, this.Data);
            this.UpdateHighlightPos();
            
        }
        public void MoveText(int toX,int toY)
        {
            this.CurrFile.FileDataChanged = true;
            List<List<char>> dataToMove = new List<List<char>>(this.GetCopyData(this.HighlightStartX,this.HighlightStartY,this.HighlightEndX,this.HighlightEndY));
            /*JE NUTNO Uložit zkopírované do proměnné a poté vymazat,pak zkopírovat z proměnné*/
            //this.SetNewHighlight(toX, toY, this.length, this.Data);
            //this.SetNewHighlight(toX, toY, copyLength, this.Data);


            this.DeleteHighlight(this.HighlightStartX, this.HighlightStartY, this.HighlightEndX, this.HighlightEndY, true);
            int insertY = toY;
            if (toY>this.Data.Count-1)
            {
                insertY = toY-dataToMove.Count;
            }
            this.InsertDataIntoNewLocation(toX, insertY, dataToMove);
            this.SetNewHighlight(toX, insertY, this.HighlightLength, this.Data);
            this.UpdateHighlightPos();
            this.SetCursorToHighlight();
            //if (f_HighlightStartY < this.HighlightStartY)
            //{
            //    this.MoveHighlightPosBackInData(copyLength);
            //}

        }
        public List<List<char>> GetCopyData(int startX,int startY,int endX,int endY)
        {
            List<List<char>> highlightedData = new List<List<char>>();
            int topY = startY < endY ? startY: endY;
            int botY = startY < endY ? endY : startY;
            int topX = startY < endY ? startX : endX;
            int botX = startY < endY ? endX : startX;
            if (topY<botY)
            {
                for (int i = topY; i <= botY; i++)
                {
                    if (i == topY)
                    {
                        List<char> line = new List<char>(this.Data[i].GetRange(topX, this.Data[i].Count - topX));
                        highlightedData.Add(line);
                    }
                    else if (i == botY)
                    {
                        List<char> line = new List<char>(this.Data[i].GetRange(0, botX + 1));
                        highlightedData.Add(line);
                    }
                    else
                    {
                        highlightedData.Add(new List<char>(this.Data[i]));
                    }
                }
            }
            if (topY == botY)
            {
                int firstX = topX < botX ? topX : botX;
                int lastX = topX < botX ? botX : topX;
                highlightedData.Add(this.Data[topY].GetRange(firstX, lastX - firstX + 1));
            }
            return highlightedData;
        }
        public void InsertDataIntoNewLocation(int newX,int newY,List<List<char>> dataToInsert)
        {
            List<List<char>> dataBlock = new List<List<char>>(dataToInsert);
            List<char> currentLine = new List<char>(this.Data[newY]);
            if (dataBlock.Count > 1)
            {
                dataBlock[0].InsertRange(0, currentLine.GetRange(0, newX));
                if (dataBlock.Last().Last() == '¬')
                {
                    dataBlock.Add(new List<char>());
                }
                dataBlock.Last().AddRange(currentLine.GetRange(newX, currentLine.Count - newX));
                this.Data.RemoveAt(newY);
                    this.Data.InsertRange(newY, dataBlock);
            }
            else
            {
                List<char> copyline = new List<char>(dataBlock.First());
                if (copyline.Last() == '¬')
                {
                    this.Data.Insert(newY + 1, currentLine.GetRange(newX, currentLine.Count - newX));
                    this.Data[newY].RemoveRange(newX, currentLine.Count - newX);
                }
                    this.Data[newY].InsertRange(newX, copyline);
                }
        }
        public void CopyHighlightToNewLocation(int newX, int newY)
        {
            if (!HighlightOn && ShowHighlight)
            {
                this.CurrFile.FileDataChanged = true;

                //List<char> currentLine = new List<char>(this.Data[newY]);
                //int topY = this.HighlightStartY < this.HighlightEndY ? this.HighlightStartY : this.HighlightEndY;
                //int botY = this.HighlightStartY < this.HighlightEndY ? this.HighlightEndY : this.HighlightStartY;
                //int topX = this.HighlightStartY < this.HighlightEndY ? this.HighlightStartX : this.HighlightEndX;
                //int botX = this.HighlightStartY < this.HighlightEndY ? this.HighlightEndX : this.HighlightStartX;

                //if (topY < botY)
                //{
                List<List<char>> copyData = new List<List<char>>(this.GetCopyData(this.HighlightStartX,this.HighlightStartY,this.HighlightEndX,this.HighlightEndY));
                //List<List<char>> copyData = new List<List<char>>();
                //for (int i = topY; i <= botY; i++)
                //{
                //    if (i == topY)
                //    {
                //        List<char> line = new List<char>(this.Data[i].GetRange(topX, this.Data[i].Count - topX));
                //        copyData.Add(line);
                //    }
                //    else if (i == botY)
                //    {
                //        List<char> line = new List<char>(this.Data[i].GetRange(0, botX + 1));
                //        copyData.Add(line);
                //    }
                //    else
                //    {
                //        copyData.Add(new List<char>(this.Data[i]));
                //    }
                //}
                //this.length = this.GetLengthBetweenTwoPoints(0, 0, copyData.Last().Count - 1, copyData.Count - 1, copyData);
                //copyData[0].InsertRange(0, currentLine.GetRange(0, newX));
                //if (copyData[copyData.Count - 1].Last() == '¬')
                //{
                //    copyData.Add(new List<char>());
                //}
                //if (newY == topY)
                //{
                //    this.HighlightStartX += copyData.Last().Count - newX;
                //}
                //copyData[copyData.Count - 1].AddRange(currentLine.GetRange(newX, currentLine.Count - newX));
                //this.Data.RemoveAt(newY);
                //this.Data.InsertRange(newY, copyData);
                //if (newY <= topY)
                //{
                //    this.HighlightStartY += copyData.Count - 1;
                //    this.HighlightEndY += copyData.Count - 1;

                //}
                //}
                //if (topY == botY)
                //{
                //int firstX = topX < botX ? topX : botX;
                //int lastX = topX < botX ? botX : topX;
                //List<char> copyline = new List<char>(this.Data[topY].GetRange(firstX, lastX - firstX + 1));
                //this.length = copyline.Count;
                //if (copyline.Last() == '¬')
                //{
                //    if (newY == topY)
                //    {
                //        this.HighlightStartX -= newX;
                //        this.HighlightEndX -= newX;

                //    }
                //    this.Data.Insert(newY + 1, currentLine.GetRange(newX, currentLine.Count - newX));
                //    this.Data[newY].RemoveRange(newX, currentLine.Count - newX);
                //    this.Data[newY].InsertRange(newX, copyline);
                //    if (newY <= topY)
                //    {
                //        this.HighlightStartY += 1;
                //        this.HighlightEndY += 1;
                //    }
                //}
                //else
                //{
                //    if (newY == topY)
                //    {
                //        this.HighlightStartX += copyline.Count - newX;
                //        this.HighlightEndX += copyline.Count - newX;

                //    }
                //    this.Data[newY].InsertRange(newX, copyline);
                //}
                
                this.InsertDataIntoNewLocation(newX,newY,copyData);
                if (newY < this.HighlightStartY)
                {
                    this.MoveHighlightPosInData(this.HighlightLength-1);
                }

                //}
                //TODO - FileChanged
            }
        }

        private (int X,int Y) GetNewCordsOffset(int fromX,int fromY,int length,List<List<char>> textBlock)
        {
            int finalX = fromX;
            int finalY = fromY;
            for (int i = 0; i < length; i++)
            {
                    if (finalX == textBlock[finalY].Count - 1)
                    {
                        finalY++;
                        finalX = 0;
                    }
                    else
                    {
                    if (i < length)
                    {
                        finalX++;
                    }
                    }
                if (finalY == textBlock.Count - 1 && finalX == textBlock[finalY].Count - 1)
                {
                    break;
                }
            }
            return (finalX, finalY);
        }
        private int GetLengthBetweenTwoPoints(int startX,int startY,int endX,int endY, List<List<char>> textBlock)
        {

                int topY = startY < endY ? startY : endY;
                int botY = startY < endY ? endY : startY;
                int topX = startY < endY ? startX : endX;
                int botX = startY < endY ? endX : startX;
                int length = 0;
            if (topY < botY)
            {
                for (int i = topY; i <= botY; i++)
                {
                    if (i == topY)
                    {
                        length += textBlock[i].Count - topX;
                    }
                    else if (i == botY)
                    {
                        length += botX+1;
                    }
                    else
                    {
                        length += textBlock[i].Count;
                    }
                }
            }
            if (topY==botY)
            {
                int firstX = topX < botX ? topX : botX;
                int lastX = topX < botX ? botX : topX;
                length = lastX - firstX;
            }
                return length;
        }
        private void SetNewHighlight(int fromX, int fromY,int length, List<List<char>> textBlock)
        {
            this.HighlightStartX = fromX;
            this.HighlightStartY = fromY;
            this.HighlightEndX = this.GetNewCordsOffset(fromX,fromY,length-1,textBlock).X;
            this.HighlightEndY = this.GetNewCordsOffset(fromX, fromY, length-1,textBlock).Y;
        }
        public void SetHighlightToCursor()
        {
            this.HighlightStartY = this.SelectedY;
            this.HighlightStartX = this.SelectedX;
            this.HighlightEndY = this.SelectedY;
            this.HighlightEndX = this.SelectedX;
        }
        public void SetCursorToSpecific(int toX,int toY)
        {
            this.SelectedX = toX;
            this.SelectedY = toY;
            this.X_CheckIfOutsideLeft();
            this.Y_CheckIfOutsideTop();
        }
        public void SetCursorToHighlight()
        {
            this.SetCursorToSpecific(this.HighlightStartX, this.HighlightStartY);
        }

        public void FindAndReplace(string srctext,string toText)
        {
            if (FindingInit)
            {
                this.CurrFile.FileDataChanged = true;
                this.SetFindToCursor();
                this.FindingInit = false;
            }
            this.CalculateNextOccurence(srctext);
            if (this.OccurenceAvailable)
            {
                this.Data[HighlightStartY].RemoveRange(this.HighlightStartX, this.HighlightEndX - this.HighlightStartX + 1);
                this.Data[HighlightStartY].InsertRange(this.HighlightStartX, toText.ToCharArray().ToList());
                this.SetNewHighlight(this.HighlightStartX, this.HighlightStartY, toText.Length, this.Data);
                this.ShowHighlight = true;

                this.SetCursorToHighlight();

            }
            else
            {
                this.FindingInit = true;
            }
        }
        public void ReplaceFromCursor(string srctext, string toText)
        {
            this.CurrFile.FileDataChanged = true;

            this.SetFindToCursor();
            for (int i = this.SelectedY; i <= this.Data.Count-1; i++)
            {
                if (new string(ToArrayV2(this.Data[i], this.Data[i].Count)).Contains(srctext))
                {
                    string temp = new string(ToArrayV2(this.Data[i], this.Data[i].Count));
                    string newLine = "";
                    if (i == this.SelectedY)
                    {
                        string subsFromX = temp.Substring(this.SelectedX, this.Data[i].Count - this.SelectedX).Replace(srctext,toText);
                        newLine = temp.Substring(0, this.SelectedX) + subsFromX;
                    }
                    else
                    {
                        newLine = temp.Replace(srctext, toText);
                    }
                    this.Data[i] = newLine.ToCharArray().ToList();
                }
            }
           
        }
        public void SkipFind(string text)
        {
            if (FindingInit)
            {
                this.SetFindToCursor();
                this.FindingInit = false;
            }
            this.ShowHighlight = true;

            this.CalculateNextOccurence(text);

            if (this.OccurenceAvailable)
            {
                this.SetCursorToHighlight();
            }
            else
            {
                this.FindingInit = true;
            }
        }
        /*public void FindTextInLine(string text)
        {
            
                while (!(new string(this.Data[this.HighlightEndY].ToArray()).Contains(text)))
                {
                    if (HighlightEndY < this.Data.Count - 1 || HighlightStartY < this.Data.Count - 1)
                    {
                        this.HighlightStartX = 0;
                    this.HighlightStartY++;
                    this.HighlightEndY++;
                        this.HighlightEndX = 0;
                    }
                    else
                    {
                        this.HighlightStartX = 0;
                        this.HighlightStartY = 0;
                        this.HighlightEndY = 0;
                        this.HighlightEndX = 0;
                        FindingMode = false;
                        ShowHighlight = false;
                        HighlightOn = false;
                        break;

                    }
                }

                if ((new string(this.Data[this.HighlightEndY].ToArray()).Contains(text)))
                {
                    while (new string(this.Data[this.HighlightEndY].ToArray()).IndexOf(text, this.HighlightEndX == 0 ? this.HighlightEndX : this.HighlightEndX + 1) == -1)
                    {
                        if (HighlightEndY < this.Data.Count - 1 || HighlightStartY < this.Data.Count - 1)
                        {
                            this.HighlightStartX = 0;
                        this.HighlightStartY++;
                        this.HighlightEndY++;
                            this.HighlightEndX = 0;
                        }
                        else
                        {
                            this.HighlightStartX = 0;
                            this.HighlightStartY = 0;
                            this.HighlightEndY = 0;
                            this.HighlightEndX = 0;
                            FindingMode = false;
                            ShowHighlight = false;
                            HighlightOn = false;

                            break;
                        }
                    }
                    
                    if (new string(this.Data[this.HighlightEndY].ToArray()).IndexOf(text, this.HighlightEndX == 0 ? this.HighlightEndX : this.HighlightEndX + 1) > -1)
                    {
                        this.HighlightStartX = new string(this.Data[this.HighlightEndY].ToArray()).IndexOf(text, this.HighlightEndX == 0 ? this.HighlightEndX : this.HighlightEndX + 1);
                        this.SetNewHighlight(new string(this.Data[this.HighlightEndY].ToArray()).IndexOf(text, this.HighlightEndX == 0 ? this.HighlightEndX : this.HighlightEndX + 1), this.HighlightStartY, text.Length, this.Data);
                        this.ShowHighlight = true;
                        HighlightOn = false;
                    this.SetCursorToHighlight();
                    }
                

            }

        }*/

        public int FindIndexX = 0;
        public int FindIndexY = 0;
        public bool OccurenceAvailable = false;
       
        public void FindTextOnly(string text)
        {
            this.SetFindToCursor();
            this.CalculateNextOccurence(text);
            if (this.OccurenceAvailable)
            {
                this.SetCursorToHighlight();
            }
        }
        private void SetFindToCursor()
        {
            this.FindIndexX = this.SelectedX;
            this.FindIndexY = this.SelectedY;
        }
        public void CalculateNextOccurence(string text)
        {
            this.OccurenceAvailable = false;
            int foundX = new string(ToArrayV2(this.Data[FindIndexY], this.Data[FindIndexY].Count)).IndexOf(text,this.FindIndexX);
            if (foundX >-1)
            {
                this.SetNewHighlight(foundX, this.FindIndexY, text.Length, this.Data);
                if (foundX < this.Data[FindIndexY].Count-1)
                {
                    this.FindIndexX = foundX + 1;
                }
                else
                {
                    if (this.FindIndexY < this.Data.Count - 1)
                    {
                        this.FindIndexY = FindIndexY + 1;
                        this.FindIndexX = 0;
                    }
                }
                this.OccurenceAvailable = true;
            }
            else
            {
                for (int i = this.FindIndexY+1; i <= this.Data.Count-1; i++)
                {
                    int appearsAtX = new string(ToArrayV2(this.Data[i], this.Data[i].Count)).IndexOf(text, 0);
                    if (appearsAtX >-1)
                    {
                        this.SetNewHighlight(appearsAtX, i, text.Length, this.Data);
                        if (appearsAtX < this.Data[i].Count - 1)
                        {
                            this.FindIndexX = appearsAtX + 1;
                            this.FindIndexY = i;
                        }
                        else
                        {
                            if (i < this.Data.Count - 1)
                            {
                                this.FindIndexY = i + 1;
                                this.FindIndexX = 0;
                            }
                        }
                        this.OccurenceAvailable = true;
                        break;
                    }
                }
            }
        } 
        public char[] ToArrayV2(List<char> list,int count)
        {
            char[] charArray = new char[count];
            int j = 0;
            foreach (char i in list)
            {
                charArray[j++] = i;
            }
            return charArray;
        }
    }
    }

    

