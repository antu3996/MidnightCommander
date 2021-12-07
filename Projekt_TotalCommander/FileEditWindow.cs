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
        private TextEditor editor;

        public FileEditWindow(int x, int y, int w, int h, ConsoleColor fore_Col, ConsoleColor back_Col,MainPanels panels) : base(x, y, w, h, fore_Col, back_Col)
        {
            this.Panels = panels;
            this.fileservice = this.Panels.Get_Selected_Sys.GetFileUtilsOfSelected();

            TextEditor text_editor = new TextEditor(true,this.X,this.Y,this.Width,this.Height-1,this.fileservice,ConsoleColor.Black,ConsoleColor.Yellow,
                ConsoleColor.White,ConsoleColor.DarkBlue,ConsoleColor.White,ConsoleColor.Blue);
            this.editor = text_editor;
            int btn_Width = (this.Width / 9) - 1;
            int btn_Y = this.Height - 1;
            Button save = new Button(false, this.X, btn_Y, btn_Width, 1, "2 Save", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);
            Button quit = new Button(false,  btn_Width + 1, btn_Y, btn_Width, 1, "10 Quit", ConsoleColor.Black, ConsoleColor.Cyan, ConsoleColor.Green);

            save.Click += SaveFile;
            save.FuncKey = ConsoleKey.F2;

            quit.Click += CloseWindow;
            quit.FuncKey = ConsoleKey.F10;

            this.modules.AddRange(new IModule[] { text_editor,save, quit });
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
    }
}
