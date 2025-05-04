using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPOS.Utilities
{
    public class ShowScreen
    {
        public static void ShowMessage(string message, string title = "Thông báo", MessageDialogIcon icon = MessageDialogIcon.Information)
        {
            Guna2MessageDialog messageDialog = new Guna2MessageDialog
            {
                Text = title,
                Caption = message,
                Buttons = MessageDialogButtons.OK,
                Icon = icon,
                Style = MessageDialogStyle.Light
            };

            messageDialog.Show();
        }
        public static DialogResult ShowConfirmDialog(string message, string title = "Xác nhận")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

    }
}
