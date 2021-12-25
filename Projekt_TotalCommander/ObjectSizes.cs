using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_TotalCommander
{
    static class ObjectSizes
    {
        //ONLY VARIABLES AND REFRESH METHOD FOR WINDOWSIZECHANGE
        //
        //
        public static (int X, int Y, int W, int H) WindowSize
        {
            get { return (0, 0, Console.WindowWidth, Console.WindowHeight); }
        }
        public static (int X, int Y, int W, int H) FuncWindowSize
        {
            get {
                int tempW = (int)Math.Ceiling(WindowSize.W*0.8);
                int tempH = (int)Math.Ceiling(WindowSize.H*0.4);

                return ((WindowSize.W-tempW)/2,(WindowSize.H-tempH)/2,tempW,tempH); 
            }
        }
        public static (int X, int Y, int W, int H) DialogSize
        {
            get
            {
                int tempW = (int)Math.Ceiling(WindowSize.W * 0.33);
                int tempH = (int)Math.Ceiling(WindowSize.H * 0.4);

                return ((WindowSize.W - tempW) / 2, (WindowSize.H - tempH) / 2, tempW, tempH);
            }
        }
    }
}
