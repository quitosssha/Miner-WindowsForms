using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miner.Model
{
    /// <summary>
    /// In Windows Forms common Button.DoubleClick if unaviable, 
    /// so you can use DoubleClickableButton to get
    /// bool this.DoubleClicked
    /// </summary>
    public class DoubleClickableButton : Button
    {
        private DateTime lastClickTime = DateTime.MinValue;
        public bool DoubleClicked
        {
            get => (DateTime.Now - lastClickTime).TotalMilliseconds <= SystemInformation.DoubleClickTime;
        }

        public DoubleClickableButton()
        {
            MouseClick += (sender, args) =>
            {
                lastClickTime = DateTime.Now;
            };
        }
    }
}
