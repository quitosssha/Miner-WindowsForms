using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner
{
    internal static class Program
    {
        public static int width;
        public static int height;
        public static int mines;

        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            StartMenuForm startForm = new StartMenuForm();
            Application.Run(startForm);
            Application.Run(new Form1(startForm.Level));
        }
    }
}
