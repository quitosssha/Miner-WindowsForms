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
    /// so you can use DoubleClickableButton to get:
    /// 1. bool this.DoubleClicked
    /// 2. bool this.ClickedWithBothMouseButtons
    /// </summary>
    public class DoubleClickableButton : Button
    {
        private DateTime lastClickTime = DateTime.MinValue;
        private bool leftButtonClicked = false;
        private bool rightButtonClicked = false;

        public bool DoubleClicked
        {
            get =>
                (DateTime.Now - lastClickTime).TotalMilliseconds
                <= SystemInformation.DoubleClickTime;
        }

        public bool ClickedWithBothMouseButtons
        {
            get => leftButtonClicked && rightButtonClicked;
        }

        public DoubleClickableButton()
        {
            MouseClick += (sender, args) => lastClickTime = DateTime.Now;

            MouseDown += (sender, args) => SetButtonClicked(args, true);

            MouseUp += (sender, args) => SetButtonClicked(args, false);
        }

        private void SetButtonClicked(MouseEventArgs args, bool isClicked)
        {
            if (args.Button == MouseButtons.Left)
                leftButtonClicked = isClicked;
            if (args.Button == MouseButtons.Right)
                rightButtonClicked = isClicked;
        }
    }
}
