using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner
{
    public partial class StartMenuForm : Form
    {
        public Level Level;
        public StartMenuForm()
        {
            //Button easybutton = new Button();
            //Controls.Add(easybutton);
            //easybutton.Click += (sender, args) =>
            //{
            //    Level = new Level(10, 10, 10);
            //    Close();
            //};

            for (int i = 0; i < 3; i++)
            {
                var index = i;
                var button = new Button()
                {
                    Location = new Point(0, i * 30),
                    Text = $"Level {index + 1}"
                };
                Controls.Add(button);
                button.Click += (sender, args) => 
                {
                    Level = new Level(10 + 5 * index, 10 + 5 * index, 10 + 20 * index);
                    Close();
                };
            }
        }
    }
}
