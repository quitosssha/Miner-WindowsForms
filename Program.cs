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
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            var game = new GameModel(10, 10);
            Application.Run(new Form1(game) { ClientSize = new Size(300, 300)});
        }
    }
}
