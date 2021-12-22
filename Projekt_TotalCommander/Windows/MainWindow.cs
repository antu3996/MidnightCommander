using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public class MainWindow : Window, IFunctionalWindow
    {
        public MainPanels Panels { get; set; }


        public MainWindow(int x, int y, int w, int h, ConsoleColor fore_Col, ConsoleColor back_Col) : base(x, y, w, h, fore_Col, back_Col)
        {
            this.Panels = new MainPanels();


            int btn_1_W = (this.Width / 5);
            Button left_btn = new Button(false, this.X, this.Y, btn_1_W, 1, "Left",ConsoleColor.Black,ConsoleColor.Cyan,ConsoleColor.Green);
            Button file_btn = new Button(false, this.X + btn_1_W, this.Y, btn_1_W, 1, "File", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button command_btn = new Button(false, this.X + 2 * btn_1_W,this.Y,  btn_1_W, 1, "Command", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button options_btn = new Button(false, this.X + 3 * btn_1_W, this.Y, btn_1_W, 1, "Options", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button right_btn = new Button(false, this.X + 4 * btn_1_W, this.Y,  btn_1_W, 1, "Right", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);

            Container container1 = new Container(false, left_btn, file_btn, command_btn, options_btn, right_btn);
            int tbl_W = (this.Width / 2);


            FolderTable table1 = new FolderTable(false, this.X, this.X+1, tbl_W, this.Height - 2, this.Panels.LeftPanel, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.Black,ConsoleColor.White);
            FolderTable table2 = new FolderTable(false, this.X + tbl_W, this.X + 1, tbl_W, this.Height-2, this.Panels.RightPanel, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.Black, ConsoleColor.White);

            Container container2 = new Container(true, table1, table2);
            int btn_Width = (this.Width / 9)-1;
            int btn_Y = this.Height - 1;
            Button help = new Button(false, this.X,this.Y+ btn_Y, btn_Width, 1, "1 Help", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button menu = new Button(false, this.X + btn_Width + 1, this.Y + btn_Y, btn_Width, 1, "2 Menu", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button view = new Button(false, this.X + 2 * btn_Width + 2, this.Y + btn_Y, btn_Width, 1, "3 View", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button edit = new Button(false, this.X + 3 * btn_Width + 3, this.Y + btn_Y, btn_Width, 1, "4 Edit", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button copy = new Button(false, this.X + 4 * btn_Width + 4, this.Y + btn_Y, btn_Width, 1, "5 Copy", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button move = new Button(false, this.X + 5 * btn_Width + 5, this.Y + btn_Y, btn_Width, 1, "6 Move", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button mkdir = new Button(false, this.X + 6 * btn_Width + 6, this.Y + btn_Y, btn_Width, 1, "7 MkDir", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button delete = new Button(false, this.X + 7 * btn_Width + 7, this.Y + btn_Y, btn_Width, 1, "8 Delete", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button quit = new Button(false, this.X + 8 * btn_Width + 8, this.Y + btn_Y, btn_Width, 1, "9 Quit", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);

            edit.Click += EditButton;
            edit.FuncKey = ConsoleKey.F4;

            copy.Click += CopyButton;
            copy.FuncKey = ConsoleKey.F5;

            mkdir.Click += MkDirButton;
            mkdir.FuncKey = ConsoleKey.F7;

            move.Click += MoveButton;
            move.FuncKey = ConsoleKey.F6;

            delete.Click += DeleteButton;
            delete.FuncKey = ConsoleKey.F8;

            quit.Click += CloseAll;
            quit.FuncKey = ConsoleKey.F9;

            this.modules.AddRange(new IModule[] { container1, container2, help, menu, view, edit, copy, move, mkdir, delete, quit });
        }
        public void CopyButton()
        {
            Application.OpenWindow(new CopyDirWindow(this.X+10, this.Y+8, this.Width - 16, this.Height-10,ConsoleColor.Black,ConsoleColor.White,this.Panels));
        }
        public void MkDirButton()
        {
            Application.OpenWindow(new CreateDirWindow(this.X + 10, this.Y + 8, this.Width - 16, this.Height-7, ConsoleColor.Black, ConsoleColor.White, this.Panels));
        }

        public void MoveButton()
        {
            Application.OpenWindow(new MoveWindow(this.X + 10, this.Y + 8, this.Width - 16, this.Height - 20, ConsoleColor.Black, ConsoleColor.White, this.Panels));
        }
        public void DeleteButton()
        {
            Application.OpenWindow(new DeleteWindow(this.X + 32, this.Y + 8, this.Width - 64, this.Height - 20, ConsoleColor.Yellow, ConsoleColor.Red, this.Panels));
        }

        public void EditButton()
        {
            if (this.Panels.Get_Selected_Sys.Editable)
            {
                Application.OpenWindow(new FileEditWindow(this.X + 0, this.Y + 0, this.Width, this.Height, ConsoleColor.White, ConsoleColor.Black, this.Panels));
            }
            else
            {
                Application.OpenDialog(new ErrorWindow(this.X + 32, this.Y + 8, this.Width-30,this.Height-20, ConsoleColor.Yellow, ConsoleColor.Red, $"Cannot work with {this.Panels.Get_Selected_Sys.Get_Selected_Name_UNI}"));
            }
        }
        public void CloseAll() 
        {
            this.Close = true;
        }


        public override void Draw()
        {
            if (this.RedrawAll)
            {
                this.Drawer.Clear();
            }
            base.Draw();
        }
    }
}
