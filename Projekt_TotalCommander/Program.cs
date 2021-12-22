using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
namespace Projekt_TotalCommander
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            //Console.OutputEncoding = System.Text.Encoding.Unicode;
            Application.OpenWindow(new MainWindow(10, 10, Console.WindowWidth-10, Console.WindowHeight-10, Console.ForegroundColor, Console.BackgroundColor));
            while (true)
            {


                Application.Update();
                Application.Window.Draw();
                ConsoleKeyInfo info = Console.ReadKey(true);
                Application.Window.HandleKey(info);

                Application.CloseUpdate();

                if (Application.Window == null)
                {
                    break;
                }
            }



            //List<List<char>> block = new List<List<char>>();
            //using (StreamReader reader = new StreamReader(@"c:\users\sokol\desktop\blbost\text.txt"))
            //{
            //    while (!reader.EndOfStream)
            //    {
            //        string getLine = reader.ReadLine();
            //        block.Add(getLine.ToCharArray().ToList());
            //        block.Last().Capacity=getLine.Length;
            //    }
            //}
            //Console.ReadKey();

































            //using (StreamReader reader = new StreamReader(@"C:\windows\notepad.exe"))
            //{
            //    while (!reader.EndOfStream)
            //    {
            //        Console.WriteLine(reader.ReadLine());
            //    }
            //}
            //Console.ReadKey();
            //ConsoleUtils2 console = new ConsoleUtils2(0, 0, 120, 10, ConsoleColor.Black, ConsoleColor.Yellow);
            //string n = new string('0', 117);
            //console.Write(n);
            //console.Write("SKT");
            //console.WriteLine();

            ////console.WriteLine("SKT");
            ////console.WriteLine("SKT");

            ////console.WriteLine("T");
            //Console.SetCursorPosition(0, 10);
            //Console.Write(console.CursorX);
            //console.WriteLine();
            //Console.SetCursorPosition(120, 0);
            //Console.Write(9);
            //for (int i = 0; i < Console.BufferWidth; i++)
            //{
            //    Console.Write(0);
            //    if (i == Console.BufferWidth - 1)
            //    {
            //        Console.WriteLine();
            //        Console.Write(i);
            //    }
            //}


            //List<string> list = new List<string>();
            //list.Add("1");
            //list.Add("2");
            //list.Add("3");
            //list.Add("4");
            //list.Add("5");

            //foreach (string item in list)
            //{
            //    Console.WriteLine(item);
            //}

            //list.Insert(5, "3a");
            //Console.WriteLine();

            //foreach (string item in list)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.ReadKey();

            //Console.WriteLine(Path.GetFileName(@"C:\users"));
            //Console.ReadKey();
            //
            //TODO - Refactoring of Window Heights
            ///
            ///

            //cesty
            //DirectoryInfo dir = new DirectoryInfo(@"C:\users\");
            //FileInfo file = new FileInfo(@"C:\users\sokol\desktop\sd.txt\");

            //Console.WriteLine(dir.FullName);
            //Console.WriteLine(file.FullName);
            //Console.WriteLine(Path.Combine(dir.FullName,file.FullName));
            //Console.WriteLine(file.Exists);
            //Console.WriteLine(dir.Exists);

            //špatný způsob
            //string temp = "";
            //using (StreamWriter writer = new StreamWriter(@"C:\users\sokol\desktop\sd.txt"))
            //{
            //    ConsoleKeyInfo key;
            //while ((key = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            //    {
            //        Console.SetCursorPosition(0, 0);
            //    //writer.Write(key.KeyChar);
            //    if (key.Key == ConsoleKey.Enter)
            //    {
            //        temp += "\n";
            //    }
            //    else
            //    {
            //        temp += key.KeyChar;
            //    }
            //    Console.Write(temp);
            //}
            //    writer.Write(temp);
            //}

            //List<string> data = FileUtils.GetContentOfFile(@"C:\users\nguyentuananh\desktop\prazdny.txt");
            //foreach (string item in data)
            //{
            //    Console.Write(item);
            //}
            //Console.ReadKey();
        }
    }
}
