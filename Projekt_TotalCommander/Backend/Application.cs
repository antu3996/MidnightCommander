using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    public static class Application
    {
        public static Window Window { get; set; }
        public static List<Window> OpenedWindows { get; set; } = new List<Window>();
        public static List<Window> Dialogs { get; set; } = new List<Window>();

        public static void CloseWindows()
        {
            for (int i = 0; i < OpenedWindows.Count; i++)
            {
                if (OpenedWindows[i].Close)
                {
                    OpenedWindows.RemoveAt(i);
                    ResetAll();

                }
            }
            if (OpenedWindows.Count > 0)
            {
                Window = OpenedWindows.Last();
            }
            else
            {
                Window = null;
            }
        }

        public static void CloseUpdate()
        {
            if (Dialogs.Count > 0)
            {
                for (int i = 0; i < Dialogs.Count; i++)
                {
                    if (Dialogs[i].Close)
                    {
                        Dialogs.RemoveAt(i);
                        ResetAll();

                    }
                }
                if (Dialogs.Count > 0)
                {
                    Window = Dialogs.Last();
                    ResetAll();

                }
                else
                {
                    CloseWindows();
                }
            }
            else
            {
                CloseWindows();
            }
        }
        public static void Update()
        {
                Window.Update();
        }
        public static void OpenWindow(Window window)
        {
            OpenedWindows.Add(window);
            Window = window;
        }
        public static void OpenDialog(Window dialog)
        {
            Dialogs.Add(dialog);
            Window = dialog;
        }
        //public static void CloseWindow()
        //{
        //    OpenedWindows.RemoveAt(OpenedWindows.Count - 1);
        //    if (OpenedWindows.Count > 0)
        //    {
        //        Window = OpenedWindows.Last();
        //        //Window.RedrawAll = true;
        //        RedrawAll();
        //    }
        //    else
        //    {
        //        Window = null;
        //    }
        //}
        public static void ResetAll()
        {
            foreach (Window item in OpenedWindows)
            {
                item.RedrawAll = true;
                item.Update();
                item.Draw();
            }
            foreach (Window item in Dialogs)
            {
                item.RedrawAll = true;
                item.Update();
                item.Draw();
            }

        }
    }
}
