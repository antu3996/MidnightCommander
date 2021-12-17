using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class FileEditWindow : Window,IFunctionalWindow
    {
        public MainPanels Panels { get; set; }
        private FileUtils fileservice;
        private TextEditor2 editor;

        public FileEditWindow(int x, int y, int w, int h, ConsoleColor fore_Col, ConsoleColor back_Col,MainPanels panels) : base(x, y, w, h, fore_Col, back_Col)
        {
            this.Panels = panels;
            this.fileservice = this.Panels.Get_Selected_Sys.GetFileUtilsOfSelected();

            TextEditor2 text_editor = new TextEditor2(true,this.X,this.Y,this.Width,this.Height-1,this.fileservice,ConsoleColor.Black,ConsoleColor.Yellow,
                ConsoleColor.White,ConsoleColor.DarkBlue,ConsoleColor.White,ConsoleColor.Blue,ConsoleColor.Black,ConsoleColor.Cyan);
            this.editor = text_editor;
            int btn_Width = (this.Width / 9) - 1;
            int btn_Y = this.Height - 1;
            Button save = new Button(false, this.X, btn_Y, btn_Width, 1, "2 Save", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button highlightSwitch = new Button(false,  (btn_Width + 1), btn_Y, btn_Width, 1, "3 High", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button replace = new Button(false, 2 * (btn_Width + 1), btn_Y, btn_Width, 1, "4 Repl", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button copy = new Button(false, 3 * (btn_Width + 1), btn_Y, btn_Width, 1, "5 Copy", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button move = new Button(false, 4 * (btn_Width + 1), btn_Y, btn_Width, 1, "6 Move", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button find = new Button(false, 5 * (btn_Width + 1), btn_Y, btn_Width, 1, "7 Find", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button delete = new Button(false, 6 * (btn_Width + 1), btn_Y, btn_Width, 1, "8 Del", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button quit = new Button(false, 7*(btn_Width + 1), btn_Y, btn_Width, 1, "10 Quit", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);



            replace.Click += Replace;
            replace.FuncKey = ConsoleKey.F4;

            move.Click += Move;
            move.FuncKey = ConsoleKey.F6;

            copy.Click += Copy;
            copy.FuncKey = ConsoleKey.F5;

            delete.Click += Delete;
            delete.FuncKey = ConsoleKey.F8;

            highlightSwitch.Click += Highlight;
            highlightSwitch.FuncKey = ConsoleKey.F3;

            save.Click += SaveFile;
            save.FuncKey = ConsoleKey.F2;

            quit.Click += CloseWindow;
            quit.FuncKey = ConsoleKey.F10;

            find.Click += OpenFindWindow;
            find.FuncKey = ConsoleKey.F7;

            this.modules.AddRange(new IModule[] { text_editor,save, quit,highlightSwitch,replace,copy,move,find,delete });
        }
        public override void Draw()
        {
            if (RedrawAll)
            {
                this.Drawer.Clear();
            }
            base.Draw();
        }
        public void SaveFile()
        {
            this.fileservice.OverwriteTextFile(false);
        }

        public void CloseWindow()
        {
            this.SaveFile();

            this.Close = true;
        }
        public void OpenFindWindow()
        {
            Application.OpenWindow(new FindTextWindow(10, 8, this.Width - 46, this.Height - 20, ConsoleColor.Black, ConsoleColor.White, editor));
        }
        public void Replace()
        {
            Application.OpenWindow(new TextReplaceWindow(10, 8, this.Width - 46, this.Height - 20, ConsoleColor.Black, ConsoleColor.White, editor,this));
        }
        public void Move()
        {
            this.editor.MoveText(this.editor.SelectedX,this.editor.SelectedY);
        }
        public void Delete()
        {
            this.editor.DeleteHighlight(this.editor.HighlightStartX,this.editor.HighlightStartY,this.editor.HighlightEndX,this.editor.HighlightEndY,false);
        }
        public void Highlight()
        {
            this.editor.TurnOnHighlight();
        }
        public void Copy()
        {
            this.editor.CopyHighlightToNewLocation(this.editor.SelectedX,this.editor.SelectedY);
        }
    }
}
