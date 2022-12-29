using System.Windows.Forms;
using System;

namespace AppBancoWinForms.Utils
{
    internal class Controles
    {
        public static void LimpaCaixasTextos(Control.ControlCollection parentControl)
        {
            foreach (Control c in parentControl)
            {
                if (c.HasChildren)
                {
                    //Recursively loop through the child controls
                    LimpaCaixasTextos(c.Controls);
                }
                else
                {
                    if (c is TextBox || c is MaskedTextBox)
                    {
                        c.Text = "";
                    }
                }
            }
        }
    }
}
