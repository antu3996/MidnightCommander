using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class TextEditor2 : IGraphicModule
    {
        //public bool UpdateVisual { get; set; } = false;
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
        public List<string> Data { get; set; }

        public int HighlightStartX = 0;
        public int HighlightStartY = 0;
        public int HighlightEndX = 0;
        public int HighlightEndY = 0;
        private bool HighlightOn = false;
        private bool ShowHighlight = false;
        public bool FindingInit = true;
        private int HighlightLengthFromOrigin = 0;
        private int HighlightLength = 0;



        public TextEditor2(bool isSelected, int x, int y, int w, int h,
            FileUtils service, ConsoleColor highlight_fore, ConsoleColor highlight_back,
            ConsoleColor normal_fore, ConsoleColor normal_back, ConsoleColor header_fore, ConsoleColor header_back, ConsoleColor selected_Fore, ConsoleColor selected_Back)
        {
            this.CurrFile = service;
            this.Data = new List<string>(service.GetContentOfFile());
            if (this.Data.Count < 1)
            {
                this.Data.Add("¬");
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
            string hex = Convert.ToUInt32(this.Data[this.SelectedY][this.SelectedX]).ToString("X");
            int ascii = ((int)this.Data[this.SelectedY][this.SelectedX]);
            string save_text = this.CurrFile.FileDataChanged == false ? "SAVED" : "UNSAVED";
            this.Header_Drawer.ResetToOrigin();

            this.Header_Drawer.WriteLine($"{fileName}{"".PadRight(10, ' ')}{columnIndex} L:[ " +
                $"{linesOutsideArea}+ {relativeLinePos}   {absoluteLinePos}/{totalLines}] *" +
                $"({absoluteCharIndex}/{totalChars}) " + $"{ascii}".PadLeft(4, '0') + $" 0x{hex}" + $"                 {save_text}".PadRight(this.Header_Drawer.MaxWidthWrite, ' '));
        }
        public void DrawData()
        {
            this.Drawer.ResetToOrigin();
            for (int i = this.Top; i < this.Top + this.Drawer.MaxHeightWrite; i++)
            {
                int currDrawX;
                int currDrawY;
                if (i < this.Data.Count && this.Data[i].Length > 0 && this.Data[i].Length > this.Left)
                {
                    string temp = this.Data[i].Substring(this.Left, this.Data[i].Length - this.Left);
                    //this.Drawer.WriteLine(new string(this.Data[i].ToArray(),this.Left,this.Data[i].Count-this.Left));
                    this.Drawer.WriteLine(temp);
                    currDrawX = this.Drawer.CursorX;
                    currDrawY = this.Drawer.CursorY;
                    //Divné ale rychlé vykreslování
                    if (!this.HighlightOn)
                    {
                        if (i == this.SelectedY)
                        {
                            for (int j = 0; j < temp.Length; j++)
                            {

                                if (j == this.SelectedX - this.Left)
                                {
                                    this.Drawer.CursorX = this.SelectedX - this.Left + this.Drawer.InitX;
                                    this.Drawer.CursorY = this.SelectedY - this.Top + this.Drawer.InitY;
                                    this.Drawer.BackColor = this.Selected_Back;
                                    this.Drawer.ForeColor = this.Selected_Fore;
                                    this.Drawer.Write(temp[j].ToString());
                                }
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
                    this.Drawer.WriteLine("".PadRight(this.Drawer.MaxWidthWrite, ' '));
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
            if (this.SelectedX < this.Data[this.SelectedY].Length - 1)
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
            if (this.Data[this.SelectedY].Length - 1 < this.SelectedX + this.Left)
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
            this.SelectedX = this.Data[lineAbsoluteIndex].Length - 1;
        }
        public void CheckIfDataEmpty()
        {
            if (this.Data.Count == 0 || this.Data[0].Length == 0)
            {
                this.Data.Add("¬");
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
                    if(this.ShowHighlight)
                    {
                        this.ResetHighlightAndFind();
                    }
                    if (this.SelectedX == 0)
                    {
                        this.Data.Insert(this.SelectedY, "¬");
                        this.Y_Down();
                    }
                    else
                    {
                        if (this.SelectedX == this.Data[this.SelectedY].Length - 1)
                        {
                            this.Data.Insert(this.SelectedY + 1, "¬");
                            this.Y_Down();
                            this.SelectedX = 0;

                            this.X_CheckIfOutsideLeft();
                        }
                        else
                        {
                            if (this.SelectedX > 0 && this.SelectedX < this.Data[this.SelectedY].Length - 1)
                            {
                                string temp = this.Data[this.SelectedY].Substring(this.SelectedX, this.Data[this.SelectedY].Length - this.SelectedX);
                                this.Data.Insert(this.SelectedY + 1, temp);
                                this.Data[this.SelectedY] = this.Data[this.SelectedY].Remove(this.SelectedX, this.Data[this.SelectedY].Length - this.SelectedX);
                                this.Data[this.SelectedY] += '¬';

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

                    if (this.ShowHighlight)
                    {
                        this.ResetHighlightAndFind();
                    }


                    if (this.Data[this.SelectedY].Length > 1)
                    {

                        if (this.SelectedY > 0 && this.SelectedX == 0)
                        {
                            this.SetXToTheEndOfLine(this.SelectedY - 1);
                            this.Data[this.SelectedY - 1] = this.Data[this.SelectedY - 1].Remove(this.Data[this.SelectedY - 1].Length - 1, 1);
                            this.Data[this.SelectedY - 1]+=this.Data[this.SelectedY];
                            this.Data.RemoveAt(this.SelectedY);
                            this.Y_Up();
                            this.X_CheckIfOutsideDataCount();

                            this.X_CheckIfOutsideLeft();


                        }
                        else
                        {
                            if (this.SelectedX > 0)
                            {
                                this.Data[this.SelectedY] = this.Data[this.SelectedY].Remove(this.SelectedX - 1, 1);

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
                    this.CurrFile.tempData = this.Data;
                }
                else
                {
                    if (!char.IsControl(key.KeyChar))
                    {
                        if (this.ShowHighlight)
                        {
                            this.ResetHighlightAndFind();
                        }
                        this.Data[this.SelectedY] = this.Data[this.SelectedY].Insert(this.SelectedX, key.KeyChar.ToString());
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
        public void ResetHighlightAndFind()
        {
            this.ShowHighlight = false;
            this.HighlightStartX = 0;
            this.HighlightStartY = 0;
            this.HighlightEndX = 0;
            this.HighlightEndY = 0;
            this.HighlightOn = false;
            this.ResetFind();
        }
        public void ResetFind()
        {
            this.FindIndexX = 0;
            this.FindIndexY = 0;
            this.OccurenceAvailable = false;
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
                currentPos = currentPos + this.Data[i].Length;
            }
            currentPos = currentPos + this.SelectedX + 1;
            return currentPos;
        }
        private int GetTotalCharsCount()
        {
            int totalcount = 0;
            foreach (string item in this.Data)
            {
                totalcount = totalcount + item.Length;
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
                int actualStartX = this.Left > startX ? this.Left : startX+1;
                this.Drawer.CursorX = 0 + this.Drawer.InitX;
                this.Drawer.CursorY = startY - this.Top + this.Drawer.InitY;
                this.Drawer.BackColor = this.Highlight_Back;
                this.Drawer.ForeColor = this.HighLight_Fore;
                this.Drawer.Write(line.Substring(0, actualStartX - this.Left));
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
                int actualEndX = endX < this.Left ? this.Left : endX+1;
                this.Drawer.CursorX = actualStartX - this.Left + this.Drawer.InitX;
                this.Drawer.CursorY = Y - this.Top + this.Drawer.InitY;
                this.Drawer.BackColor = this.Highlight_Back;
                this.Drawer.ForeColor = this.HighLight_Fore;
                this.Drawer.Write(line.Substring(actualStartX - this.Left, actualEndX - actualStartX));
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

        public void DeleteHighlight(int startX, int startY, int endX, int endY, bool keepHighlight)
        {
            if (!HighlightOn && ShowHighlight)
            {
                this.CurrFile.FileDataChanged = true;


                string currentLine = "";
                int topY = startY < endY ? startY : endY;
                int botY = startY < endY ? endY : startY;
                int topX = startY < endY ? startX : endX;
                int botX = startY < endY ? endX : startX;
                if (topY < botY)
                {
                    currentLine+=this.Data[topY].Substring(0, topX);
                    currentLine+=this.Data[botY].Substring(botX + 1, this.Data[botY].Length - botX - 1);
                    this.Data.RemoveRange(topY, botY - topY + 1);

                    if (currentLine.Length > 0)
                    {
                        this.Data.Insert(topY, currentLine);

                        if (currentLine.Last() != '¬')
                        {
                            if (topY + 1 <= this.Data.Count - 1)
                            {
                                this.Data[topY]+= this.Data[topY + 1];
                                this.Data.RemoveAt(topY + 1);
                            }
                            else
                            {
                                this.Data[topY]+='¬';
                            }
                        }
                    }
                    //this.Data[topY].AddRange(this.Data[topY + 1]);
                }
                if (topY == botY)
                {
                    int firstX = topX < botX ? topX : botX;
                    int lastX = topX < botX ? botX : topX;

                    this.Data[topY] = this.Data[topY].Remove(firstX, lastX - firstX + 1);
                    if (this.Data[topY].Length > 0)
                    {
                        if (this.Data[topY].Last() != '¬')
                        {
                            if (topY + 1 <= this.Data.Count - 1)
                            {
                                this.Data[topY]+=this.Data[topY + 1];
                                this.Data.RemoveAt(topY + 1);
                            }
                            else
                            {
                                this.Data[topY]+='¬';
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
                    this.ResetHighlightAndFind();
                }
                this.CheckIfDataEmpty();
                this.Y_CheckIfOutsideDataCount();
                this.Y_CheckIfOutsideTop();
                this.X_CheckIfOutsideDataCount();
                this.X_CheckIfOutsideLeft();
            }

        }
        private void MoveHighlightPosInData(int lengthOffset)
        {

            int finalLength = this.HighlightLengthFromOrigin + lengthOffset;
            this.HighlightStartX = this.GetNewCordsOffset(0, 0, finalLength, this.Data).X;
            this.HighlightStartY = this.GetNewCordsOffset(0, 0, finalLength, this.Data).Y;
            this.SetNewHighlight(this.HighlightStartX, this.HighlightStartY, this.HighlightLength, this.Data);

        }
        public void MoveText(int toX, int toY)
        {
            if (!HighlightOn && ShowHighlight && !CursorIsInsideHighlight())
            {
                this.CurrFile.FileDataChanged = true;
                List<string> dataToMove = new List<string>(this.GetCopyData(this.HighlightStartX, this.HighlightStartY, this.HighlightEndX, this.HighlightEndY));
                /*JE NUTNO Uložit zkopírované do proměnné a poté vymazat,pak zkopírovat z proměnné*/
                int insertX = toX;
                int insertY = toY;
                int beforeDel = this.GetLengthBetweenTwoPoints(0, 0, toX, toY, this.Data);
                this.DeleteHighlight(this.HighlightStartX, this.HighlightStartY, this.HighlightEndX, this.HighlightEndY, true);
                if (this.HighlightEndY < toY || (toX >= this.HighlightEndX && toY == this.HighlightEndY))
                {
                    beforeDel = beforeDel - this.GetLengthBetweenTwoPoints(0, 0, dataToMove.Last().Length - 1, dataToMove.Count - 1, dataToMove)-1;
                    insertX = this.GetNewCordsOffset(0, 0, beforeDel, this.Data).X;
                    insertY = this.GetNewCordsOffset(0, 0, beforeDel, this.Data).Y;
                }

                //if (toY >= this.HighlightEndY)
                //{
                //        insertY = insertY - dataToMove.Count;

                //} 
                this.InsertDataIntoNewLocation(insertX, insertY, dataToMove);
                this.SetNewHighlight(insertX, insertY, this.HighlightLength, this.Data);
                //this.SetCursorToHighlight();
                this.X_CheckIfOutsideDataCount();
                this.X_CheckIfOutsideLeft();
                this.Y_CheckIfOutsideDataCount();
                this.Y_CheckIfOutsideTop();
            }
        }
        private List<string> GetCopyData(int startX, int startY, int endX, int endY)
        {
            List<string> highlightedData = new List<string>();
            int topY = startY < endY ? startY : endY;
            int botY = startY < endY ? endY : startY;
            int topX = startY < endY ? startX : endX;
            int botX = startY < endY ? endX : startX;
            if (topY < botY)
            {
                for (int i = topY; i <= botY; i++)
                {
                    if (i == topY)
                    {
                       string line = this.Data[i].Substring(topX, this.Data[i].Length - topX);
                        highlightedData.Add(line);
                    }
                    else if (i == botY)
                    {
                        string line = this.Data[i].Substring(0, botX + 1);
                        highlightedData.Add(line);
                    }
                    else
                    {
                        highlightedData.Add(this.Data[i]);
                    }
                }
            }
            if (topY == botY)
            {
                int firstX = topX < botX ? topX : botX;
                int lastX = topX < botX ? botX : topX;
                highlightedData.Add(this.Data[topY].Substring(firstX, lastX - firstX + 1));
            }
            return highlightedData;
        }
        private void InsertDataIntoNewLocation(int newX, int newY, List<string> dataToInsert)
        {
            List<string> dataBlock = new List<string>(dataToInsert);
            string currentLine = this.Data[newY];
            if (dataBlock.Count > 1)
            {
                dataBlock[0] = dataBlock[0].Insert(0, currentLine.Substring(0, newX));
                if (dataBlock.Last().Last() == '¬')
                {
                    dataBlock.Add("");
                }
                dataBlock[dataBlock.Count-1]+=currentLine.Substring(newX, currentLine.Length - newX);
                this.Data.RemoveAt(newY);
                this.Data.InsertRange(newY, dataBlock);
            }
            else
            {
                string copyline = dataBlock.First();
                if (copyline.Last() == '¬')
                {
                    this.Data.Insert(newY + 1, currentLine.Substring(newX, currentLine.Length - newX));
                    this.Data[newY]= this.Data[newY].Remove(newX, currentLine.Length - newX);
                }
                this.Data[newY]=this.Data[newY].Insert(newX, copyline);
            }
        }
        public void CopyHighlightedToNewLocation(int newX, int newY)
        {
            if (!HighlightOn && ShowHighlight && !CursorIsInsideHighlight())
            {
                this.CurrFile.FileDataChanged = true;
                List<string> copyData = new List<string>(this.GetCopyData(this.HighlightStartX, this.HighlightStartY, this.HighlightEndX, this.HighlightEndY));

                this.InsertDataIntoNewLocation(newX, newY, copyData);
                
                if (newY < this.HighlightStartY || (newY == this.HighlightStartY && newX <=this.HighlightStartX))
                {
                        this.MoveHighlightPosInData(this.HighlightLength - 1);
                }
            }
        }
        private bool CursorIsInsideHighlight()
        {
            if (this.HighlightStartY < this.HighlightEndY)
            {
                return (this.SelectedY > this.HighlightStartY && this.SelectedY < this.HighlightEndY) || (this.SelectedX >= this.HighlightStartX && this.SelectedY == this.HighlightStartY) || (this.SelectedX <= this.HighlightEndX && this.SelectedY == this.HighlightEndY);
            }
            if (this.HighlightStartY == this.HighlightEndY)
            {
                return (this.SelectedX >= this.HighlightStartX && this.SelectedX <= this.HighlightEndX && this.SelectedY==this.HighlightStartY);
            }
            return false;
        }
        private (int X, int Y) GetNewCordsOffset(int fromX, int fromY, int length, List<string> textBlock)
        {
            int finalX = fromX;
            int finalY = fromY;
            for (int i = 0; i < length; i++)
            {
                if (finalX == textBlock[finalY].Length - 1)
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
                if (finalY == textBlock.Count - 1 && finalX == textBlock[finalY].Length - 1)
                {
                    break;
                }
            }
            return (finalX, finalY);
        }
        private int GetLengthBetweenTwoPoints(int startX, int startY, int endX, int endY, List<string> textBlock)
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
                        length += textBlock[i].Length - topX;
                    }
                    else if (i == botY)
                    {
                        length += botX + 1;
                    }
                    else
                    {
                        length += textBlock[i].Length;
                    }
                }
            }
            if (topY == botY)
            {
                int firstX = topX < botX ? topX : botX;
                int lastX = topX < botX ? botX : topX;
                length = lastX - firstX+1;
            }
            return length;
        }
        private void SetNewHighlight(int fromX, int fromY, int length, List<string> textBlock)
        {
            this.HighlightStartX = fromX;
            this.HighlightStartY = fromY;
            this.HighlightEndX = this.GetNewCordsOffset(fromX, fromY, length-1, textBlock).X;
            this.HighlightEndY = this.GetNewCordsOffset(fromX, fromY, length-1, textBlock).Y;
            this.UpdateHighlightPos();
        }
        private void SetHighlightToCursor()
        {
            this.HighlightStartY = this.SelectedY;
            this.HighlightStartX = this.SelectedX;
            this.HighlightEndY = this.SelectedY;
            this.HighlightEndX = this.SelectedX;
        }
        private void SetCursorToSpecific(int toX, int toY)
        {
            this.SelectedX = toX;
            this.SelectedY = toY;
            this.X_CheckIfOutsideLeft();
            this.Y_CheckIfOutsideTop();
        }
        private void SetCursorToHighlight()
        {
            this.SetCursorToSpecific(this.HighlightStartX, this.HighlightStartY);
        }

        public void FindAndReplace(string srctext, string toText)
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
                this.Data[HighlightStartY] = this.Data[HighlightStartY].Remove(this.HighlightStartX, this.HighlightEndX - this.HighlightStartX + 1);
                this.Data[HighlightStartY] = this.Data[HighlightStartY].Insert(this.HighlightStartX, toText);
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
            for (int i = this.SelectedY; i <= this.Data.Count - 1; i++)
            {
                if (this.Data[i].Contains(srctext))
                {
                    string temp = this.Data[i];
                    string newLine = "";
                    if (i == this.SelectedY)
                    {
                        string subsFromX = temp.Substring(this.SelectedX, this.Data[i].Length - this.SelectedX).Replace(srctext, toText);
                        newLine = temp.Substring(0, this.SelectedX) + subsFromX;
                    }
                    else
                    {
                        newLine = temp.Replace(srctext, toText);
                    }
                    this.Data[i] = newLine;
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

            this.CalculateNextOccurence(text);

            if (this.OccurenceAvailable)
            {
                this.SetCursorToHighlight();
                this.ShowHighlight = true;
            }
            else
            {
                this.FindingInit = true;
            }
        }
       

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
                this.ShowHighlight = true;
            }
        }
        private void SetFindToCursor()
        {
            this.FindIndexX = this.SelectedX;
            this.FindIndexY = this.SelectedY;
        }
        private void CalculateNextOccurence(string text)
        {
            this.OccurenceAvailable = false;
            int foundX = this.Data[FindIndexY].IndexOf(text, this.FindIndexX);
            if (foundX > -1)
            {
                this.SetNewHighlight(foundX, this.FindIndexY, text.Length, this.Data);
                if (foundX < this.Data[FindIndexY].Length - 1)
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
                for (int i = this.FindIndexY + 1; i <= this.Data.Count - 1; i++)
                {
                    int appearsAtX = this.Data[i].IndexOf(text, 0);
                    if (appearsAtX > -1)
                    {
                        this.SetNewHighlight(appearsAtX, i, text.Length, this.Data);
                        if (appearsAtX < this.Data[i].Length - 1)
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
    }
}



