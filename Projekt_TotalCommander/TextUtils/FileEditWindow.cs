using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class FileEditWindow : Window
    {
        private FileUtils fileservice;
        private TextEditor2 editor;

        public FileEditWindow(int x, int y, int w, int h, ConsoleColor fore_Col, ConsoleColor back_Col,MainPanels panels) : base(x, y, w, h, fore_Col, back_Col)
        {
            this.fileservice = new FileUtils(panels);

            TextEditor2 text_editor = new TextEditor2(true,this.X,this.Y,this.Width,this.Height-1,this.fileservice,ConsoleColor.Black,ConsoleColor.Yellow,
                ConsoleColor.White,ConsoleColor.DarkBlue,ConsoleColor.White,ConsoleColor.Blue,ConsoleColor.Black,ConsoleColor.Cyan);
            this.editor = text_editor;
            int btn_Width = (this.Width / 9);
            int btn_Y = this.Height - 1;
            Button save = new Button(false, this.X, this.Y+btn_Y, btn_Width, 1, "2 Save", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button highlightSwitch = new Button(false,  this.X+(btn_Width), this.Y + btn_Y, btn_Width, 1, "3 High", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button replace = new Button(false, this.X + 2 * (btn_Width), this.Y + btn_Y, btn_Width, 1, "4 Repl", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button copy = new Button(false, this.X + 3 * (btn_Width), this.Y + btn_Y, btn_Width, 1, "5 Copy", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button move = new Button(false, this.X + 4 * (btn_Width), this.Y + btn_Y, btn_Width, 1, "6 Move", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button find = new Button(false, this.X + 5 * (btn_Width), this.Y + btn_Y, btn_Width, 1, "7 Find", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button delete = new Button(false, this.X + 6 * (btn_Width), this.Y + btn_Y, btn_Width, 1, "8 Del", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button quit = new Button(false, this.X + 7 *(btn_Width), this.Y + btn_Y, btn_Width, 1, "10 Quit", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);



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
        public override void Update()
        {
            if (this.Close)
            {
                Console.CursorVisible = false;
            }
            base.Update();
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
            Application.OpenWindow(new FindTextWindow(this.X+5, this.Y+5, ObjectSizes.DialogSize.W, ObjectSizes.DialogSize.H, ConsoleColor.Black, ConsoleColor.White, editor));
        }
        public void Replace()
        {
            Application.OpenWindow(new TextReplaceWindow(this.X+5, this.Y+5, ObjectSizes.DialogSize.W, ObjectSizes.DialogSize.H, ConsoleColor.Black, ConsoleColor.White, editor));
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
            this.editor.CopyHighlightedToNewLocation(this.editor.SelectedX,this.editor.SelectedY);
        }
    }
}
