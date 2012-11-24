using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace TLABS.Controls
{
    public class NumericTextBox:TextBox
    {
        public NumericTextBox()
        {
            this.KeyPress += new KeyPressEventHandler(NumericTextBox_KeyPress);
        }

        void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CheckForNumeric(e.KeyChar) == false)
                e.Handled = true;
        }

        static bool CheckForNumeric(char ch)
        {
            //allow only numbers and a decimal point and backspace key
            int keyInt = (int)ch;
            if ((keyInt < 48 || keyInt > 57) && keyInt != 46 && keyInt != 8 && ch != '-')
                return false;
            else
                return true;
        }
    }
}
